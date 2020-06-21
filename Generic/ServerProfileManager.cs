using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using Amnista.Annotations;
using Amnista.Models;

namespace Amnista.Generic
{
    /// <summary>
    /// The serverProfileManager keeps track of all the connected sockets
    /// </summary>
    public class ServerProfileManager: INotifyPropertyChanged
    {
        private readonly List<ClientProfile> _clientProfiles = new List<ClientProfile>();

        public List<ClientProfile> Profiles { get => _clientProfiles; }

        /// <summary>
        /// Adds a new client to the connected clients list
        /// </summary>
        /// <param name="clientProfile">Client to add</param>
        public void AddProfile(ClientProfile clientProfile)
        {
            _clientProfiles.Add(clientProfile);
            OnPropertyChanged("Profiles");
        }

        /// <summary>
        /// Removes an client from the connected clients list
        /// </summary>
        /// <param name="clientProfile"></param>
        public void DeleteProfile(ClientProfile clientProfile)
        {
            _clientProfiles.Remove(clientProfile);
            OnPropertyChanged("Profiles");
        }

        /// <summary>
        /// Searches all the connected clients and returns the one that has the same IP as the requested seacrh IP
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
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