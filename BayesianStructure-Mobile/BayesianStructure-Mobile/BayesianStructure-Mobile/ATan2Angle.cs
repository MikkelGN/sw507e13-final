using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BayesianStructure
{
    public class ATan2Angle : Variable
    {
        public Vector3 Mean { get; internal set; }
        public Vector3 Variance { get; internal set; }

        public ATan2Angle(Variable acceleration) : base()
        {
            Vector3 temp = new Vector3();

            temp.X = (float)Math.Atan2(((Acceleration)acceleration).Mean.Y, -((Acceleration)acceleration).Mean.Z);//Pitching, not sure about this yet
            temp.Y = 0f;//(float)Math.Atan2(((Acceleration)adjustedAcceleration).Mean.Z, -((Acceleration)adjustedAcceleration).Mean.X);
            temp.Z = (float)Math.Atan2(((Acceleration)acceleration).Mean.Y, -((Acceleration)acceleration).Mean.X);//Yawing WORKS

            Mean = temp;
            Variance = Vector3.Zero;
        }

        protected override void UpdateVariance()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateMean()
        {
            throw new NotImplementedException();
        }
    }
}
