﻿using System;
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

        }
    }
}
