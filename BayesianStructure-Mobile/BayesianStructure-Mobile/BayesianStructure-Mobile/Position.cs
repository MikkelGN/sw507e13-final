using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BayesianStructure
{
    public class Position : Variable
    {
        internal Variable _ball;

        internal Position(Variable previous, Variable parent, Variable ball)
            : base()
        {
            Previous = previous;
            Parent = parent;
            _ball = ball;

            UpdateVariance();
            UpdateMean();
        }

        internal Position(float variance, float mean)
            : base()
        {
            Variance = variance;
            Mean = mean;
        }


        protected override void UpdateVariance()
        {
            Variance = Constants.POSITION_VARIANCE_GAIN + MatrixMultiplication(Constants.F_POSITION, GetCoVarianceMatrix());
        }

        protected override float[,] GetCoVarianceMatrix()
        {
            return new float[3, 3] {{Parent.Variance,0,0},{0,Previous.Variance,0},{0,0,_ball.Variance}};
        }

        protected override float MatrixMultiplication(float[,] firstMatrix, float[,] secondMatrix)
        {
            return (float)Math.Pow((double)firstMatrix[0, 0], 2) * secondMatrix[0, 0] +
                   (float)Math.Pow((double)firstMatrix[0, 1], 2) * secondMatrix[1, 1] +
                   (float)Math.Pow((double)firstMatrix[0, 2], 2) * secondMatrix[2, 2];
        }


        protected override void UpdateMean()
        {
            Mean = Constants.POSITION_MEAN_GAIN + 
                   Constants.F_POSITION[0, 0] * Parent.Mean + 
                   Constants.F_POSITION[0, 1] * Previous.Mean + 
                   Constants.F_POSITION[0, 2] * _ball.Mean;

            if (Mean > Constants.POSITION_MAX || Mean < Constants.POSITION_MIN)
            {
                Mean = Mean > Constants.POSITION_MAX ? Constants.POSITION_MAX : Constants.POSITION_MIN;

                Variance = 0.0f;
                Parent.Variance = 0.0f;

                if (Constants.SHOW_ERROR_MESSAGES)
                {
                    if(Mean > Constants.POSITION_MAX)
                        Console.WriteLine("Encountered constraint: Position greater than " + Constants.POSITION_MAX);
                    else
                        Console.WriteLine("Encountered constraint: Position less than " + Constants.POSITION_MIN);
                }
            }
        }
    }
}
