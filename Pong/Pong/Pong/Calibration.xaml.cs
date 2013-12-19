using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Devices.Sensors;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Threading;

namespace Pong
{
    public partial class Calibration : PhoneApplicationPage
    {
        private DateTime _timeStart;
        Compass compass = new Compass();
        private List<float> _accelerations;
        private double calibrationTime = 3.0;

        public Calibration()
        {
            InitializeComponent();
            compass.Start();
            compass.CurrentValueChanged += compass_CurrentValueChanged;
        }

        void compass_CurrentValueChanged(object sender, SensorReadingEventArgs<CompassReading> e)
        {
                BayesianStructure.Constants.BASE_DIRECTION = (float)e.SensorReading.TrueHeading;
        }


        private void btnStartCalibration_Click(object sender, RoutedEventArgs e)
        {
            btnStartCalibration.IsEnabled = false;

            _timeStart = DateTime.Now;
            txtStatus.Text = "Calibrated";



            compass.Stop();
        }

        public void UpdateUI(Action displayCall)
        {
            if (Dispatcher.CheckAccess() == false)
            {
                Dispatcher.BeginInvoke(() =>
                     displayCall()
                );
            }
            else
            {
                displayCall();
            }
        }

    }
}