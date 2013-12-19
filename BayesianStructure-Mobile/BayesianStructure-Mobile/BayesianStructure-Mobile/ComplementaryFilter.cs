using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BayesianStructure
{
    public class ComplementaryFilter : Variable
    {
        private Variable _aTan2;

        public Vector3 Mean { get; internal set; }
        public Vector3 Variance { get; internal set; }

        public ComplementaryFilter(Variable aTan2, Variable gyroscope, Variable previous)
            : base()
        {
            Previous = previous;
            Parent = gyroscope;
            _aTan2 = aTan2;

            UpdateMean();
        }

        public ComplementaryFilter()
            : base()
        {
            Mean = Vector3.Zero;
            Variance = Vector3.Zero;
        }


        protected override void UpdateVariance()
        {
            Variance = Vector3.Zero;
        }

        protected override void UpdateMean()
        {
            Vector3 temp = new Vector3();

            temp.X = CalculateAngle(((ComplementaryFilter)Previous).Mean.X, ((Gyroscope)Parent).Mean.X, ((ATan2Angle)_aTan2).Mean.X);
            temp.Y = 0f;
            temp.Z = CalculateAngle(((ComplementaryFilter)Previous).Mean.Z, ((Gyroscope)Parent).Mean.Z, ((ATan2Angle)_aTan2).Mean.Z);// WORKS

            Mean = temp;
            //Constants.ACCUMULATED_GYROSCOPEZ_ANGLE = Mean.Z;
            //Constants.ACCUMULATED_GYROSCOPEX_ANGLE = Mean.X;
        }

        private float CalculateAngle(float oldAngle, float gyroScope, float aTan2Angle){
            return (0.98f) * (oldAngle + gyroScope * Constants.DELTA_TIME) + (0.02f) * (aTan2Angle);
        }
    }
}

