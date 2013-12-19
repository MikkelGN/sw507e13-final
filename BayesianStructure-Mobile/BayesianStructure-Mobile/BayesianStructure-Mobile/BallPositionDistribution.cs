using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BayesianStructure
{
    internal class BallPositionDistribution : Variable
    {
        private float _mean;
        public BallPositionDistribution(float mean) : base()
        {
            _mean = mean;

            UpdateVariance();
            UpdateMean(); 
        }

        protected override void UpdateVariance()
        {
            Variance = Constants.BALL_VARIANCE_GAIN;
        }

        protected override void UpdateMean()
        {
            Mean = _mean;
        }
    }
}
