using System.Net.Sockets;

namespace Amnista.Events
{
    class VoteReceivedEventArgs
    {
        public Socket Client { get; set; }

        public VoteReceivedEventArgs(Socket client)
        {
            Client = client;
        }
    }
}
