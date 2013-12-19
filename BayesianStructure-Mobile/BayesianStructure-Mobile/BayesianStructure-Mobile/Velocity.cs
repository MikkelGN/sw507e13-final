using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BayesianStructure
{
    internal class Velocity : Variable
    {
        private int _countConstantVelocity { get; set; }
        private static float _initialConstantVelocity;

        public Velocity(Variable previous, Variable parent) : base(previous, parent) 
        {
            _countConstantVelocity = ((Velocity)previous)._countConstantVelocity;
            UpdateConstantVelocityCount();
        }

        public Velocity(float variance, float mean)
            : base()
        {
            Variance = variance;
            Mean = mean;
        }

        public void UpdateConstantVelocityCount()
        {  
            if(Previous != null)
            {
                //Assignment of start constant velocity
                if (_countConstantVelocity == 0)
                {
                    _initialConstantVelocity = Previous.Mean;
                }

                if (Math.Abs(Mean - _initialConstantVelocity) < Constants.CONSTANT_VELOCITY_JITTER_LIMIT)
                {
                    _countConstantVelocity++;
                    if (_countConstantVelocity > Constants.CONSTANT_VELOCITY_LIMIT)
                    {
                        Reset();
                    }
                }
                else
                {
                    _countConstantVelocity = 0;
                }
            }

        }

        public void Reset()
        {
            Mean = 0f;
            Variance = 0f;
        }


        protected override void UpdateVariance()
        {
            Variance = Constants.VELOCITY_VARIANCE_GAIN + MatrixMultiplication(Constants.F_VELOCITY, GetCoVarianceMatrix());    
        }

        protected override void UpdateMean()
        {
            Mean = Constants.F_VELOCITY[0, 0] * Parent.Mean + Constants.F_VELOCITY[0, 1] * Previous.Mean + Constants.VELOCITY_MEAN_GAIN; 

            // Constraints: Maximum velocity is 3 m/s
            if (Mean > Constants.VELOCITY_MAX)
            {
                Mean = Constants.VELOCITY_MAX;
                Variance = 0f;

                if (Constants.SHOW_ERROR_MESSAGES)
                {
                    Console.WriteLine("Encountered constraint: Velocity greater than " + Constants.VELOCITY_MAX);
                }

            }
            else if (Mean < Constants.VELOCITY_MIN)
            {
                Mean = Constants.VELOCITY_MIN;
                Variance = 0f;

                if (Constants.SHOW_ERROR_MESSAGES)
                {
                    Console.WriteLine("Encountered constraint: Velocity less than " + Constants.VELOCITY_MIN);
                }
            }

             
        }
    }
}
