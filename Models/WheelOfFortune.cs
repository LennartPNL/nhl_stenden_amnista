namespace Amnista.Models
{
    class WheelOfFortune
    {
        // TODO: add connected clients from clientProfileManager

        public ClientProfileManager ClientProfileManager { get; set; }

        public ClientProfile Winner { get; set; }
    }
}