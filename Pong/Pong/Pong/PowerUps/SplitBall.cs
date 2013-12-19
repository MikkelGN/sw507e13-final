using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong.PowerUps
{
    public class SplitBall : PowerUp
    {
        public int _ballAmount;

        public SplitBall(int splitAmount)
        {
            splitAmount = splitAmount > 3 ? 3 : splitAmount;
            _ballAmount = splitAmount;
        }
    }
}
