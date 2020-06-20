using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amnista.Models
{
    public class ClientProfileManager
    {
        public List<ClientProfile> ClientProfiles { get; set; }

        public List<ClientProfile> OwnProfile { get; set; }

        public ClientSocket ClientSocket { get; set; }
    }
}
