// author : peng cheng

using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    class Camera
    {
        public Matrix projection;
        public Matrix view;
        public float rotationx;
        public float rotationz;
        public Vector3 pos;


        public Camera(Game1 game)
        {
            this.pos = new Vector3(0, 0, -30);
            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.aspectRatio, 1f, 1000.0f);
            this.view = Matrix.CreateLookAt(pos,
                        Vector3.Zero, Vector3.Up);
            rotationx = 0.0f;
            rotationz = 0.0f;
        }

        public void update(GameTime gameTime,People c,Game1 game)
        {
            pos = new Vector3(0, 1f, 2f);
            Matrix rotation = Matrix.CreateRotationX(rotationx) * Matrix.CreateRotationY(rotationz);
            pos = Vector3.Transform(pos, rotation);
            pos += c.modelPosition;

            Vector3 camup = new Vector3(0, 1, 0);
            camup = Vector3.Transform(camup, rotation);

            view = Matrix.CreateLookAt(pos, c.modelPosition + new Vector3(0.0f, 1.0f, 0.0f), camup);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.aspectRatio, 0.2f, 500.0f);
        }

        public void Draw()
        {

        }
    }
}