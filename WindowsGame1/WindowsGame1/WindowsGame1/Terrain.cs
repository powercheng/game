//author peng cheng
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class Terrain
    {
        GraphicsDevice device;

        VertexPositionNormalTexture[] terrainVertices;
        int[] terrainIndices;
        VertexBuffer terrainVertexBuffer;
        IndexBuffer terrainIndexBuffer;
        BasicEffect effect;

        Texture2D texture;

        int width = 128;
        int length = 128;

        public Terrain(Game1 game,Camera camera,GraphicsDevice device,BasicEffect effect)
        {
            this.effect = effect;
            this.device = device;
            LoadVertices();
            LoadTextures(game);
        }

        private void LoadTextures(Game1 game)
        {
            texture = game.Content.Load<Texture2D>("Textures\\grass");
        }

        private void LoadVertices()
        {
            terrainVertices = SetUpTerrainVertices();
            terrainIndices = SetUpTerrainIndices();
            terrainVertices = CalculateNormals(terrainVertices, terrainIndices);
            CopyToTerrainBuffers(terrainVertices, terrainIndices);
            // terrainVertexDeclaration = new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
        }

        private VertexPositionNormalTexture[] SetUpTerrainVertices()
        {
            VertexPositionNormalTexture[] terrainVertices = new VertexPositionNormalTexture[width * length];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    terrainVertices[x + y * width].Position = new Vector3(x, 0, -y);
                    terrainVertices[x + y * width].TextureCoordinate.X = (float)x / 30.0f;
                    terrainVertices[x + y * width].TextureCoordinate.Y = (float)y / 30.0f;
                }
            }
            return terrainVertices;
        }

        private int[] SetUpTerrainIndices()
        {
            int[] indices = new int[(width - 1) * (length - 1) * 6];
            int counter = 0;
            for (int y = 0; y < length - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    int lowerLeft = x + y * width;
                    int lowerRight = (x + 1) + y * width;
                    int topLeft = x + (y + 1) * width;
                    int topRight = (x + 1) + (y + 1) * width;

                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;

                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = lowerRight;
                }
            }

            return indices;
        }


        private VertexPositionNormalTexture[] CalculateNormals(VertexPositionNormalTexture[] vertices, int[] indices)
        {
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];

                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;
            }

            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal.Normalize();

            return vertices;
        }

        private void CopyToTerrainBuffers(VertexPositionNormalTexture[] vertices, int[] indices)
        {
            terrainVertexBuffer = new VertexBuffer(device, typeof(VertexPositionNormalTexture), vertices.Length, BufferUsage.WriteOnly);
            terrainVertexBuffer.SetData(vertices);

            terrainIndexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
            terrainIndexBuffer.SetData(indices);
        }


        public void Draw(Camera camera)
        {

            effect.TextureEnabled = true;
            effect.Texture = texture;

            Matrix worldMatrix = Matrix.Identity;
            effect.Projection = camera.projection;
            effect.View = camera.view; ;
            effect.World = worldMatrix;
            effect.LightingEnabled = true;
            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
            effect.DirectionalLight0.Direction = new Vector3(0, -1, 0);
            effect.AmbientLightColor = new Vector3(0.3f);

            device.SetVertexBuffer(terrainVertexBuffer);
            device.Indices = terrainIndexBuffer;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, terrainVertices.Length, 0,
    terrainIndices.Length / 3);

            }
        }
    }
}
