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


namespace WindowsGame1
{
    class Sky
    {

        private Model skyboxModel;
        private Matrix[] skyboxTransforms;
        private Camera _camera;
        BasicEffect basicEffect;
        Game1 game;
        public Sky(Camera camera,Game1 game)
        {
            this._camera = camera;
            this.game = game;
            skyboxModel = game.Content.Load<Model>(@"Skybox\skybox");
            skyboxTransforms = new Matrix[skyboxModel.Bones.Count];
        }

        public void draw()
        {
            game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

            //draw the skybox
            game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            skyboxModel.CopyAbsoluteBoneTransformsTo(skyboxTransforms);

            foreach (ModelMesh mesh in skyboxModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = skyboxTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(_camera.pos);
                    effect.View = _camera.view;
                    effect.Projection = _camera.projection;
                }
                mesh.Draw();
            }


            game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
