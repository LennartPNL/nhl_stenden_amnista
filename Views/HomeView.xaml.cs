using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Amnista.Models;

namespace Amnista.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : Page
    {
        //For test purposes, remove when this page has an implementation
        private MainWindow parent;

        public HomeView(MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void Btn_coffee_vote_Click(object sender, RoutedEventArgs e)
        {
            ClientProfileManager clientProfileManager = new ClientProfileManager();

            List<ClientProfile> clientProfiles = new List<ClientProfile>();
            clientProfiles.Add(new ClientProfile
            {
                Name = "Henk"
            });
            clientProfiles.Add(new ClientProfile
            {
                Name = "Jaapie"
            });
            clientProfiles.Add(new ClientProfile
            {
                Name = "Jan"
            });
            clientProfiles.Add(new ClientProfile
            {
                Name = "Klaas"
            });
            clientProfiles.Add(new ClientProfile
            {
                Name = "Robbert"
            });
            clientProfiles.Add(new ClientProfile
            {
                Name = "Ellen"
            });

            clientProfileManager.ClientProfiles = clientProfiles;
            parent.MainFrame.Navigate(new WheelOfFortuneView(clientProfileManager));
        }

        private void test()
        {
            
        }
    }
}
