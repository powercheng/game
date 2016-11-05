// author : peng cheng

using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    class Camera
    {
        public Matrix projection;
        public Matrix view;

        public Camera(Game1 game)
        {
            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.aspectRatio, 1f, 1000.0f);
            this.view = Matrix.CreateLookAt(new Vector3(130, 30, -50),
                        Vector3.Zero, Vector3.Up);
        }

        public void update(GameTime gameTime,Character c,Game1 game)
        {
            Vector3 campos = new Vector3(0, 2f, 3f);
            campos = Vector3.Transform(campos, Matrix.CreateFromQuaternion(Quaternion.Identity));
            campos += c.modelPosition;

            Vector3 camup = new Vector3(0, 1, 0);
            camup = Vector3.Transform(camup, Matrix.CreateFromQuaternion(Quaternion.Identity));

            view = Matrix.CreateLookAt(campos, c.modelPosition, camup);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.aspectRatio, 0.2f, 500.0f);
        }

        public void Draw()
        {

        }
    }
}