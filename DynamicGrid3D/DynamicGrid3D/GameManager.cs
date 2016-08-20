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
    public static class GameManager
    {
        public static FillMode FM = FillMode.Solid;
        public static DynamicGrid Grid = new DynamicGrid(181, 181);
        public static ThirdPersonCamera Camera;
        public static Type CameraType;

        public static void Load(GraphicsDevice GD)
        {
            Camera = new ThirdPersonCamera(new Vector3(0, 3, 10), GD);
            CameraType = Camera.GetType();
        }

        public static void UpdateFillMode()
        {
            if (Control.WasKeyJustPressed(Keys.R))
            {
                if (FM == FillMode.Solid)
                {
                    FM = FillMode.WireFrame;
                }
                else
                {
                    FM = FillMode.Solid;
                }
            }
        }
        public static void Update()
        {
            Camera.Update();
            UpdateFillMode();
            Grid.Update();
        }

        public static void Draw3D(GraphicsDevice GD)
        {
            Grid.Draw(GD);
        }
        public static void Draw2D(SpriteBatch SB, GraphicsDevice GD)
        {
            SB.DrawString(Assets.Font, "Vertices: " + Grid.vertices.GetLength(0).ToString(), new Vector2(Values.WindowSize.X / 2 -
                Assets.Font.MeasureString("Vertices: " + Grid.vertices.GetLength(0).ToString()).X / 2, 12), Color.White);
            Camera.DrawCoords(SB);
            Camera.DrawAxes(SB, GD);
            Assets.DrawLine(new Vector3(-10, 0, 0), new Vector3(10, 0, 0), 2, Color.Red, SB, GD.Viewport);
            FPSCounter.Draw(SB);
        }
    }
}
