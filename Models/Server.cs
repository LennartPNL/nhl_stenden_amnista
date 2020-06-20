using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using Amnista.Annotations;
using Amnista.Events;
using Amnista.Generic;
using Amnista.Generic.Server.Commands;
using Newtonsoft.Json;

namespace Amnista.Models
{
    public class Server : INotifyPropertyChanged
    {
        private readonly SocketService _socketService;
        private readonly VoteManager _voteManager = new VoteManager();
        private readonly ServerProfileManager _serverProfileManager;
        private bool _started = false;

        public string ServerIP => _socketService.ServerIP;

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

        private void SocketServiceOnStartVoteReceived(object sender, VoteReceivedEventArgs e)
        {
            
            ClientProfile votedClient =
                _serverProfileManager.FindClientProfileByIP(e.Client.RemoteEndPoint);
            ServerResponse = votedClient.Name;

            if (!_voteManager.VoteHasStarted)
            {
                _serverProfileManager.Profiles.ForEach(client =>
                {
                    ClientVotedCommand clientVotedCommand = new ClientVotedCommand()
                        {Client = votedClient, Command = "start_vote"};
                    string clientVotedCommandSerialized = JsonConvert.SerializeObject(clientVotedCommand);
                    client.Socket.Send(Encoding.ASCII.GetBytes(clientVotedCommandSerialized));
                    client.Socket.Send(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new ClientVotedCommand()
                    {
                        Command = "client_voted",
                        Client = votedClient
                    })));
                });
            }

                _voteManager.Vote(_serverProfileManager.FindClientProfileByIP(e.Client.RemoteEndPoint));
            
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
            OnPropertyChanged(nameof(ServerIP));
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