using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Amnista.Events;
using Amnista.Generic.Server;
using Amnista.Models;
using Json.Net;
using Newtonsoft.Json;

namespace Amnista.Generic
{
    class SocketService
    {
        private Thread _connectionThread;
        private readonly Socket _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        /// <summary>
        /// Starts the server
        /// </summary>
        public void Start()
        {
            _server.ReceiveTimeout = -1;
            _connectionThread = new Thread(StartServerThread);
            _connectionThread.Start();
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        public void Stop()
        {
            _connectionThread.Abort();
        }

        /// <summary>
        /// Runs the server in a new thread
        /// </summary>
        private void StartServerThread()
        {
            try
            {
                _server.Bind(new IPEndPoint(IPAddress.Any, 11000));
                _server.Listen(-1);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                return;
            }

            while (true)
            {
                Socket client = _server.Accept();
                new Thread(() =>
                {
                    try
                    {
                        HandleConnection(client);
                    }
                    catch (Exception)
                    {
                        ClientDisconnectedEvent(new ClientDisconnectedEventArgs(client));
                    }
                }).Start();
            }
        }

        /// <summary>
        /// Retrieves the incoming messages from the socket
        /// </summary>
        /// <param name="client">Holding the connection</param>
        void HandleConnection(Socket client)
        {
            ClientConnectedEvent(new ClientConnectedEventArgs(client));

            const int maxMessageSize = 1024;

            while (true)
            {
                var response = new byte[maxMessageSize];
                var received = client.Receive(response);

                if (received == 0)
                {
                    ClientDisconnectedEvent(new ClientDisconnectedEventArgs(client));
                    return;
                }

                var respBytesList = new List<byte>(response);
                respBytesList.RemoveRange(received, maxMessageSize - received);

                string message = Encoding.ASCII.GetString(respBytesList.ToArray());

                string serverCommand = JsonConvert.DeserializeObject<ServerCommand>(message).Command;

                switch (serverCommand)
                {
                    case "update":
                        ClientProfile clientProfile = JsonConvert.DeserializeObject<ClientProfile>(message);
                        UpdateReceivedEvent(new ClientUpdateReceivedEventArgs(client, clientProfile));
                        break;
                    default:
                        MessageReceivedEvent(new ClientMessageReceivedEventArgs(client, message));
                        break;
                }
                
                    
         
            }
        }

        protected virtual void ClientConnectedEvent(ClientConnectedEventArgs e)
        {

            EventHandler<ClientConnectedEventArgs> handler = ClientConnected;
            handler?.Invoke(this, e);
        }

        protected virtual void MessageReceivedEvent(ClientMessageReceivedEventArgs e)
        {
            EventHandler<ClientMessageReceivedEventArgs> handler = MessageReceived;
            handler?.Invoke(this, e);
        }

        protected virtual void UpdateReceivedEvent(ClientUpdateReceivedEventArgs e)
        {
            EventHandler<ClientUpdateReceivedEventArgs> handler = UpdateReceived;
            handler?.Invoke(this, e);
        }

        protected virtual void ClientDisconnectedEvent(ClientDisconnectedEventArgs e)
        {
            EventHandler<ClientDisconnectedEventArgs> handler = ClientDisconnected;
            handler?.Invoke(this, e);
        }

        public event EventHandler<ClientConnectedEventArgs> ClientConnected;
        public event EventHandler<ClientMessageReceivedEventArgs> MessageReceived;
        public event EventHandler<ClientUpdateReceivedEventArgs> UpdateReceived;
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;
    }
}