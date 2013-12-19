using BayesianStructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ConsoleApplication1
{
    public class Program
    {
        public static float yPosition = 1.5f;
        public static float xPosition = 0.9f;
        public static float yballSpeed = 2;
        public static float xballSpeed = 2;

        static void Main(string[] args)
        {
            StreamReader sourceReader = new StreamReader("Mikkel50.txt");
            string line = sourceReader.ReadToEnd();
            string[] lines = line.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            sourceReader.Close();
            sourceReader.Dispose();

            StreamWriter positionWriter = new StreamWriter("Position.txt");
            StreamWriter movingAverageWriter = new StreamWriter("MovingAverage.txt");
            StreamWriter velocityWriter = new StreamWriter("Velocity.txt");
            StreamWriter sensorFusionWriter = new StreamWriter("sensorFusion.txt");
            StreamWriter rollWriter = new StreamWriter("Roll.txt");
            StreamWriter ballWriter = new StreamWriter("Ball.txt");
            StreamWriter stepDirectionWriter = new StreamWriter("StepDirection.txt");
            StreamWriter gyroScopeWriter = new StreamWriter("Gyroscope.txt");

            Network theNetwork = new Network();
            foreach (string s in lines)
            {
                float time = (float)Convert.ToDouble(s.Split('\t')[0]);
                float accelerationX = (float)Convert.ToDouble(s.Split('\t')[1]);
                float accelerationY = (float)Convert.ToDouble(s.Split('\t')[2]);
                float accelerationZ = (float)Convert.ToDouble(s.Split('\t')[3]);
                float gyroscopeX = (float)Convert.ToDouble(s.Split('\t')[4]);
                float gyroscopeY = (float)Convert.ToDouble(s.Split('\t')[5]);
                float gyroscopeZ = (float)Convert.ToDouble(s.Split('\t')[6]);
                Vector3 acceleration = new Vector3(accelerationX, accelerationY, accelerationZ);
                Vector3 gyroscope = new Vector3(gyroscopeX, gyroscopeY, gyroscopeZ);
                

                CalculateBallPosition();
                theNetwork.UpdateNetwork(acceleration, yPosition, gyroscope);

                positionWriter.WriteLine(time + "\t" + theNetwork.Position.Mean.ToString().Replace(',', '.') + "\t" + theNetwork.Position.Variance);
                //positionWriter.WriteLine(theNetwork.Position.Mean.ToString().Replace(',', '.') + ",");
                movingAverageWriter.WriteLine(time + "\t" + theNetwork.CorrectedMovingAverage.Mean.ToString().Replace(',', '.') + "\t" + theNetwork.MovingAverage.Variance);
                velocityWriter.WriteLine(time + "\t" + theNetwork.Velocity.Mean.ToString().Replace(',', '.') + "\t" + theNetwork.Velocity.Variance);
                sensorFusionWriter.WriteLine(time + "\t" + theNetwork.SensorFusion.Mean.ToString().Replace(',', '.') + "\t" + theNetwork.SensorFusion.Variance);
                //rollWriter.WriteLine(time + "\t" + Convert.ToDouble(s.Split('\t')[2]));
                ballWriter.WriteLine(time + "\t" + theNetwork.Ball.Mean.ToString().Replace(',', '.') + "\t" + theNetwork.Ball.Variance);
                stepDirectionWriter.WriteLine(time + "\t" + GetDirection(theNetwork));
            }


            movingAverageWriter.Close();
            movingAverageWriter.Dispose();
            velocityWriter.Close();
            velocityWriter.Dispose();
            positionWriter.Close();
            positionWriter.Dispose();
            sensorFusionWriter.Close();
            sensorFusionWriter.Dispose();
            rollWriter.Close();
            rollWriter.Dispose();
            ballWriter.Close();
            ballWriter.Dispose();
            stepDirectionWriter.Close();
            stepDirectionWriter.Dispose();
            gyroScopeWriter.Close();
            gyroScopeWriter.Dispose();

            Console.WriteLine("Please input your World of Warcraft username and password to exit successfully.");
            Console.Read();
        }
        public static string GetDirection(Network theNetwork)
        {
            return theNetwork.Acceleration.Mean < Constants.JITTER_MIN ? "-1" : theNetwork.Acceleration.Mean > Constants.JITTER_MAX ? "1" : "0";
        }
        public static void CalculateBallPosition()
        {

            yPosition += yballSpeed * 0.02f;
            xPosition += xballSpeed * 0.02f;

            float maxY = 3;
            float minY = 0;
            float maxX = 1.8f;
            float minX = 0;

            if (yPosition > maxY)
            {
                yballSpeed *= -1f;
                yPosition = maxY;
            }
            else if (yPosition < minY)
            {
                yballSpeed *= -1f;
                yPosition = minY;
            }
            if (xPosition > maxX)
            {
                xballSpeed *= -1f;
                xPosition = maxX;
            }
            else if (xPosition < minX)
            {
                xballSpeed *= -1f;
                xPosition = minX;
            }
        }
    }
}
