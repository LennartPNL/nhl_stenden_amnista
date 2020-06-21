using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Amnista.Annotations;
using Amnista.Events;
using Amnista.Generic;

namespace Amnista.Models
{
    class CoffeeMachine : INotifyPropertyChanged
    {
        private readonly Server _server;
        private string _serverResponse;
        public List<ClientProfile> _clientProfiles = new List<ClientProfile>();
        public List<ClientProfile> VotedClients { get; set; }

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

        public bool ServerEnabled => !_server.Started;

        /// <summary>
        /// Constructor
        /// </summary>
        public CoffeeMachine()
        {
            _server = new Server();
            _server.PropertyChanged += ServerOnPropertyChanged;
            MainWindow.ClientSocket.VoteEnded += ClientSocketOnVoteEnded;
        }

        /// <summary>
        /// Event gets triggered whenever a vote round has finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientSocketOnVoteEnded(object sender, VoteEndedEventArgs e)
        {
            VotedClients = e.Clients;
            OnPropertyChanged(nameof(VotedClients));
        }

        /// <summary>
        /// Starts the server
        /// </summary>
        public void StartServer()
        {
            _server.Start();
        }
        
        /// <summary>
        /// Gets called whenver the server updates a property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ClientsOnline));

            switch (e.PropertyName)
            {
                case "ServerResponse":
                    Application.Current.Dispatcher.Invoke(callback: () =>
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}