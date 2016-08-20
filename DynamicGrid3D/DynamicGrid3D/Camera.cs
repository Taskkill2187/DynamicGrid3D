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
    public class Camera
    {
        public Matrix ViewMatrix;
        public Matrix ProjectionMatrix;
        public Vector3 CameraPos;
        public Vector3 OriginPos;
        public Quaternion RotQuar;
        public Matrix WorldMatrix;
        public static float MouseSensitivity = 0.003f;
        public float YRot = 0;
        public float XRot = 0;
        public float Distance = 10;

        public void DrawCoords(SpriteBatch SB)
        {
            SB.DrawString(Assets.Font, "CameraPos: " + CameraPos.ToString(), new Vector2(12, 12), Color.White);
        }
        public void DrawAxes(SpriteBatch SB, GraphicsDevice GD)
        {
            SB.DrawString(Assets.Font, "0", new Vector2(GD.Viewport.Project(Vector3.Zero, ProjectionMatrix, ViewMatrix, Matrix.Identity).X,
                GD.Viewport.Project(Vector3.Zero, ProjectionMatrix, ViewMatrix, Matrix.Identity).Y), Color.Black);

            SB.DrawString(Assets.Font, "X", new Vector2(GD.Viewport.Project(new Vector3(5, 0, 0), ProjectionMatrix, ViewMatrix, Matrix.Identity).X,
                GD.Viewport.Project(new Vector3(5, 0, 0), ProjectionMatrix, ViewMatrix, Matrix.Identity).Y), Color.Black);

            SB.DrawString(Assets.Font, "Y", new Vector2(GD.Viewport.Project(new Vector3(0, 5, 0), ProjectionMatrix, ViewMatrix, Matrix.Identity).X,
                GD.Viewport.Project(new Vector3(0, 5, 0), ProjectionMatrix, ViewMatrix, Matrix.Identity).Y), Color.Black);

            SB.DrawString(Assets.Font, "Z", new Vector2(GD.Viewport.Project(new Vector3(0, 0, 5), ProjectionMatrix, ViewMatrix, Matrix.Identity).X,
                GD.Viewport.Project(new Vector3(0, 0, 5), ProjectionMatrix, ViewMatrix, Matrix.Identity).Y), Color.Black);

            SB.DrawString(Assets.Font, "-X", new Vector2(GD.Viewport.Project(new Vector3(-5, 0, 0), ProjectionMatrix, ViewMatrix, Matrix.Identity).X,
                GD.Viewport.Project(new Vector3(-5, 0, 0), ProjectionMatrix, ViewMatrix, Matrix.Identity).Y), Color.Black);

            SB.DrawString(Assets.Font, "-Y", new Vector2(GD.Viewport.Project(new Vector3(0, -5, 0), ProjectionMatrix, ViewMatrix, Matrix.Identity).X,
                GD.Viewport.Project(new Vector3(0, -5, 0), ProjectionMatrix, ViewMatrix, Matrix.Identity).Y), Color.Black);

            SB.DrawString(Assets.Font, "-Z", new Vector2(GD.Viewport.Project(new Vector3(0, 0, -5), ProjectionMatrix, ViewMatrix, Matrix.Identity).X,
                GD.Viewport.Project(new Vector3(0, 0, -5), ProjectionMatrix, ViewMatrix, Matrix.Identity).Y), Color.Black);
        }

        public virtual void Update()
        {

        }
        public virtual void SetShaderArguments()
        {

        }
    }
    
    public class ThirdPersonCamera : Camera
    {
        public ThirdPersonCamera(Vector3 CameraPos0, GraphicsDevice GD)
        {
            CameraPos = CameraPos0;
            ViewMatrix = Matrix.CreateLookAt(CameraPos, new Vector3(0, 0, 0), Vector3.Up);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GD.Viewport.AspectRatio, 1.0f, 300.0f);
            OriginPos = Vector3.Zero;
            WorldMatrix = Matrix.Identity;
        }
        void HandleControls()
        {
            if (Control.CurKS.IsKeyDown(Keys.W))
                OriginPos -= new Vector3((float)Math.Sin(YRot) * 0.1f, 0, (float)Math.Cos(YRot) * 0.1f);

            if (Control.CurKS.IsKeyDown(Keys.S))
                OriginPos += new Vector3((float)Math.Sin(YRot) * 0.1f, 0, (float)Math.Cos(YRot) * 0.1f);

            if (Control.CurKS.IsKeyDown(Keys.A))
                OriginPos += new Vector3((float)Math.Sin(YRot-Math.PI / 2) * 0.1f, 0, (float)Math.Cos(YRot-Math.PI / 2) * 0.1f);

            if (Control.CurKS.IsKeyDown(Keys.D))
                OriginPos += new Vector3((float)Math.Sin(YRot+Math.PI / 2) * 0.1f, 0, (float)Math.Cos(YRot+Math.PI / 2) * 0.1f);

            if (Control.CurKS.IsKeyDown(Keys.Space))
                OriginPos.Y += 0.1f;

            if (Control.CurKS.IsKeyDown(Keys.LeftShift))
                OriginPos.Y -= 0.1f;

            XRot -= (Control.CurMS.Y - (int)Values.WindowSize.Y / 2) * MouseSensitivity;
            YRot -= (Control.CurMS.X - (int)Values.WindowSize.X / 2) * MouseSensitivity;
            Mouse.SetPosition((int)Values.WindowSize.X / 2, (int)Values.WindowSize.Y / 2);
            Distance -= (Control.CurMS.ScrollWheelValue - Control.LastMS.ScrollWheelValue) / 100f;
        }
        public new void SetShaderArguments()
        {
            Vector3 lightDirection = new Vector3(0.5f, -0.5f, -1.0f);
            lightDirection.Normalize(); Assets.Default.Parameters["xLightDirection"].SetValue(lightDirection);
            Assets.Default.Parameters["xAmbient"].SetValue(0.1f);
            Assets.Default.CurrentTechnique = Assets.Default.Techniques["Colored"];
            Assets.Default.Parameters["xView"].SetValue(ViewMatrix);
            Assets.Default.Parameters["xEnableLighting"].SetValue(true);
            Assets.Default.Parameters["xProjection"].SetValue(ProjectionMatrix);
            Assets.Default.Parameters["xWorld"].SetValue(WorldMatrix);
            Assets.Default.Parameters["xCamPos"].SetValue(CameraPos);
        }
        public new void Update()
        {
            HandleControls();

            if (YRot > (float)Math.PI * 2)
                YRot = 0;

            if (YRot < -(float)Math.PI * 2)
                YRot = 0;

            if (XRot > (float)Math.PI * 2)
                XRot = 0;

            if (XRot < -(float)Math.PI * 2)
                XRot = 0;

            RotQuar = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), YRot) * Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), XRot);
            CameraPos = new Vector3(0, 3, Distance);
            CameraPos = Vector3.Transform(CameraPos, Matrix.CreateFromQuaternion(RotQuar));
            CameraPos += OriginPos;
            ViewMatrix = Matrix.CreateLookAt(CameraPos, OriginPos, Vector3.Transform(Vector3.Up, Matrix.CreateFromQuaternion(RotQuar)));
        }
    }
}
