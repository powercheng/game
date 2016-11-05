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
        Model myModel;
        Camera camera;

        public Vector3 modelPosition;
        public Vector3 modelRotation;

        public Character(Camera camera, Game1 game)
        {
            this.camera = camera;
            this.modelPosition = new Vector3(0.0f, 0.0f,0.0f);
            this.modelRotation = new Vector3(0.0f, 0.0f, 0.0f);
            myModel = game.Content.Load<Model>("Models\\p1_wedge");

        }

        public void update(GameTime gameTime)
        {   
            
        }

        public void Draw()
        {
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(transforms);
            Matrix world = Matrix.CreateScale(0.001f) * Matrix.CreateRotationX(modelRotation.X) * Matrix.CreateRotationY(modelRotation.Y
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
