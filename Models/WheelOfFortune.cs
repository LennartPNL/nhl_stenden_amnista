﻿using Amnista.Events.client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using System.Windows.Documents;

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
            Winner = new ClientProfile
            {
                Name = "Waiting..."
            };
        }

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

        public void Spin()
        {
            timer = new Timer(interval);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
        }

        public void Stop(ClientProfile winner)
        {
            timer.Stop();
            Winner = winner;
            SpinnerEndedEvent();
        }

        private void _clientSocket_VoteEnded(object sender, Events.VoteEndedEventArgs e)
        {
            ClientProfiles.AddRange(e.Clients);
            Spin();
            System.Threading.Thread.Sleep(5000);
            Stop(e.Winner);
        }

        protected virtual void SpinnerEndedEvent()
        {
            EventHandler handler = VoteEnded;
            handler?.Invoke(this, null);
        }

        protected virtual void WheelUpdatedEvent(WheelOfFortuneUpdatedEventArgs e)
        {
            EventHandler<WheelOfFortuneUpdatedEventArgs> handler = WheelUpdated;
            handler?.Invoke(this, e);
        }

        public event EventHandler VoteEnded;
        public event EventHandler<WheelOfFortuneUpdatedEventArgs> WheelUpdated;
    }
}