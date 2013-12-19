using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BayesianStructure
{
    internal class CorrectedMovingAverage : Variable
    {
        static private Stack<Variable> movingAverageAccelerationStack = new Stack<Variable>();
        static private StepDirection _previousStepDirection = StepDirection.Unknown;
        static private bool _decellerating = false;
        static private Network _theNetwork;

        public CorrectedMovingAverage(Variable parent)
            : base()
        {
            
            Parent = parent;
            Mean = GetAccelerationDirection(Parent).Mean;
            Variance = parent.Variance;
        }

        public CorrectedMovingAverage(Network theNetwork)
            : base()
        {
            movingAverageAccelerationStack.Clear();
            _theNetwork = theNetwork;
        }

        protected override void UpdateVariance()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateMean()
        {
            throw new NotImplementedException();
        }

        public Variable GetAccelerationDirection(Variable movingAverageAcceleration)
        {
            //If we don't know the step direction
            if (_previousStepDirection == StepDirection.Unknown)
            {
                movingAverageAccelerationStack.Clear();
                _theNetwork.Velocity.Mean = 0;
                if (movingAverageAcceleration.Mean < Constants.JITTER_MIN)
                {
                    _previousStepDirection = StepDirection.Left;
                    movingAverageAccelerationStack.Push(movingAverageAcceleration);
                }
                else if (movingAverageAcceleration.Mean > Constants.JITTER_MAX)
                {
                    _previousStepDirection = StepDirection.Right;
                    movingAverageAccelerationStack.Push(movingAverageAcceleration);
                }
            }
            //If we know the step direction
            else
            {
                StepDirection currentStepDirection = StepDirection.Unknown;
                if(movingAverageAcceleration.Mean < Constants.JITTER_MIN){
                    currentStepDirection = StepDirection.Left;
                }
                else if(movingAverageAcceleration.Mean > Constants.JITTER_MAX){
                    currentStepDirection = StepDirection.Right;
                }

                if (currentStepDirection == _previousStepDirection)
                {
                    if (movingAverageAccelerationStack.Count > 0 && _decellerating)
                    {
                        _decellerating = false;
                        movingAverageAccelerationStack.Clear();
                        _theNetwork.Velocity.Mean = 0;
                    }
                    movingAverageAccelerationStack.Push(movingAverageAcceleration);
                }
                else
                {
                    _decellerating = true;
                    if (movingAverageAccelerationStack.Count > 0)
                    {
                        Variable tempAcceleration = movingAverageAccelerationStack.Pop();
                        tempAcceleration.Mean *= -1;
                        return tempAcceleration;
                    }
                    movingAverageAcceleration.Mean = 0f;
                    _previousStepDirection = currentStepDirection;
                }
            }
            return movingAverageAcceleration;
        }
    }
}
