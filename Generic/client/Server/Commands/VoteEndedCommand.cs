using System.Collections.Generic;
using System.Windows.Documents;
using Amnista.Generic.Server;
using Amnista.Models;

namespace Amnista.Generic.client.Server.Commands
{
    public class VoteEndedCommand : ServerCommand
    {
        public List<ClientProfile> Clients { get; set; }
        public ClientProfile Winner { get; set; }
    }
}