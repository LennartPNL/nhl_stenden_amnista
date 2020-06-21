using System.Net.Sockets;

namespace Amnista.Events
{
    public class ClientConnectedEventArgs
    {
        public Socket Client { get; set; }

        public ClientConnectedEventArgs(Socket client)
        {
            Client = client;
        }
    }
}
