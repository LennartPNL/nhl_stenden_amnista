using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Amnista.Events;
using Amnista.Generic.client.Server.Commands;
using Amnista.Models;
using Newtonsoft.Json;

namespace Amnista.Generic
{
    /// <summary>
    /// The votemanager keeps track of all voting participants
    /// </summary>
    public class VoteManager
    {
        private List<ClientProfile> _clientProfilesDidVote = new List<ClientProfile>();

        private Timer _timer = new Timer(10000);

        public bool VoteHasStarted => _clientProfilesDidVote.Count > 0;

        public int ClientProfilesDidVote => _clientProfilesDidVote.Count;

        /// <summary>
        /// Constructor
        /// </summary>
        public VoteManager()
        {
            _timer.Elapsed += TimerOnElapsed;
        }

        /// <summary>
        /// This method is used to wait x seconds after choosing a random "winner" that needs to get coffee.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            var chosenClientProfile =
                _clientProfilesDidVote[new Random().Next(0, _clientProfilesDidVote.Count)];
            _clientProfilesDidVote.ForEach(client =>
            {
                client.Socket.Send(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new VoteEndedCommand()
                    {
                        Command = "end_vote", 
                        Winner = chosenClientProfile,
                        Clients = _clientProfilesDidVote
                    })));
                
            });
            VoteEndedEvent(new VoteEndedEventArgs(chosenClientProfile)
            {
                Winner = chosenClientProfile,
                Clients = _clientProfilesDidVote
            });
            _timer.Stop();
            Reset();
        }

        /// <summary>
        /// In case the client didn't already vote it gets added
        /// </summary>
        /// <param name="clientProfile"></param>
        public void Vote(ClientProfile clientProfile)
        {
            if (!VoteHasStarted) _timer.Start();

            if (!_clientProfilesDidVote.Contains(clientProfile)) _clientProfilesDidVote.Add(clientProfile);
        }

        /// <summary>
        /// Resets the voting round and clears all participants
        /// </summary>
        public void Reset()
        {
            lock (_clientProfilesDidVote)
            {
                _clientProfilesDidVote.Clear();
            }
        }

        /// <summary>
        /// Triggeres an event whenever the vote round ended.
        /// </summary>
        /// <param name="e">Argument containing all participants and the "winner"</param>
        protected virtual void VoteEndedEvent(VoteEndedEventArgs e)
        {
            var handler = VoteEnded;
            handler?.Invoke(this, e);
        }

        public event EventHandler<VoteEndedEventArgs> VoteEnded;
    }
}