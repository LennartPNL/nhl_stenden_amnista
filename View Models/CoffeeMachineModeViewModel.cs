using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Amnista.Generic;
using Amnista.Models;

namespace Amnista.View_Models
{
    class CoffeeMachineModeViewModel : ObservableObject
    {
        private CoffeeMachine _coffeeMachine;
        public int ClientsOnline => _coffeeMachine.ClientsOnline;
        public string ServerResponse => _coffeeMachine.ServerResponse;
        public List<ClientProfile> ClientProfiles => _coffeeMachine.ClientProfiles;
        public string ServerIP => _coffeeMachine.ServerIP;
        public int ClientProfilesDidVote => _coffeeMachine.ClientProfilesDidVote;
        public List<ClientProfile> VotedClients => _coffeeMachine.VotedClients;
        public bool ServerEnabled => _coffeeMachine.ServerEnabled;
        public ICommand StartServerCommand => new Commander(StartServer);

        /// <summary>
        /// Constructor
        /// </summary>
        public CoffeeMachineModeViewModel()
        {
            _coffeeMachine = new CoffeeMachine();
            _coffeeMachine.PropertyChanged += CoffeeMachineOnPropertyChanged;
        }

        /// <summary>
        /// Starts the server
        /// </summary>
        public void StartServer()
        {
            _coffeeMachine.StartServer();
        }

        /// <summary>
        /// Gets called whenever a property in the coffeemachine mode gets changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CoffeeMachineOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ClientsOnline));

            switch (e.PropertyName)
            {
                case "ServerResponse":
                    OnPropertyChanged(nameof(ServerResponse));
                    break;
                case "Profiles":
                    OnPropertyChanged(nameof(ClientProfiles));
                    break;
                case nameof(ServerIP):
                    OnPropertyChanged(nameof(ServerIP));
                    break;
                case nameof(ClientProfilesDidVote):
                    OnPropertyChanged(nameof(ClientProfilesDidVote));
                    break;
                case nameof(VotedClients):
                    OnPropertyChanged(nameof(VotedClients));
                    break;
                case nameof(ClientsOnline):
                    OnPropertyChanged(nameof(ClientsOnline));
                    break;
                case nameof(ServerEnabled):
                    OnPropertyChanged(nameof(ServerEnabled));
                    break;
            }
        }
    }
}