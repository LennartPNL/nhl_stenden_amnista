using Amnista.Views;
using Amnista.Views.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Amnista
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _disableSidebarNavigation;

        public MainWindow()
        {
            InitializeComponent();
            this.MainFrame.Navigate(new HomeView(this));
            
        }


        private void Btn_profile_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Ik ben er");
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
                MainFrame?.Navigate(type);
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