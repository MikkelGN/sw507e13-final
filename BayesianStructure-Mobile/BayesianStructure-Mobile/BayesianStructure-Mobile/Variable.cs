using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BayesianStructure
{
    public abstract class Variable
    { 
        #region Properties
        protected Variable Previous { get; set; }
        protected Variable Parent { get; set; }

        public float Mean { get; internal set; }
        public float Variance { get; internal set; }
        #endregion

        protected Variable() { }

        protected Variable(Variable previous, Variable parent) 
        {
            Parent = parent;
            Previous = previous;

            UpdateVariance();
            UpdateMean();
        }

        #region Methods    
        protected virtual float[,] GetCoVarianceMatrix()
        {
            return new float[2, 2] { { Parent.Variance, 0 }, { 0, Previous.Variance } };;
        }

        protected abstract void UpdateVariance();
        protected abstract void UpdateMean();

        /// <summary>
        /// <para>Matrix multiplication for F[1x2] and C [2x2] matrices</para>
        /// <para>F * C * transpose(F)</para>
        /// </summary>
        /// <param name="firstMatrix">F [1x2] dimension</param>
        /// <param name="secondMatrix">C [2x2] dimension</param>
        /// <returns>The matrix multiplication result</returns>
        protected virtual float MatrixMultiplication(float[,] firstMatrix, float[,] secondMatrix)
        {
            return (float)Math.Pow((double)firstMatrix[0, 0], 2) * secondMatrix[0, 0] +
                firstMatrix[0, 0] * firstMatrix[0, 1] * secondMatrix[1, 0] +
                firstMatrix[0, 0] * firstMatrix[0, 1] * secondMatrix[0, 1] +
                (float)Math.Pow((double)firstMatrix[0, 1], 2) * secondMatrix[1, 1];
        }

        #endregion
    }
}