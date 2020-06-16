using Amnista.Data_Types.Enums;

namespace Amnista.Models
{
    class DrinkPreference
    {
        public DrinkType DrinkType { get; set; }
        public bool WithMilk { get; set; }
        public bool WithSugar { get; set; }
    }
}