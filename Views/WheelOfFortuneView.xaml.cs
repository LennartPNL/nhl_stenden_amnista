using Amnista.Models;
using Amnista.View_Models;
using System.Windows.Controls;

namespace Amnista.Views
{
    /// <summary>
    /// Interaction logic for WheelOfFortuneView.xaml
    /// </summary>
    public partial class WheelOfFortuneView : Page
    {
        public WheelOfFortuneView(ClientSocket clientSocket)
        {
            InitializeComponent();
            ((WheelOfFortuneViewModel)DataContext).ClientSocket = clientSocket;
        }
    }
}
