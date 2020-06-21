using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Amnista.Events;
using Amnista.Events.client;
using Amnista.Generic.client.Server.Commands;
using Amnista.Generic.Server;
using Amnista.Generic.Server.Commands;
using Newtonsoft.Json;

namespace Amnista.Generic
{
    /// <summary>
    /// The ClientSocket handles all the socket communication for the client
    /// </summary>
    public class ClientSocket
    {
        // Data buffer for incoming data.  
        byte[] bytes = new byte[1024];
        private Socket server;
        public bool IsRunning { get; set; }

        /// <summary>
        /// Connects the socket if possible
        /// </summary>
        public void StartClient()
        {
            IPEndPoint serverEp = new IPEndPoint(IPAddress.Parse(Properties.Settings.Default.server_ip), 11000);

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.ReceiveTimeout = -1;

            // Connect to the server.
            while (!server.Connected)
            {
                try
                {
                    server.Connect(serverEp);
                    IsRunning = true;
                    new Thread(() => { HandleConnection(server); }).Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Establish connection with server (" + serverEp + ") failed!");
                    Debug.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Sends a message to the server
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            server.Send(Encoding.ASCII.GetBytes(message));
            Console.WriteLine();
        }

        /// <summary>
        /// Closes the connection
        /// </summary>
        public void CloseConnection()
        {
            if (IsRunning)
            {
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                IsRunning = false;
            }
        }

        /// <summary>
        /// Converts an object to json and adds a command to it. Once it's converted it will be sent to the server
        /// </summary>
        /// <param name="command">Command to send to the server</param>
        /// <param name="payload">Object that should be sent to the server</param>
        public void SendCommand(string command, object payload)
        {
            if (IsRunning)
            {
                ServerCommand serverCommand = (ServerCommand) payload;
                serverCommand.Command = command;
                Debug.WriteLine(JsonConvert.SerializeObject(serverCommand));
                SendMessage(JsonConvert.SerializeObject(serverCommand));
            }
        }

        /// <summary>
        /// Start or participates in a vote round
        /// </summary>
        public void Vote()
        {
            SendCommand("start_vote", new StartVoteCommand());
        }

        /// <summary>
        /// Reads the incoming socket bytes
        /// </summary>
        /// <param name="client"></param>
        void HandleConnection(Socket client)
        {
            Console.WriteLine("Connection with server () established!");

            const int maxMessageSize = 1024;
            while (true)
            {
                var response = new byte[maxMessageSize];
                var received = client.Receive(response);

                if (received == 0)
                {
                    return;
                }

                var respBytesList = new List<byte>(response);
                respBytesList.RemoveRange(received, maxMessageSize - received);

                string message = Encoding.ASCII.GetString(respBytesList.ToArray());

                string serverCommand = JsonConvert.DeserializeObject<ServerCommand>(message).Command;

                Debug.WriteLine("Command " + serverCommand + "received");
                switch (serverCommand)
                {
                    case "start_vote":
                        ClientVotedCommand votedClient1 = JsonConvert.DeserializeObject<ClientVotedCommand>(message);

                        VoteStartedEvent(new VoteStartedEventArgs(votedClient1.Client));
                        break;
                    case "client_voted":
                        ClientVotedCommand votedClient2 = JsonConvert.DeserializeObject<ClientVotedCommand>(message);
                        VoteReceivedEvent(new ClientVoteReceivedEventArgs(votedClient2.Client));
                        break;
                    case "end_vote":
                        VoteEndedCommand winnerClient = JsonConvert.DeserializeObject<VoteEndedCommand>(message);
                        VoteEndedEvent(new VoteEndedEventArgs(winnerClient.Winner)
                        {
                            Clients = winnerClient.Clients
                        });
                        break;
                }
            }
        }

        protected virtual void VoteReceivedEvent(ClientVoteReceivedEventArgs e)
        {
            EventHandler<ClientVoteReceivedEventArgs> handler = VoteReceived;
            handler?.Invoke(this, e);
        }

        protected virtual void VoteStartedEvent(VoteStartedEventArgs e)
        {
            EventHandler<VoteStartedEventArgs> handler = VoteStarted;
            handler?.Invoke(this, e);
        }

        protected virtual void VoteEndedEvent(VoteEndedEventArgs e)
        {
            EventHandler<VoteEndedEventArgs> handler = VoteEnded;
            handler?.Invoke(this, e);
        }

        public event EventHandler<ClientVoteReceivedEventArgs> VoteReceived;
        public event EventHandler<VoteStartedEventArgs> VoteStarted;
        public event EventHandler<VoteEndedEventArgs> VoteEnded;
    }
}