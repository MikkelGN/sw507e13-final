using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.IO;

namespace Pong
{
    public partial class GameOver : PhoneApplicationPage
    {
        public GameOver()
        {
            InitializeComponent();
            txtScore.Text = GlobalVariables.Score.ToString("n0");
        }

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void btnReplay_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            while (NavigationService.RemoveBackEntry() != null)
            {
                NavigationService.RemoveBackEntry();
            }
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void txtScoreName_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter) {

                HighScoreHelper.SaveHighScore(txtScoreName.Text, GlobalVariables.Score);

                NavigationService.Navigate(new Uri("/HighScore.xaml", UriKind.Relative));
            }
        }

    
    }
}