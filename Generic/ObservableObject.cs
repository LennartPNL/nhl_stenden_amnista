using System.ComponentModel;
using System.Runtime.CompilerServices;
using Amnista.Annotations;

namespace Amnista.Generic
{
    /// <summary>
    /// This class is used to notify the view that a property has changed.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes a change event from which the view can update the value.
        /// </summary>
        /// <param name="propertyName">Name of the changed property</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}