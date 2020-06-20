using System;
using System.Windows.Input;
using Amnista.Generic;
using Amnista.Models;

namespace Amnista.View_Models
{
    class SettingsViewModel : ObservableObject
    {
        /// <summary>
        /// Fields
        /// </summary>
        private readonly Settings _settings;
        private string _serverIp = Properties.Settings.Default.server_ip;

        public SettingsViewModel()
        {
            _settings = new Settings();
        }
        public string ServerIp
        {
            get => Properties.Settings.Default.server_ip == null ? "" : _serverIp;
            set
            {
                _serverIp = value;
                OnPropertyChanged(nameof(ServerIp));
            }
        }
        public ICommand SaveSettingsCommand => new Commander(SaveSettings);

        public void SaveSettings()
        {
            _settings.ServerIp = _serverIp;
            
            Properties.Settings.Default.server_ip = _serverIp;
            Properties.Settings.Default.Save();
        }
    }
}