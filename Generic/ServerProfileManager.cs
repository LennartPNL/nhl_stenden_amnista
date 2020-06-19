using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using Amnista.Annotations;
using Amnista.Models;

namespace Amnista.Generic
{
    public class ServerProfileManager: INotifyPropertyChanged
    {
        private readonly List<ClientProfile> _clientProfiles = new List<ClientProfile>();

        public List<ClientProfile> Profiles { get => _clientProfiles; }



        public void AddProfile(ClientProfile clientProfile)
        {
            _clientProfiles.Add(clientProfile);
            OnPropertyChanged("Profiles");
        }

        public void DeleteProfile(ClientProfile clientProfile)
        {
            _clientProfiles.Remove(clientProfile);
            OnPropertyChanged("Profiles");
        }

        public ClientProfile FindClientProfileByIP(EndPoint endPoint)
        {
            return _clientProfiles.FirstOrDefault(client => client.Socket.RemoteEndPoint.Equals(endPoint));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}