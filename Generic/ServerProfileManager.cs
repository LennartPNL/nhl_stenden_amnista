using System.Collections.Generic;
using System.Linq;
using System.Net;
using Amnista.Models;

namespace Amnista.Generic
{
    public class ServerProfileManager
    {
        private List<ClientProfile> _clientProfiles = new List<ClientProfile>();

        public List<ClientProfile> Profiles { get => _clientProfiles; }



        public void AddProfile(ClientProfile clientProfile)
        {
            _clientProfiles.Add(clientProfile);
        }

        public void DeleteProfile(ClientProfile clientProfile)
        {
            _clientProfiles.Remove(clientProfile);
        }

        public ClientProfile FindClientProfileByIP(EndPoint endPoint)
        {
            return _clientProfiles.FirstOrDefault(client => client.Socket.RemoteEndPoint.Equals(endPoint));
        }
    }
}