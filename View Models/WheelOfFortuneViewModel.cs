using Amnista.Events.client;
using Amnista.Generic;
using Amnista.Models;
using System;
using System.Collections.Generic;

namespace Amnista.View_Models
{
    class WheelOfFortuneViewModel : ObservableObject
    {
        private readonly WheelOfFortune _wheelOfFortune;
        private string _winnerImg;

        public ClientSocket ClientSocket
        {
            get { return _wheelOfFortune.ClientSocket; }
            set
            {
                _wheelOfFortune.ClientSocket = value;
                OnPropertyChanged(nameof(ClientSocket));
            }
        }

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
            _wheelOfFortune.VoteEnded += _wheelOfFortune_VoteEnded;
            _wheelOfFortune.WheelUpdated += _wheelOfFortune_WheelUpdated;
            WinnerImg = "pack://siteoforigin:,,,/Resources/and-the-winner-is1170px.jpg";
        }

        private void _wheelOfFortune_WheelUpdated(object sender, WheelOfFortuneUpdatedEventArgs e)
        {
            Winner = e.ClientProfile;
        }

        private void _wheelOfFortune_VoteEnded(object sender, EventArgs e)
        {
            WinnerImg = "pack://siteoforigin:,,,/Resources/tcFo7yK.png";
        }
    }
}