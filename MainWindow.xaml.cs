using Amnista.Views;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Amnista.Events;
using Amnista.Events.client;
using Amnista.Generic.client.Server.Commands;
using Amnista.Models;

namespace Amnista
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _disableSidebarNavigation;
        private ClientSocket _clientSocket;

        public MainWindow()
        {
            InitializeComponent();
            _clientSocket = new ClientSocket();
            new Thread(()=> _clientSocket.StartClient()).Start();
            this.MainFrame.Navigate(new HomeView(), _clientSocket);
            _clientSocket.VoteStarted += ClientSocketOnVoteStarted;
            _clientSocket.VoteReceived += ClientSocketOnVoteReceived;
            _clientSocket.VoteEnded += ClientSocketOnVoteEnded;
        }

        private void ClientSocketOnVoteEnded(object sender, VoteEndedEventArgs e)
        {
            MessageBox.Show(e.ClientProfile.Name + " needs to get the coffee");
        }

        private void ClientSocketOnVoteStarted(object sender, VoteStartedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (MessageBox.Show("Coffee time!", e.ClientProfile.Name + " started a coffee vote",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    _clientSocket.SendCommand("start_vote", new StartVoteCommand());
                    // this.MainFrame.Navigate(new WheelOfFortuneView());
                }

            });

        }

        private void ClientSocketOnVoteReceived(object sender, ClientVoteReceivedEventArgs e)
        {
            Debug.WriteLine("Client " + e.Client.Name + " voted");
        }


        private void Btn_profile_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(new ClientProfileView());
        }

        private void Btn_mode_switch_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(new CoffeeMachineModeView());
        }

        private void Btn_toggle_availability_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Btn_settings_Click(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(new SettingsView());
        }


        private void PagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_disableSidebarNavigation)
            {
                NavigateToSelectedPage();
            }
        }

        private void NavigateToSelectedPage()
        {
            if (PagesList.SelectedValue is Object type)
            {
                MainFrame?.Navigate(type, _clientSocket);
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            _disableSidebarNavigation = true;
            PagesList.SelectedValue = this.MainFrame.Content;
            _disableSidebarNavigation = false;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _clientSocket.Vote();
        }
    }
}