using System.Windows.Input;
using Amnista.Generic;
using Amnista.Models;

namespace Amnista.View_Models
{
    class CoffeeVoteViewModel
    {
        public ClientSocket ClientSocket { get; set; }

        public ICommand StartCoffeeVoteCommand => new Commander(StartCoffeeVote);

        public void StartCoffeeVote()
        {
            ClientSocket.Vote();
        }
    }
}