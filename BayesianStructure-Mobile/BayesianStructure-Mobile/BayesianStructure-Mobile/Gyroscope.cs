using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BayesianStructure
{
    class Gyroscope : Variable
    {
        private Vector3 _mean;
        public Vector3 Mean { get; internal set; }
        public Vector3 Variance { get; internal set; }

        public Gyroscope(Vector3 gyroScope)
            : base()
        {
            _mean = gyroScope;
            UpdateVariance();
            UpdateMean();
        }

        protected override void UpdateVariance()
        {
            Variance = Vector3.Zero;
        }

        protected override void UpdateMean()
        {
            Mean = _mean;
        }
    }
}
