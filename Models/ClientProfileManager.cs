using System.Collections.Generic;

namespace Amnista.Models
{
    public class ClientProfileManager
    {
        public List<ClientProfile> ClientProfiles { get; set; }

        public List<ClientProfile> OwnProfile { get; set; }

        public ClientSocket ClientSocket { get; set; }
    }
}
