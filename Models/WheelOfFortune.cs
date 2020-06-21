using Amnista.Events.client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using System.Windows.Documents;
using Amnista.Generic;

namespace Amnista.Models
{
    class WheelOfFortune
    {
        private Timer timer;
        private const int interval = 200;
        private int rotationNr = 0;
        private ClientSocket _clientSocket;

        public List<ClientProfile> ClientProfiles { get; set; }

        public ClientProfile Winner { get; set; }

        public ClientSocket ClientSocket
        {
            get { return _clientSocket; } 
            set
            {
                _clientSocket = value;
                _clientSocket.VoteEnded += _clientSocket_VoteEnded;
            } 
        }

        public WheelOfFortune()
        {

            ClientProfiles = new List<ClientProfile>();

            //Sets the text while waiting for the winner to be determined 
            Winner = new ClientProfile
            {
                Name = "Waiting..."
            };
        }

        /// <summary>
        /// Updates the name that should be displayed every time the set interval expires
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (ClientProfiles.Count > 0)
            {
                if (++rotationNr >= ClientProfiles.Count)
                {
                    rotationNr = 0;
                }
                Winner = ClientProfiles[rotationNr];
                WheelUpdatedEvent(new WheelOfFortuneUpdatedEventArgs(Winner));
            }
        }

        /// <summary>
        /// Creates the timer and sets the elapsed event
        /// </summary>
        public void Spin()
        {
            timer = new Timer(interval);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
        }

        /// <summary>
        /// Stops the wheel, sets the winner and calls the spinner ended event
        /// </summary>
        /// <param name="winner">Winner of the poll</param>
        public void Stop(ClientProfile winner)
        {
            timer.Stop();
            Winner = winner;
            SpinnerEndedEvent();
        }

        /// <summary>
        /// Initiates a spin of 5 seconds after the winner has been declared and shows the result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Results of the vote</param>
        private void _clientSocket_VoteEnded(object sender, Events.VoteEndedEventArgs e)
        {
            ClientProfiles.AddRange(e.Clients);
            Spin();
            System.Threading.Thread.Sleep(5000);
            Stop(e.Winner);
        }

        /// <summary>
        /// Should be called when the vote has ended
        /// </summary>
        protected virtual void SpinnerEndedEvent()
        {
            EventHandler handler = VoteEnded;
            handler?.Invoke(this, null);
        }

        /// <summary>
        /// Should be called when the displayed name should be changed
        /// </summary>
        /// <param name="e">client profile of the name that should be displayed</param>
        protected virtual void WheelUpdatedEvent(WheelOfFortuneUpdatedEventArgs e)
        {
            EventHandler<WheelOfFortuneUpdatedEventArgs> handler = WheelUpdated;
            handler?.Invoke(this, e);
        }

        public event EventHandler VoteEnded;
        public event EventHandler<WheelOfFortuneUpdatedEventArgs> WheelUpdated;
    }
}