using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong
{
    public class ChangePoints : PowerUp
    {
        public float scoreMultiplier;
        public ChangePoints(float multiplier)
        {
            scoreMultiplier = multiplier;
        }
    }
}
