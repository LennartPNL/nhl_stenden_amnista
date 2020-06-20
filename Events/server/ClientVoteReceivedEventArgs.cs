using Amnista.Models;

namespace Amnista.Events
{
    class ClientVoteReceivedEventArgs
    {
        public ClientProfile Client { get; set; }

        public ClientVoteReceivedEventArgs(ClientProfile client)
        {
            Client = client;
        }
    }
}
