using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong
{
    public class InstantiatedPowerUp
    {
        private Vector2 position = Vector2.Zero;

        public PowerUp PowerUp { get; set; }
        public Vector2 Position { get { return position; } set { position = value; } }
        public bool Delete { get; set; }
    }
}
