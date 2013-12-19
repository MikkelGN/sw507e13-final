using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DataLogger.Resources;
using Microsoft.Phone.Tasks;
using Microsoft.Devices.Sensors;
using System.IO;
using Microsoft.Xna.Framework;

namespace DataLogger
{

    public class Data 
    {
        
        public Vector3 acceleration;
        public Vector3 gyroscope;
        public float accelerationBoxing;
        public float accelerationWheel;
        public double angle;
        public double time;
        public float ANTIgyroscope;
         
        public Data(Vector3 _acceleration, double _time){
            acceleration = _acceleration;
            time = _time;
        }
    }

    public partial class MainPage : PhoneApplicationPage
    {
        public double oneDirection;
        bool oneDirectionSet = false;
        
        string session = "";
        Random ran = new Random();
        List<Data> DataList = new List<Data>();
        DateTime starttime;
        Accelerometer accelerometer = new Accelerometer();
        Windows.Devices.Sensors.Compass compass = Windows.Devices.Sensors.Compass.GetDefault();
        Compass compass2 = new Compass();
        Gyroscope gyroscope = new Gyroscope();
        Vector3 gyroscopeAccumulated = Vector3.Zero;
      
        public MainPage()
        {
            InitializeComponent();
            accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
            accelerometer.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(AccelerometerReadingChanged);
        }




        private void AccelerometerReadingChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            var acceleration = e.SensorReading.Acceleration;       
            var time = (DateTime.Now - starttime).TotalMilliseconds;
            //                          acc,          gyroscope,                pitch,               yaw,   roll
            var newDataInput = new Data(acceleration, time);
            newDataInput.gyroscope = gyroscope.CurrentValue.RotationRate;
            DataList.Add(newDataInput);
        }
       
        
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            RandomizeSession();
            DataList.Clear();
            txtboxCount.Text = "0";
            starttime = new DateTime();
            starttime = DateTime.Now;
            btnStart.Content = "Running";
            btnDatabase.IsEnabled = false;
            
            compass2.Start();
            gyroscope.Start();
            accelerometer.Start();

        }


        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            accelerometer.Stop();
            compass2.Stop();
            gyroscope.Stop();
            btnStart.Content = "Start";
            btnDatabase.IsEnabled = true;
            gyroscopeAccumulated = Vector3.Zero;
            oneDirectionSet = false;
        }


        private void RandomizeSession()
        {
            session = ran.Next(int.MinValue, int.MaxValue).ToString();
            textBoxSession.Text = session;

        }

        private void btnDatabase_Click(object sender, RoutedEventArgs e)
        {
            string accelerationXResult = "", accelerationYResult = "", accelerationZResult = "", gyroscopeXResult = "", gyroscopeYResult = "", gyroscopeZResult = "", pitchResult = "", yawResult = "", rollResult = "", timeResult = ""; 

            int counter = 0;
            foreach (Data x in DataList)
            {
                if (counter > 0)
                {
                    accelerationXResult += "#";
                    accelerationYResult += "#";
                    accelerationZResult += "#";

                    gyroscopeXResult += "#";
                    gyroscopeYResult += "#";
                    gyroscopeZResult += "#";

                    pitchResult += "#";
                    yawResult += "#";
                    rollResult += "#";

                    timeResult += "#";
                }

                accelerationXResult += x.acceleration.X.ToString("n4").Replace(',','.');
                accelerationYResult += x.acceleration.Y.ToString("n4").Replace(',', '.');
                accelerationZResult += x.acceleration.Z.ToString("n4").Replace(',', '.');

                gyroscopeXResult +=  x.gyroscope.X.ToString("n4").Replace(',', '.');
                gyroscopeYResult += x.gyroscope.Y.ToString("n4").Replace(',', '.');
                gyroscopeZResult += x.gyroscope.Z.ToString("n4").Replace(',', '.');

                yawResult += "0";// x.ANTIgyroscope.ToString("n4").Replace(',', '.');
                pitchResult += "0"; // x.accelerationWheel.ToString("n4").Replace(',', '.');
                rollResult += "0";// x.accelerationBoxing.ToString("n4").Replace(',', '.');

                timeResult += x.time.ToString("0");

                counter++;
            }

            var uri = new Uri("http://37.75.184.179:15000/WebService1.asmx/SaveMobileOutput");
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            wc.UploadStringAsync(uri, "POST", "accelerationX=" + accelerationXResult + "&accelerationY=" + accelerationYResult + "&accelerationZ=" + accelerationZResult
                                + "&gyroscopeX=" + gyroscopeXResult + "&gyroscopeY=" + gyroscopeYResult + "&gyroscopeZ=" + gyroscopeZResult
                                + "&pitch=" + pitchResult + "&yaw=" + yawResult + "&roll=" + rollResult
                                + "&time=" + timeResult + "&session=" + session);
            txtboxCount.Text = DataList.Count.ToString();
            btnDatabase.IsEnabled = false;
        }
    }
}