using Amnista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amnista.Events.client
{
    public class WheelOfFortuneUpdatedEventArgs
    {
        public ClientProfile ClientProfile { get; set; }
        public WheelOfFortuneUpdatedEventArgs(ClientProfile clientProfile)
        {
            ClientProfile = clientProfile;
        }
    }
}
