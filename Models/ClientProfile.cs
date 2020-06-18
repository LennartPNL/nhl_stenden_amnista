using System.Net.Sockets;
using Amnista.Data_Types.Enums;

namespace Amnista.Models
{
    public class ClientProfile
    {
        public string Name { get; set; }
        public Status Status { get; set; }
        public DrinkPreference DrinkPreference { get; set; }
        public int CoffeePoints { get; set; }
        public Socket Socket { get; set; }
    }
}