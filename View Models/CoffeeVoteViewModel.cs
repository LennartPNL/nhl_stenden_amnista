using System.Windows.Input;
using Amnista.Generic;
using Amnista.Models;

namespace Amnista.View_Models
{
    class CoffeeVoteViewModel: ObservableObject
    {
        

        public ICommand StartVoteCommand => new Commander(StartVote);

        public void StartVote()
        {

        }
    }
}