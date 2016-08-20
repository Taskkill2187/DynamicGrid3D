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
    public class GridSpring
    {
        public GridPoint End1;
        public GridPoint End2;
        private float Length0;

        public GridSpring(GridPoint End1, GridPoint End2)
        {
            this.End1 = End1;
            this.End2 = End2;
            Length0 = (End1.Pos - End2.Pos).Length();
        }

        public void Update()
        {
            lock (End1)
            {
                lock (End2)
                {
                    End1.ApplyForce((End2.Pos - End1.Pos) / 5);
                    End2.ApplyForce((End1.Pos - End2.Pos) / 5);

                    //End1.ApplyForce(new Vector3(0, (End2.Pos.Y - End1.Pos.Y) / 25, 0));
                    //End2.ApplyForce(new Vector3(0, (End1.Pos.Y - End2.Pos.Y) / 25, 0));
                }
            }
        }
    }
}
