using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BayesianStructure
{
    public class SensorFusion : Variable
    {
        private float _mean;
        //ComplementaryFilter is previous, Acceleraiton is parent
        public SensorFusion(Variable complementaryFilter, Variable acceleration) : base(complementaryFilter, acceleration)
        {

        }

        public SensorFusion() : base() { }

        protected override void UpdateVariance()
        {
            Variance = ((ComplementaryFilter)Previous).Variance.X +
                       ((ComplementaryFilter)Previous).Variance.Y +
                       ((ComplementaryFilter)Previous).Variance.Z +
                       ((Acceleration)Parent).Variance.X +
                       ((Acceleration)Parent).Variance.Y +
                       ((Acceleration)Parent).Variance.Z;
        }

        protected override void UpdateMean()
        {
            float tempAcceleration = GetAccelerationAdjustYaw(((Acceleration)Parent).Mean.Y, -1, ((ComplementaryFilter)Previous).Mean.Z);
            Mean = GetAccelerationAdjustPitch(tempAcceleration, ((ComplementaryFilter)Previous).Mean.X);
            _mean = Mean;

            if (Mean <= Constants.ACCELERATION_MAX && Mean >= Constants.ACCELERATION_MIN)
            {
                Mean *= Constants.GRAVITATIONAL_PULL;
                Mean += Constants.ACCELERATION_MEAN_GAIN;

                if (_mean <= Constants.JITTER_MAX && _mean >= Constants.JITTER_MIN)
                {
                    Mean = 0.0f;
                    Variance = 0.0f;
                }
            }
            else
            {
                if (Constants.SHOW_ERROR_MESSAGES)
                {
                    Console.WriteLine("Encountered constraint: mean sensorfusion: " + _mean);
                }

                Mean = _mean < Constants.ACCELERATION_MIN ? -Constants.GRAVITATIONAL_PULL : Constants.GRAVITATIONAL_PULL;
                Variance = 0f;
            }

        }

        private float GetAccelerationAdjustYaw(float accelerationY, float accelerationX, float gyroscopeAngle)
        {
            return (float)((accelerationY - (-accelerationX * Math.Sin(gyroscopeAngle))) / (Math.Cos(gyroscopeAngle)));
            
        }

        private float GetAccelerationAdjustPitch(float acceleration, float angle)
        {
            return acceleration / (float)Math.Cos(angle);

            //float overfraq = (float)Math.Sqrt(Math.Pow(acceleration.Y, 2) + Math.Pow(acceleration.Z, 2));
            //float underfraq = (float)Math.Cos(Math.Abs(angle - Math.Atan(Math.Abs(acceleration.Y / acceleration.Z))));
            //float result = overfraq / underfraq;
            //return result;
        }
    }
}
