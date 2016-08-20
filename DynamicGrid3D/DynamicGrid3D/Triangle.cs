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
    public class ColorTriangle
    {
        public VertexPositionColor[] Edges = new VertexPositionColor[3];
        
        public ColorTriangle(Vector3 P0, Vector3 P1, Vector3 P2, Color Col)
        {
            Edges[0] = new VertexPositionColor();
            Edges[1] = new VertexPositionColor();
            Edges[2] = new VertexPositionColor();

            Edges[0].Position = P0;
            Edges[1].Position = P1;
            Edges[2].Position = P2;

            Edges[0].Color = Col;
            Edges[1].Color = Col;
            Edges[2].Color = Col;
        }

        public void Draw(GraphicsDevice GD)
        {
            GD.DrawUserPrimitives(PrimitiveType.TriangleList, Edges, 0, 1, VertexPositionColor.VertexDeclaration);
        }
    }
    public class ColorNormalTriangle
    {
        public VertexPositionColorNormal[] Edges = new VertexPositionColorNormal[3];

        public ColorNormalTriangle(Vector3 P0, Vector3 P1, Vector3 P2, Color Col)
        {
            Edges[0] = new VertexPositionColorNormal();
            Edges[1] = new VertexPositionColorNormal();
            Edges[2] = new VertexPositionColorNormal();

            Edges[0].Position = P0;
            Edges[1].Position = P1;
            Edges[2].Position = P2;

            Edges[0].Color = Col;
            Edges[1].Color = Col;
            Edges[2].Color = Col;
        }

        public void Draw(GraphicsDevice GD)
        {
            Vector3 Side1 = Edges[1].Position - Edges[0].Position;
            Vector3 Side2 = Edges[2].Position - Edges[0].Position;
            Vector3 Normal = Vector3.Cross(Side1, Side2);

            Edges[0].Normal = Normal;
            Edges[1].Normal = Normal;
            Edges[2].Normal = Normal;

            GD.DrawUserPrimitives(PrimitiveType.TriangleList, Edges, 0, 1, VertexPositionColorNormal.VertexDeclaration);
        }
    }
}
