using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Amnista.Annotations;
using Amnista.Events;
using Amnista.Generic;

namespace Amnista.Models
{
    public class Server : INotifyPropertyChanged
    {
        private readonly SocketService _socketService;
        private readonly VoteManager _voteManager = new VoteManager();
        private readonly ServerProfileManager _serverProfileManager;
        private bool _started = false;

        public bool Started
        {
            get => _started;
            set
            {
                _started = value;
                OnPropertyChanged(nameof(Started));
            }
        }

        public int ClientProfilesDidVote => _voteManager.ClientProfilesDidVote;


        private string _serverResponse = "";

        public string ServerResponse
        {
            get => _serverResponse;
            set
            {
                _serverResponse = value;
                OnPropertyChanged(nameof(ServerResponse));
            }
        }

        public ServerProfileManager ServerProfileManager
        {
            get => _serverProfileManager;
        }

        public Server()
        {
            _serverProfileManager = new ServerProfileManager();
            _socketService = new SocketService();
            _socketService.ClientConnected += SocketServiceOnClientConnected;
            _socketService.MessageReceived += SocketServiceOnMessageReceived;
            _socketService.UpdateReceived += SocketServiceOnUpdateReceived;
            _socketService.ClientDisconnected += SocketServiceOnClientDisconnected;
            _socketService.StartVoteReceived += SocketServiceOnStartVoteReceived;
            _serverProfileManager.PropertyChanged += ServerProfileManagerOnPropertyChanged;
        }

        private void SocketServiceOnStartVoteReceived(object sender, StartVoteReceivedEventArgs e)
        {
            if (!_voteManager.VoteHasStarted)
            {
                _serverProfileManager.Profiles.ForEach(client =>
                {
                    client.Socket.Send(
                        // e.Client.RemoteEndPoint) + started a vote
                        Encoding.ASCII.GetBytes(
                            _serverProfileManager.FindClientProfileByIP(e.Client.RemoteEndPoint).Name +
                            " started a vote!"));
                });
            }

            _voteManager.Vote(_serverProfileManager.FindClientProfileByIP(e.Client.RemoteEndPoint));
            ServerResponse = _serverProfileManager.FindClientProfileByIP(e.Client.RemoteEndPoint).Socket.RemoteEndPoint
                .ToString();
            OnPropertyChanged(nameof(ServerResponse));
            OnPropertyChanged(nameof(ClientProfilesDidVote));
        }

        private void ServerProfileManagerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Profiles")
            {
                OnPropertyChanged("Profiles");
            }
        }


        public void Start()
        {
            Started = true;
            _socketService.Start();
        }

        public void Stop()
        {
            Started = false;
            _socketService.Stop();
        }

        private void SocketServiceOnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            ClientProfile clientProfile = _serverProfileManager.FindClientProfileByIP(e.Client.RemoteEndPoint);
            _serverProfileManager.DeleteProfile(clientProfile);
            ServerResponse = e.Client.RemoteEndPoint + " disconnected \n conns: " +
                             _serverProfileManager.Profiles.Count;
        }

        private void SocketServiceOnMessageReceived(object sender, ClientMessageReceivedEventArgs e)
        {
            ServerResponse = e.Message;
        }

        private void SocketServiceOnUpdateReceived(object sender, ClientUpdateReceivedEventArgs e)
        {
            ClientProfile client = _serverProfileManager.FindClientProfileByIP(e.Client.RemoteEndPoint);
            client.Name = e.ClientProfile.Name;
            client.CoffeePoints = e.ClientProfile.CoffeePoints;
            client.DrinkPreference = e.ClientProfile.DrinkPreference;
            client.Status = e.ClientProfile.Status;
            ServerResponse = client.Name;
        }


        private void SocketServiceOnProfileUpdateReceived(object sender, ClientMessageReceivedEventArgs e)
        {
            ServerResponse = e.Message;
        }


        private void SocketServiceOnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            ClientProfile clientProfile = new ClientProfile();
            clientProfile.Socket = e.Client;
            _serverProfileManager.AddProfile(clientProfile);
            OnPropertyChanged("ClientsOnline");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}