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

namespace Amnista.Models
{
    public class ClientSocket
    {
        // Data buffer for incoming data.  
        byte[] bytes = new byte[1024];
        private Socket server;
        private ClientProfile _ownProfile = new ClientProfile();
        public bool IsRunning { get; set; }

        public void StartClient()
        {
            // TODO: This needs to come from the settings page and be updated on change
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

        public void SendMessage(string message)
        {
            const int maxMessageSize = 1024;
            byte[] response;
            int received;
            server.Send(Encoding.ASCII.GetBytes(message));
            Console.WriteLine();
        }

        public void CloseConnection()
        {
            if (IsRunning)
            {
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                IsRunning = false;
            }
        }

        public void SendCommand(string command, object payload)
        {
            if (IsRunning)
            {
                ServerCommand serverCommand = (ServerCommand) payload;
                serverCommand.Command = command;
                SendMessage(JsonConvert.SerializeObject(serverCommand));
            }
        }

        public void Vote()
        {
            SendCommand("start_vote", new StartVoteCommand());
        }

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