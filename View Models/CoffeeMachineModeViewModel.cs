using System.Windows;
using System.Windows.Input;
using Amnista.Generic;
using Amnista.Models;

namespace Amnista.View_Models
{
    class CoffeeMachineModeViewModel : ObservableObject
    {
        private readonly Server _server;

        public CoffeeMachineModeViewModel()
        {
            _server = new Server();
        }

        public ICommand StartServerCommand => new Commander(StartServer);

        public void StartServer()
        {
            
        }
    }
}