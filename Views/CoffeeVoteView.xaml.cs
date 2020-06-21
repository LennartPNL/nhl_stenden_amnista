using System.Windows.Controls;
using Amnista.View_Models;

namespace Amnista.Views
{
    /// <summary>
    /// Interaction logic for CoffeeVoteView.xaml
    /// </summary>
    public partial class CoffeeVoteView : Page
    {
        public CoffeeVoteView()
        {
            InitializeComponent();
            ((CoffeeVoteViewModel) DataContext).ClientSocket = MainWindow.ClientSocket;
        }
    }
}