using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BayesianStructure
{
    internal class MovingAverage : Variable
    {
        public MovingAverage(Variable previous, Variable parent)  : base(previous, parent)
        {
        }

        public MovingAverage(float variance, float mean) : base()
        {
            Variance = variance;
            Mean = mean;
        }

        protected override void UpdateVariance()
        {
            //please comment dis
            Variance = Constants.MOVING_AVERAGE_VARIANCE_GAIN + MatrixMultiplication(Constants.F_MOVING_AVERAGE, GetCoVarianceMatrix());
        }

        protected override void UpdateMean()
        {
            //Please comment dis
            Mean = Constants.F_MOVING_AVERAGE[0, 0] * Parent.Mean + Constants.F_MOVING_AVERAGE[0, 1] * Previous.Mean;
        }
    }

    
       
}
