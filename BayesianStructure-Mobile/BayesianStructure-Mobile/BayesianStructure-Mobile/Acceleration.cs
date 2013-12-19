using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BayesianStructure
{
    internal class Acceleration : Variable
    {
        private Vector3 _mean;
        public Vector3 Mean { get; internal set; }
        public Vector3 Variance { get; internal set; }

        public Acceleration(Vector3 acceleration) : base()
        {
            _mean = acceleration;
            UpdateVariance();
            UpdateMean();
        }

        protected override void UpdateVariance()
        {
            Variance = new Vector3(Constants.ACCELERATION_VARIANCE, Constants.ACCELERATION_VARIANCE, Constants.ACCELERATION_VARIANCE);
        }

        protected override void UpdateMean()
        {
            Mean = _mean;
        }
    }
}
