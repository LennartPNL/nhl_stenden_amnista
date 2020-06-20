using System.Windows.Input;
using Amnista.Generic;
using Amnista.Generic.client.Server.Commands;
using Amnista.Generic.Server;
using Amnista.Models;

namespace Amnista.View_Models
{
    class CoffeeVoteViewModel: ObservableObject
    {
        

        public ICommand StartVoteCommand => new Commander(StartVote);


       // public void SendCommand(string command, object payload)
      //  {
            //ServerCommand serverCommand = (ServerCommand)payload;
            //serverCommand.Command = command;
            //SendMessage(JsonConvert.SerializeObject(serverCommand));
        //}

        public void StartVote()
        {
            //SendCommand("start_vote", new StartVoteCommand());
        }

    }
}