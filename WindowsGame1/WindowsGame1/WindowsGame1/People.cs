using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;

namespace WindowsGame1
{
    class People
    {
        Model myModel;
        Camera camera;

        public Vector3 modelPosition;
        public Vector3 modelRotation;
        public Matrix world;

        AnimationPlayer animationPlayer;

        public People(Camera camera, Game1 game)
        {
            this.camera = camera;
            this.modelPosition = new Vector3(0.0f, 0.0f, 0.0f);
            this.modelRotation = new Vector3(0.0f, 0.0f, 0.0f);
            myModel = game.Content.Load<Model>("Models\\dude");
            SkinningData skinningData = myModel.Tag as SkinningData;

            animationPlayer = new AnimationPlayer(skinningData);

            AnimationClip clip = skinningData.AnimationClips["Take 001"];

            animationPlayer.StartClip(clip);

        }

        public void update(GameTime gameTime)
        {
            world = Matrix.CreateScale(0.01f) * Matrix.CreateRotationX(modelRotation.X) * Matrix.CreateRotationY(modelRotation.Y
                        ) * Matrix.CreateRotationZ(modelRotation.Z)
                    * Matrix.CreateTranslation(modelPosition);
            animationPlayer.Update(gameTime.ElapsedGameTime, true, world);
        }

        public void Draw()
        {
            // Copy any parent transforms.
            Matrix[] bones = animationPlayer.GetSkinTransforms();
            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);
             //       effect.World = bones[mesh.ParentBone.Index] * world;
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }

    }
}
