﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Pong
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
        }

        // Simple button Click event handler to take us to the second page
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void btnHighScore_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/HighScore.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            while (NavigationService.RemoveBackEntry() != null)
            {
                NavigationService.RemoveBackEntry();
            }
            
            base.OnNavigatedTo(e);
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void btnCalibration_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Calibration.xaml", UriKind.Relative));
        }
    }
}