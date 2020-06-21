using System.Net.Sockets;
using Amnista.Models;

namespace Amnista.Events
{
    public class ClientUpdateReceivedEventArgs
    {
        public Socket Client { get; set; }
        public ClientProfile ClientProfile { get; set; }

        public ClientUpdateReceivedEventArgs(Socket client, ClientProfile clientProfile)
        {
            Client = client;
            ClientProfile = clientProfile;
        }
    }
}