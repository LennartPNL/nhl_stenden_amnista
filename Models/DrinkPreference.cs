using Amnista.Data_Types.Enums;

namespace Amnista.Models
{
    /// <summary>
    /// Used to hold the drink preferences for a client
    /// </summary>
    public class DrinkPreference
    {
        public DrinkType DrinkType { get; set; }
        public bool WithMilk { get; set; }
        public bool WithSugar { get; set; }
    }
}