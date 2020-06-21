using System.Collections.Generic;
using System.Windows.Documents;
using Amnista.Models;

namespace Amnista.Events
{
    public class VoteEndedEventArgs
    {
        public List<ClientProfile> Clients { get; set; }
        public ClientProfile Winner { get; set; }

        public VoteEndedEventArgs(ClientProfile winner)
        {
            Winner = winner;
        }
    }
}