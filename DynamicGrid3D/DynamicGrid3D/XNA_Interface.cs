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
    public class XNA_Interface : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public XNA_Interface()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = (int)Values.WindowSize.X;
            graphics.PreferredBackBufferHeight = (int)Values.WindowSize.Y;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Assets.Load(Content, GraphicsDevice);
            GameManager.Load(GraphicsDevice);
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                Control.Update();
                FPSCounter.Update(gameTime);

                if (Control.CurKS.IsKeyDown(Keys.Escape))
                    this.Exit();

                GameManager.Update();
            }
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            RasterizerState rs = new RasterizerState(); rs.FillMode = GameManager.FM; GraphicsDevice.RasterizerState = rs;
            GraphicsDevice.Clear(Color.LightBlue);
            GameManager.Camera.SetShaderArguments();
            foreach (EffectPass pass in Assets.Default.CurrentTechnique.Passes) { pass.Apply();
                GameManager.Draw3D(GraphicsDevice);
            }
            spriteBatch.Begin();
            GameManager.Draw2D(spriteBatch, GraphicsDevice);
            spriteBatch.End();
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            base.Draw(gameTime);
        }
    }
}
