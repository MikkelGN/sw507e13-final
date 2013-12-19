using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pong.PowerUps;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public static class PowerUpHelper
    {
        public static bool Initialized { get; set; }
        private static Random randomPowerup = new Random();
        private static List<PowerUp> powerUps = null;
        public static List<PowerUp> PowerUps {
            get
            {
                if (powerUps == null)
                {
                    InitializePowerUps();
                }
                return powerUps; } 
            set { powerUps = value; } 
        }

        private static void InitializePowerUps()
        {
            powerUps = new List<PowerUp>();
        }


        public static PowerUp GetRandomPowerUp()
        {
            return PowerUps.ElementAt(randomPowerup.Next(0, PowerUps.Count));
        }

    }
}
