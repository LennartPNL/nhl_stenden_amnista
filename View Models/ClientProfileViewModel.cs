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
        private readonly ClientProfile _clientProfile;

        private string _name = Properties.Settings.Default.user_name;
        private Status _status = ParseEnum<Status>(Properties.Settings.Default.user_status);
        private DrinkType _drinkType = ParseEnum<DrinkType>(Properties.Settings.Default.user_drinktype);
        private bool _withMilk = Properties.Settings.Default.user_withmilk;
        private bool _withSugar = Properties.Settings.Default.user_withsugar;
        private int _coffeePoints = Properties.Settings.Default.user_coffeePoints;
        private int _onlineUsers;

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
            get => Properties.Settings.Default.user_name == null ? "" : _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public Status Status
        {
            get => Properties.Settings.Default.user_status == null ? Status.OFFLINE : _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public DrinkType DrinkType
        {
            get => Properties.Settings.Default.user_drinktype == null ? DrinkType.WATER : _drinkType;
            set
            {
                _drinkType = value;
                OnPropertyChanged(nameof(DrinkType));
            }
        }

        public bool WithMilk
        {
            get => Properties.Settings.Default.user_withmilk == null ? false : _withMilk;
            set
            {
                _withMilk = value;
                OnPropertyChanged(nameof(WithMilk));
            }
        }

        public bool WithSugar
        {
            get => Properties.Settings.Default.user_withsugar == null ? false : _withSugar;
            set
            {
                _withSugar = value;
                OnPropertyChanged(nameof(WithSugar));
            }
        }

        public int CoffeePoints
        {
            get => Properties.Settings.Default.user_coffeePoints == null ? 0 : _coffeePoints;
            set
            {
                _coffeePoints = value;
                OnPropertyChanged(nameof(CoffeePoints));
            }
        }
        public int OnlineUsers
        {
            get => _onlineUsers;
            set
            {
                _onlineUsers = value;
                OnPropertyChanged(nameof(OnlineUsers));
            }
        }

        /// <summary>
        /// Helper method for converting string to Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(string value)
        {
            return (T) Enum.Parse(typeof(T), value, ignoreCase: true);
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
            Properties.Settings.Default.user_withmilk = _withMilk;
            Properties.Settings.Default.user_withsugar = _withSugar;
            Properties.Settings.Default.user_coffeePoints = _coffeePoints;
            Properties.Settings.Default.Save();
        }

        public int GetCoffeePoints()
        {
            return 1;
        }
    }
}