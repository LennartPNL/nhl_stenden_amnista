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
        public static ClientSocket ClientSocket { get; set; }

        public MainWindow()
        {
            ClientSocket = new ClientSocket();

            InitializeComponent();
            new Thread(() => ClientSocket.StartClient()).Start();
            MainFrame.Navigate(new CoffeeVoteView(), ClientSocket);
            ClientSocket.VoteStarted += ClientSocketOnVoteStarted;
            ClientSocket.VoteReceived += ClientSocketOnVoteReceived;
        }


        private void ClientSocketOnVoteStarted(object sender, VoteStartedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (MessageBox.Show("Coffee time!", e.ClientProfile.Name + " started a coffee vote",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ClientSocket.SendCommand("start_vote", new StartVoteCommand());
                    this.MainFrame.Navigate(new WheelOfFortuneView(ClientSocket));
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
                MainFrame?.NavigationService.Navigate(type, ClientSocket);
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