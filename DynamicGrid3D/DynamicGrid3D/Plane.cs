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
    public class Plane
    {
        public VertexPositionColor[] vertices = new VertexPositionColor[4];
        short[] indices = new short[12];

        public Plane(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, Color Col)
        {
            vertices[0] = new VertexPositionColor(P0, Col);
            vertices[1] = new VertexPositionColor(P1, Col);
            vertices[2] = new VertexPositionColor(P2, Col);
            vertices[3] = new VertexPositionColor(P3, Col);

            
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 2;
            indices[4] = 3;
            indices[5] = 0;

            indices[6] = 0;
            indices[7] = 3;
            indices[8] = 2;
            indices[9] = 2;
            indices[10] = 1;
            indices[11] = 0;
        }

        public void Draw(GraphicsDevice GD)
        {
            GD.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3, VertexPositionColor.VertexDeclaration);
        }
    }
}
