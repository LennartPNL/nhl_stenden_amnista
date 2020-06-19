using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Amnista.Events;
using Amnista.Models;

namespace Amnista.Generic
{
    public class VoteManager
    {
        private List<ClientProfile> _clientProfilesDidVote = new List<ClientProfile>();

        private System.Timers.Timer _timer = new System.Timers.Timer(5000);

        public VoteManager()
        {
            _timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            ClientProfile chosenClientProfile =
                _clientProfilesDidVote[new Random().Next(0, _clientProfilesDidVote.Count)];
            _clientProfilesDidVote.ForEach(client =>
            {
                client.Socket.Send(Encoding.ASCII.GetBytes(chosenClientProfile.Name + " must get drinks!"));
            });
            VoteEndedEvent(new VoteEndedEventArgs()
            {
                ClientProfile = chosenClientProfile
            });
            _timer.Stop();
            Reset();
        }

        public bool VoteHasStarted => _clientProfilesDidVote.Count > 0;

        public int ClientProfilesDidVote => _clientProfilesDidVote.Count;

        public void Vote(ClientProfile clientProfile)
        {
            if (!VoteHasStarted)
            {
                _timer.Start();
            }

            if (!_clientProfilesDidVote.Contains(clientProfile))
            {
                _clientProfilesDidVote.Add(clientProfile);
            }
        }

        public void Reset()
        {
            lock (_clientProfilesDidVote)
            {
                _clientProfilesDidVote.Clear();
            }
        }

        protected virtual void VoteEndedEvent(VoteEndedEventArgs e)
        {
            EventHandler<VoteEndedEventArgs> handler = VoteEnded;
            handler?.Invoke(this, e);
        }

        public event EventHandler<VoteEndedEventArgs> VoteEnded;
    }
}