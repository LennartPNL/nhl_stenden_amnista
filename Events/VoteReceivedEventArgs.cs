using System.Net.Sockets;

namespace Amnista.Events
{
    public class VoteReceivedEventArgs
    {
        public Socket Client { get; set; }

        public VoteReceivedEventArgs(Socket client)
        {
            Client = client;
        }
    }
}
