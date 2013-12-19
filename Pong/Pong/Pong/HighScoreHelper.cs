using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows;

namespace Pong
{
    public static class HighScoreHelper
    {
        private static string textFile;
        static List<Score> scores;

        public static List<Score> LoadHighScore()
        {
            scores = new List<Score>();
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader Reader = null;
            try
            {
                Reader = new StreamReader(new IsolatedStorageFileStream("HighScore.txt", FileMode.OpenOrCreate, fileStorage));
                textFile = Reader.ReadToEnd();
                Reader.Close();
            }
            catch
            {
                MessageBox.Show("File not created");
            }

            string[] nameAndScoreArray = textFile.Split('#');

            foreach (var item in nameAndScoreArray.Where(x => x.Contains(",")))
            {
                Score score = new Score();

                int commaIndex = item.IndexOf(",");
                string name = item.Substring(commaIndex + 1);
                string points = item.Substring(0, commaIndex);
                score.Name = name;
                score.Points = int.Parse(points);
                scores.Add(score);

            }

            scores = scores.OrderByDescending(x => x.Points).ToList();

            if (scores.Count > 10)
            {
                scores.RemoveRange(10, scores.Count - 10);
            }

            return scores;
        }

        public static void SaveHighScore(string name, double score)
        {
            LoadHighScore();

            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            StreamWriter Writer = new StreamWriter(new IsolatedStorageFileStream("HighScore.txt", FileMode.OpenOrCreate, fileStorage));
            Writer.Write(textFile + score.ToString("n0") + "," + name + "#");
            Writer.Close();
        }
    }
}
