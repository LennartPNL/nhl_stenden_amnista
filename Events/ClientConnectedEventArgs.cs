using System.Net.Sockets;

namespace Amnista.Events
{
    class ClientConnectedEventArgs
    {
        public Socket Client { get; set; }

        public ClientConnectedEventArgs(Socket client)
        {
            Client = client;
        }
    }
}
