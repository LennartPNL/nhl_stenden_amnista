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
            _socketService = new SocketService();
            _socketService.ClientConnected += SocketServiceOnClientConnected;
            _socketService.MessageReceived += SocketServiceOnMessageReceived;
            _socketService.ClientDisconnected += SocketServiceOnClientDisconnected;
        }


        public void Start()
        {
            _socketService.Start();
        }

        private void SocketServiceOnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            ServerResponse = e.Client.RemoteEndPoint + " disconnected";
        }

        private void SocketServiceOnMessageReceived(object sender, ClientMessageReceivedEventArgs e)
        {
            ServerResponse = e.Message;
        }
        private void SocketServiceOnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            ServerResponse = e.Client.RemoteEndPoint + " \n stay Connected";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}