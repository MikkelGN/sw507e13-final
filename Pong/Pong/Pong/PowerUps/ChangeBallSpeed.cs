using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pong.PowerUps
{
    public class ChangeBallSpeed : PowerUp
    {
        public float speedAmount;
        public ChangeBallSpeed(float speed)
        {
            speedAmount = speed;
        }
    }
}
