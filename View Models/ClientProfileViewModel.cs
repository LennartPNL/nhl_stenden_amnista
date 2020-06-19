using System;
using System.Windows.Input;
using Amnista.Data_Types.Enums;
using Amnista.Generic;
using Amnista.Models;

namespace Amnista.View_Models
{
    class ClientProfileViewModel : ObservableObject
    {

        /// <summary>
        /// Fields
        /// </summary>
        private ClientProfile _clientProfile;
        private string _name;
        private Status _status;
        private DrinkType _drinkType;
        private bool _withMilk;
        private bool _withSugar;
        private int _coffeePoints;

        /// <summary>
        /// Constructor
        /// </summary>
        public ClientProfileViewModel()
        {
            _clientProfile = new ClientProfile
            {
                DrinkPreference = new DrinkPreference()
            };
        }

        /// <summary>
        /// All bindings of ClientProfileView
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public Status Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        public DrinkType DrinkType
        {
            get { return _drinkType; }
            set
            {
                _drinkType = value;
                OnPropertyChanged(nameof(DrinkType));
            }
        }
        public bool WithMilk
        {
            get { return _withMilk; }
            set
            {
                _withMilk = value;
                OnPropertyChanged(nameof(WithMilk));
            }
        }
        public bool WithSugar
        {
            get { return _withSugar; }
            set
            {
                _withSugar = value;
                OnPropertyChanged(nameof(WithSugar));
            }
        }
        public int CoffeePoints
        {
            get { return _coffeePoints; }
            set
            {
                _coffeePoints = value;
                OnPropertyChanged(nameof(CoffeePoints));
            }
        }
        
        /// <summary>
        /// Methods
        /// </summary>
        public ICommand SaveProfileCommand => new Commander(SaveProfile);

        public void SaveProfile()
        {
            _clientProfile.Name = _name;
            _clientProfile.status = _status;
            _clientProfile.DrinkPreference.DrinkType = _drinkType;
            _clientProfile.DrinkPreference.WithMilk = _withMilk;
            _clientProfile.DrinkPreference.WithSugar = _withSugar;
            _clientProfile.CoffeePoints = _coffeePoints;

            Properties.Settings.Default.user_name = _name;
            Properties.Settings.Default.user_status = _status.ToString();
            Properties.Settings.Default.user_drinktype = _drinkType.ToString();
            Properties.Settings.Default.user_withmilk = _withMilk.ToString();
            Properties.Settings.Default.user_withsugar = _withSugar.ToString();
            Properties.Settings.Default.user_coffeePoints = _coffeePoints;
            Properties.Settings.Default.Save();
        }

        public int GetCoffeePoints()
        {
            return 1;
        }
    }
}