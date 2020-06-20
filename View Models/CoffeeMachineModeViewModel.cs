using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Amnista.Generic;
using Amnista.Models;

namespace Amnista.View_Models
{
    class CoffeeMachineModeViewModel : ObservableObject
    {
        private readonly Server _server;
        private string _serverResponse;
        public List<ClientProfile> _clientProfiles = new List<ClientProfile>();


        public string ServerResponse
        {
            get => _serverResponse;
            set
            {
                _serverResponse = value;
                OnPropertyChanged("ServerResponse");
            }
        }

        public int ClientProfilesDidVote => _server.ClientProfilesDidVote;

        public string ServerIP => (_server.Started) ? _server.ServerIP : "not started";

        public int ClientsOnline => _server.ServerProfileManager.Profiles.Count == null
            ? 0
            : _server.ServerProfileManager.Profiles.Count;

        public List<ClientProfile> ClientProfiles
        {
            get => _clientProfiles;
            set
            {
                _clientProfiles = value;
                OnPropertyChanged();
            }
        }

        public bool ServerEnabled
        {
            get => !_server.Started;
        }

        public CoffeeMachineModeViewModel()
        {
            _server = new Server();
            _server.PropertyChanged += ServerOnPropertyChanged;
        }


        public ICommand StartServerCommand => new Commander(StartServer);

        public void StartServer()
        {
            _server.Start();
        }

        private void ServerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ClientsOnline));

            switch (e.PropertyName)
            {
                case "ServerResponse":
                    Application.Current.Dispatcher.BeginInvoke(() =>
                    {
                        ServerResponse = _server.ServerResponse;
                        OnPropertyChanged();
                    });
                    break;
                case "Profiles":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ClientProfiles.AddRange(_server.ServerProfileManager.Profiles);
                        ClientProfiles.Reverse();
                        OnPropertyChanged("ClientProfiles");
                    });
                    break;
                case nameof(ServerIP):
                    Application.Current.Dispatcher.Invoke(() => { OnPropertyChanged(nameof(ServerIP)); });
                    break;
                case nameof(ClientProfilesDidVote):
                    Application.Current.Dispatcher.Invoke(() => { OnPropertyChanged(nameof(ClientProfilesDidVote)); });
                    break;
                case nameof(ClientsOnline):
                    Application.Current.Dispatcher.Invoke(() => { OnPropertyChanged(nameof(ClientsOnline)); });
                    break;
                case nameof(_server.Started):
                    Application.Current.Dispatcher.Invoke(() => { OnPropertyChanged(nameof(ServerEnabled)); });
                    break;
            }
        }
    }
}