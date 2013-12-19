using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Devices.Sensors;
using Pong.PowerUps;

namespace Pong
{
    public class Ball
    {
        public Vector2 Position;
        public Vector2 Speed;
        public Texture2D BallTexture;

        public Ball(Vector2 position, Vector2 speed, ContentManager contentmanager)
        {
            Position = position;
            Speed = speed;
            BallTexture = contentmanager.Load<Texture2D>("ball");
            Position.X = Position.X - BallTexture.Width;
        }

        public Ball(ContentManager contentmanager)
        {
            BallTexture = contentmanager.Load<Texture2D>("ball");
        }
    }
}
