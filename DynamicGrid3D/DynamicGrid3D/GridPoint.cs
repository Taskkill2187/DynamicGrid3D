using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DynamicGrid3D
{
    public class GridPoint
    {
        public Vector3 Pos, Vel;
        public bool Moveable = true;
        private float Damping = 0.9999f;

        public GridPoint(Vector3 Pos, bool Moveable)
        {
            this.Pos = Pos;
            Vel = Vector3.Zero;
            this.Moveable = Moveable;
        }

        public void ApplyForce(Vector3 Force) { if (Moveable) { Vel += Force; } }

        public void Update()
        {
            //// X
            //if (Vel.X > 5)
            //    Vel.X = 5;

            //if (Vel.X < -5)
            //    Vel.X = -5;

            // Y
            if (Vel.Y > 5)
                Vel.Y = 5;

            if (Vel.Y < -5)
                Vel.Y = -5;

            //// Z
            //if (Vel.Z > 5)
            //    Vel.Z = 5;

            //if (Vel.Z < -5)
            //    Vel.Z = -5;

            Pos += Vel;
            Vel *= Damping;
        }
    }
}
