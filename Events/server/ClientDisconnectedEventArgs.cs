using System.Net.Sockets;

namespace Amnista.Events
{
    public class ClientDisconnectedEventArgs
    {
        public Socket Client { get; set; }

        public ClientDisconnectedEventArgs(Socket client)
        {
            Client = client;
        }
    }
}
