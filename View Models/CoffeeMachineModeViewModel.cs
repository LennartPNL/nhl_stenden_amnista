using System.ComponentModel;
using System.Windows.Input;
using Amnista.Generic;
using Amnista.Models;

namespace Amnista.View_Models
{
    class CoffeeMachineModeViewModel : ObservableObject
    {
        private readonly Server _server;
        private string _serverResponse;

        public string ServerResponse
        {
            get => _serverResponse;
            set
            {
                _serverResponse = value;
                OnPropertyChanged();
            }
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
            if (e.PropertyName == "ServerResponse")
            {
                ServerResponse = _server.ServerResponse;
            }
        }
    }
}