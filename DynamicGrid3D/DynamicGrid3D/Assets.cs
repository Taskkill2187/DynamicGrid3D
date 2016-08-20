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
    public static class Assets
    {
        public static SpriteFont Font;
        public static Texture2D White;
        public static Effect Default;

        public static void DrawLine(Vector2 End1, Vector2 End2, int Thickness, Color Col, SpriteBatch SB)
        {
            Vector2 Delta = End1 - End2;
            SB.Draw(White, End1, null, Col, -(float)Math.Atan2(Delta.X, Delta.Y) - (float)Math.PI / 2, new Vector2(0, 0.5f), new Vector2(Delta.Length(), Thickness), SpriteEffects.None, 0f);
        }

        public static void DrawLine(Vector3 End1, Vector3 End2, int Thickness, Color Col, SpriteBatch SB, Viewport VP)
        {
            Vector2 UnProjectedPos1 = new Vector2(VP.Unproject(End1, GameManager.Camera.ProjectionMatrix, GameManager.Camera.ViewMatrix, GameManager.Camera.WorldMatrix).X,
                                                  VP.Unproject(End1, GameManager.Camera.ProjectionMatrix, GameManager.Camera.ViewMatrix, GameManager.Camera.WorldMatrix).Y);
            Vector2 UnProjectedPos2 = new Vector2(VP.Unproject(End2, GameManager.Camera.ProjectionMatrix, GameManager.Camera.ViewMatrix, GameManager.Camera.WorldMatrix).X,
                                                  VP.Unproject(End2, GameManager.Camera.ProjectionMatrix, GameManager.Camera.ViewMatrix, GameManager.Camera.WorldMatrix).Y);
            DrawLine(UnProjectedPos1, UnProjectedPos2, Thickness, Col, SB);
        }

        public static void DrawCircle(Vector2 Pos, float Radius, Color Col, SpriteBatch SB)
        {
            for (int i = -(int)Radius; i < (int)Radius; i++)
            {
                int HalfHeight = (int)Math.Sqrt(Radius * Radius - i * i);
                SB.Draw(White, new Rectangle((int)Pos.X + i, (int)Pos.Y - HalfHeight, 1, HalfHeight * 2), Col);
            }
        }

        public static void Load(ContentManager Content, GraphicsDevice GD)
        {
            Default = Content.Load<Effect>("Default");
            Font = Content.Load<SpriteFont>("Font");
            White = new Texture2D(GD, 1, 1);
            Color[] Col = new Color[1];
            Col[0] = Color.White;
            White.SetData<Color>(Col);
        }
    }
}
