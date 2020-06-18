using System.ComponentModel;
using System.Runtime.CompilerServices;
using Amnista.Annotations;
using Amnista.Events;
using Amnista.Generic;

namespace Amnista.Models
{
    public class Server : INotifyPropertyChanged
    {
        private readonly SocketService _socketService;
        private readonly ServerProfileManager _serverProfileManager;

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

        public Server()
        {
            _serverProfileManager = new ServerProfileManager();
            _socketService = new SocketService();
            _socketService.ClientConnected += SocketServiceOnClientConnected;
            _socketService.MessageReceived += SocketServiceOnMessageReceived;
            _socketService.UpdateReceived += SocketServiceOnUpdateReceived;
            _socketService.ClientDisconnected += SocketServiceOnClientDisconnected;
        }

  

        public void Start()
        {
            _socketService.Start();
        }

        private void SocketServiceOnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            ClientProfile clientProfile = _serverProfileManager.FindClientProfileByIP(e.Client.RemoteEndPoint);
            _serverProfileManager.DeleteProfile(clientProfile);
            ServerResponse = e.Client.RemoteEndPoint + " disconnected \n conns: " + _serverProfileManager.Profiles.Count;
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
            ServerResponse =_serverProfileManager.Profiles.Count + " \n stay Connected";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}