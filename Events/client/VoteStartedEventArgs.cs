using Amnista.Models;

namespace Amnista.Events.client
{
    public class VoteStartedEventArgs
    {
        public ClientProfile ClientProfile { get; set; }

        public VoteStartedEventArgs(ClientProfile clientProfile)
        {
            ClientProfile = clientProfile;
        }
    }
}
