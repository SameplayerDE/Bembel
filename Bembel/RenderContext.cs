using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bembel;

public class RenderContext
{
    public SpriteBatch SpriteBatch;
    public GraphicsDevice GraphicsDevice;
    public Matrix View;
    public Matrix Projection;
}