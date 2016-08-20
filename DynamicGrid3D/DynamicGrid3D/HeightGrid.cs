using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DynamicGrid3D
{
    public struct VertexPositionColorNormal
    {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
        );
    }

    public class DynamicGrid
    {
        int terrainWidth;
        int terrainHeight;
        GridPoint[,] Points;
        public VertexPositionColorNormal[] vertices;
        List<GridSpring> Springs;
        short[] indices;
        const int InvertedSize = 5;
        const bool BothSided = false;
        int WaveTimer;

        public DynamicGrid(int terrainWidth, int terrainHeight)
        {
            this.terrainWidth = terrainWidth;
            this.terrainHeight = terrainHeight;

            // LoadHeightData
            Points = new GridPoint[terrainWidth, terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    if (x == 0 || x == terrainWidth - 1 || y == 0 || y == terrainHeight - 1)
                    {
                        Points[x, y] = new GridPoint(new Vector3(x - terrainWidth / 2f, 0, -y + terrainHeight / 2f), false);
                    }
                    else
                    {
                        Points[x, y] = new GridPoint(new Vector3(x - terrainWidth / 2f, 0, -y + terrainHeight / 2f), true);
                    }
                }
            }

            // LoadVertecies
            vertices = new VertexPositionColorNormal[terrainWidth * terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    vertices[x + y * terrainWidth].Position = new Vector3((x - terrainWidth / 2f) / InvertedSize, 0, (-y + terrainHeight / 2f) / InvertedSize);
                    vertices[x + y * terrainWidth].Color = Color.FromNonPremultiplied(0, 66, 291, 255);
                }
            }

            //LoadIndicies
            indices = new short[(terrainWidth - 1) * (terrainHeight - 1) * 12];
            int counter = 0;
            for (int y = 0; y < terrainHeight - 1; y++)
            {
                for (int x = 0; x < terrainWidth - 1; x++)
                {
                    short lowerLeft = (short)(x + y * terrainWidth);
                    short lowerRight = (short)((x + 1) + y * terrainWidth);
                    short topLeft = (short)(x + (y + 1) * terrainWidth);
                    short topRight = (short)((x + 1) + (y + 1) * terrainWidth);

                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;

                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = lowerRight;

                    if (BothSided)
                    {
                        indices[counter++] = lowerRight;
                        indices[counter++] = topRight;
                        indices[counter++] = topLeft;

                        indices[counter++] = lowerLeft;
                        indices[counter++] = lowerRight;
                        indices[counter++] = topLeft;
                    }
                }
            }

            Springs = new List<GridSpring>();
            // Create the horz Springs
            for (int ix = 0; ix < Points.GetLength(0) - 1; ix++)
            {
                for (int iy = 0; iy < Points.GetLength(1); iy++)
                {
                    Springs.Add(new GridSpring(Points[ix, iy], Points[ix + 1, iy]));
                }
            }

            // Create the vert Springs
            for (int ix = 0; ix < Points.GetLength(0); ix++)
            {
                for (int iy = 0; iy < Points.GetLength(1) - 1; iy++)
                {
                    Springs.Add(new GridSpring(Points[ix, iy], Points[ix, iy + 1]));
                }
            }

            CalculateNormals();
        }
        
        void CalculateNormals()
        {
            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];

                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal = normal;
                vertices[index2].Normal = normal;
                vertices[index3].Normal = normal;
            }

            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal.Normalize();
        }
        void UpdateSprings()
        {
            for (int i = 0; i < Springs.Count; i++)
            {
                Springs[i].Update();
            }
        }
        public void Reset()
        {
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    if (x == 0 || x == terrainWidth - 1 || y == 0 || y == terrainHeight - 1)
                    {
                        Points[x, y] = new GridPoint(new Vector3(x - terrainWidth / 2f, 0, -y + terrainHeight / 2f), false);
                    }
                    else
                    {
                        Points[x, y] = new GridPoint(new Vector3(x - terrainWidth / 2f, 0, -y + terrainHeight / 2f), true);
                    }
                }
            }

            Springs = new List<GridSpring>();
            // Create the horz Springs
            for (int ix = 0; ix < Points.GetLength(0) - 1; ix++)
            {
                for (int iy = 0; iy < Points.GetLength(1); iy++)
                {
                    Springs.Add(new GridSpring(Points[ix, iy], Points[ix + 1, iy]));
                }
            }

            // Create the vert Springs
            for (int ix = 0; ix < Points.GetLength(0); ix++)
            {
                for (int iy = 0; iy < Points.GetLength(1) - 1; iy++)
                {
                    Springs.Add(new GridSpring(Points[ix, iy], Points[ix, iy + 1]));
                }
            }
        }
        public void ApplyForce(int x, int y, float Strength)
        {
            if (x >= 0 && y >= 0 && x < terrainWidth && y < terrainHeight)
                Points[x, y].ApplyForce(new Vector3(0, Strength, 0));
        }

        public void Update()
        {
            if (Control.CurKS.IsKeyDown(Keys.E) || WaveTimer > 120)
            {
                ApplyForce(terrainWidth / 2, terrainHeight / 2, 0.5f);
                WaveTimer = 0;
            }

            if (Control.CurKS.IsKeyDown(Keys.Q))
            {
                ApplyForce(terrainWidth / 2, terrainHeight / 2, -0.5f);
            }

            if (Control.WasKeyJustPressed(Keys.X))
            {
                Reset();
            }

            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    Points[x, y].Update();
                    vertices[x + y * terrainWidth].Position = Points[x, y].Pos;
                    vertices[x + y * terrainWidth].Position.Y *= 1;
                    vertices[x + y * terrainWidth].Position.X /= InvertedSize;
                    vertices[x + y * terrainWidth].Position.Z /= InvertedSize;
                }
            }

            Task.Factory.StartNew(() => UpdateSprings());
            Task.Factory.StartNew(() => CalculateNormals());
            Task.WaitAll();

            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    if (Points[x, y].Pos.Y > 0)
                        vertices[x + y * terrainWidth].Color = Color.Lerp(Color.FromNonPremultiplied(0, 66, 191, 255), Color.FromNonPremultiplied(10, 100, 255, 255), Points[x, y].Pos.Y);
                    else
                        vertices[x + y * terrainWidth].Color = Color.Lerp(Color.FromNonPremultiplied(0, 66, 191, 255), Color.FromNonPremultiplied(0, 25, 125, 255), -Points[x, y].Pos.Y);
                }
            }
        }
        public void Draw(GraphicsDevice GD)
        {
            GD.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3, VertexPositionColorNormal.VertexDeclaration);
        }
    }
}
