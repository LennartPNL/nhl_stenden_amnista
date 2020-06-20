using Amnista.Generic.Server;
using Amnista.Models;

namespace Amnista.Generic.client.Server.Commands
{
    public class VoteEndedCommand : ServerCommand
    {
        public ClientProfile Winner { get; set; }
    }
}