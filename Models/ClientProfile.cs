using Amnista.Data_Types.Enums;

namespace Amnista.Models
{
    public class ClientProfile
    {
        public string Name { get; set; }
        public Status status { get; set; }
        public DrinkPreference DrinkPreference { get; set; }
        public int CoffeePoints { get; set; }
    }
}