namespace Amnista.Models
{
    class CoffeeVote
    {
        // TODO: add connected clients from clientProfileManager

        public bool PassedThreshold { get; set; }
        public int Threshold { get; set; }

        private void Vote()
        {
        }

        private void StartVote()
        {
        }

        private float CalculateThreshold()
        {
            return Threshold;
        }
    }
}