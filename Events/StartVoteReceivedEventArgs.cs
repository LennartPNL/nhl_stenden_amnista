using System.Net.Sockets;

namespace Amnista.Events
{
    class StartVoteReceivedEventArgs
    {
        public Socket Client { get; set; }

        public StartVoteReceivedEventArgs(Socket client)
        {
            Client = client;
        }
    }
}
