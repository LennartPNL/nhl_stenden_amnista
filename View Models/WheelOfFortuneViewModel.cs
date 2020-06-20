using Amnista.Generic;
using Amnista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace Amnista.View_Models
{
    class WheelOfFortuneViewModel : ObservableObject
    {
        private readonly WheelOfFortune _wheelOfFortune;
        private Timer timer;
        private const int interval = 200;
        private int rotationNr = 0;
        private string _winnerImg;

        public List<ClientProfile> ClientProfiles
        {
            get { return _wheelOfFortune.ClientProfiles; }
            set
            {
                _wheelOfFortune.ClientProfiles = value;
                OnPropertyChanged(nameof(ClientProfiles));
            }
        }

        public ClientProfile Winner
        {
            get { return _wheelOfFortune.Winner; }
            set
            {
                _wheelOfFortune.Winner = value;
                OnPropertyChanged(nameof(Winner));
            }
        }

        public string WinnerImg
        {
            get { return _winnerImg; }
            set
            {
                _winnerImg = value;
                OnPropertyChanged(nameof(WinnerImg));
            }
        }

        public WheelOfFortuneViewModel()
        {
            _wheelOfFortune = new WheelOfFortune();
            WinnerImg = "pack://siteoforigin:,,,/Resources/and-the-winner-is1170px.jpg";
        }

        public ICommand LoadedCommand => new Commander(Spin);

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            timer.Stop();

            if (++rotationNr == ClientProfiles.Count)
            {
                rotationNr = 0;
            }
            Winner = ClientProfiles[rotationNr];

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
            WinnerImg = "pack://siteoforigin:,,,/Resources/tcFo7yK.png";
        }
    }
}