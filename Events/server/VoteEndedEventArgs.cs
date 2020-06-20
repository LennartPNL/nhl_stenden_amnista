using Amnista.Models;

namespace Amnista.Events
{
    public class VoteEndedEventArgs
    {
        public ClientProfile ClientProfile { get; set; }

        public VoteEndedEventArgs(ClientProfile clientProfile)
        {
            ClientProfile = clientProfile;
        }
    }
}