using Amnista.Generic;
using Amnista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Documents;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace Amnista.View_Models
{
    class WheelOfFortuneViewModel : ObservableObject
    {
        private readonly WheelOfFortune _wheelOfFortune;
        private string _resultText;
        private Timer timer;
        private const int interval = 200;
        private int rotationNr = 0;

        public ClientProfileManager ClientProfileManager
        {
            get { return _wheelOfFortune.ClientProfileManager; }
            set
            {
                _wheelOfFortune.ClientProfileManager = value;
                OnPropertyChanged(nameof(ClientProfileManager));
            }
        }

        public ClientProfile Winner
        {
            get { return _wheelOfFortune.Winner; }
            set
            {
                _wheelOfFortune.Winner = value;
                OnPropertyChanged(nameof(ClientProfileManager));
            }
        }

        public string ResultText
        {
            get { return _resultText; }
            set
            {
                _resultText = value;
                OnPropertyChanged(nameof(ResultText));
            }
        }

        public WheelOfFortuneViewModel()
        {
            _wheelOfFortune = new WheelOfFortune();
        }

        public ICommand LoadedCommand => new Commander(Spin);

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            List<ClientProfile> profiles = ClientProfileManager.ClientProfiles;
            timer.Stop();

            if (++rotationNr == profiles.Count)
            {
                rotationNr = 0;
            }
            ResultText = profiles[rotationNr].Name;

            timer.Enabled = true;
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
        }
    }
}