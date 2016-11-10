// author peng cheng
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class Character
    {
        //Cizhen
        public Model myModel;
        public bool isActive;
        int live;

        //Cizhen
        Camera camera;

        public Vector3 modelPosition;
        public Vector3 modelRotation;

        float ratio;

        //public Character(Camera camera, Game1 game)
        public Character(Camera camera, Game1 game, String path, float r, Vector3 pos, Vector3 rot)
        {
            this.camera = camera;
            this.modelPosition = pos;
            this.modelRotation = rot;
            //myModel = game.Content.Load<Model>("Models\\p1_wedge");

            myModel = game.Content.Load<Model>(path);
            ratio = r;
            live = 30;
            isActive = true;
        }

        public void update(GameTime gameTime, float x, float z)
        {
            //Cizhen Wu
            float xDiff = modelPosition.X - x;
            float zDiff = modelPosition.Z - z;


            modelPosition.X -= xDiff / 50;
            modelPosition.Z -= zDiff / 50;
            //Cizhen Wu
        }

        public void Draw()
        {
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(transforms);
            Matrix world = Matrix.CreateScale(ratio) * Matrix.CreateRotationX(modelRotation.X) * Matrix.CreateRotationY(modelRotation.Y
                        ) * Matrix.CreateRotationZ(modelRotation.Z)
                    * Matrix.CreateTranslation(modelPosition);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * world;
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }

    }
}
