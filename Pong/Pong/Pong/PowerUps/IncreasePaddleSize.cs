using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong.PowerUps
{
    public class IncreasePaddleSize : PowerUp
    {
        public float IncreaseFactor { get; set; }
        
        float increaseFactor = 2f;

        public IncreasePaddleSize(float increaseFactor)
        {
            IncreaseFactor = increaseFactor;
        }
    }
}
