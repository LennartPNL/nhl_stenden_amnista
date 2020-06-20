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
            this.MainFrame.Navigate(new HomeView(this), _clientSocket);
            _clientSocket.VoteStarted += ClientSocketOnVoteStarted;
            _clientSocket.VoteReceived += ClientSocketOnVoteReceived;
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
    }
}