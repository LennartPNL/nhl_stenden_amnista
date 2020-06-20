using Amnista.Models;

namespace Amnista.Events
{
    public class ClientVoteReceivedEventArgs
    {
        public ClientProfile Client { get; set; }

        public ClientVoteReceivedEventArgs(ClientProfile client)
        {
            Client = client;
        }
    }
}
