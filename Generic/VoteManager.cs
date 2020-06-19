using System.Collections.Generic;
using Amnista.Models;

namespace Amnista.Generic
{
    public class VoteManager
    {
        private List<ClientProfile> _clientProfilesDidVote = new List<ClientProfile>();

        public bool VoteHasStarted => _clientProfilesDidVote.Count > 0;

        public int ClientProfilesDidVote => _clientProfilesDidVote.Count;

        public void Vote(ClientProfile clientProfile)
        {
            if (!_clientProfilesDidVote.Contains(clientProfile))
            {
                _clientProfilesDidVote.Add(clientProfile);
            }
        }

        public void Reset()
        {
            _clientProfilesDidVote.Clear();
        }
    }
}