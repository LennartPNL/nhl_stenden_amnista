using System.Collections.Generic;
using System.Windows.Documents;

namespace Amnista.Models
{
    class WheelOfFortune
    {
        // TODO: add connected clients from clientProfileManager
        public List<ClientProfile> ClientProfiles { get; set; }

        public ClientProfile Winner { get; set; }
    }
}