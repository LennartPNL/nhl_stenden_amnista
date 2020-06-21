using System.Net.Sockets;
using Amnista.Data_Types.Enums;
using Amnista.Generic.Server;
using Newtonsoft.Json;

namespace Amnista.Models
{
    /// <summary>
    /// This class holds all the information for an client. And is also used as communication model between
    /// the server and the client
    /// </summary>
    public class ClientProfile : ServerCommand
    {
        public string Name { get; set; }
        public Status Status { get; set; }
        public DrinkPreference DrinkPreference { get; set; }
        public int CoffeePoints { get; set; }
        [JsonIgnore]
        public Socket Socket { get; set; }
    }
}