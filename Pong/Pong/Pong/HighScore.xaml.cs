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
    public partial class HighScore : PhoneApplicationPage
    {
        IList<Score> scores;
        string textFile;

        public HighScore()
        {
            InitializeComponent();

            scores = HighScoreHelper.LoadHighScore();

            PrintScores();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            while (NavigationService.RemoveBackEntry() != null)
            {
                NavigationService.RemoveBackEntry();
            }
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        

        private void PrintScores()
        {
            int counter = 1;
            foreach (var score in scores)
            {
                string txt = "" + score.Points.ToString("D4") + " " + score.Name + "\n";
                if (counter <= scores.Count / 2)
                {
                    HighScores.Text += txt;
                }
                else
                {
                    HighScores2.Text += txt;
                }
                counter++;
            }
        }
    }
}