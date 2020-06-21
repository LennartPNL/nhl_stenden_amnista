using Amnista.Models;

namespace Amnista.Generic.Server.Commands
{
    public class ClientVotedCommand : ServerCommand
    {
        public ClientProfile Client { get; set; }
    }
}
