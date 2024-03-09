using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bembel;

public class RenderContext
{
    public SpriteBatch SpriteBatch;
    public GraphicsDevice GraphicsDevice;
    public Matrix View;
    public Matrix Projection;
    
    public void DrawText(SpriteFont font, string text, Vector2 position)
    {
        SpriteBatch.DrawString(font, text, position, Color.White);
    }
    
    public void DrawText(SpriteFont font, string text, Point position)
    {
        SpriteBatch.DrawString(font, text, position.ToVector2(), Color.White);
    }
    
    public void DrawText(SpriteFont font, string text, Vector2 position, Color tint)
    {
        SpriteBatch.DrawString(font, text, position, tint);
    }
    
    public void DrawText(SpriteFont font, string text, Point position, Color tint)
    {
        SpriteBatch.DrawString(font, text, position.ToVector2(), tint);
    }
    
    public void DrawTexture(Texture2D texture, Vector2 position)
    {
        SpriteBatch.Draw(texture, position, Color.White);
    }
    
    public void DrawTexture(Texture2D texture, Point position)
    {
        SpriteBatch.Draw(texture, position.ToVector2(), Color.White);
    }
    
    public void DrawTexture(Texture2D texture, Vector2 position, Color tint)
    {
        SpriteBatch.Draw(texture, position, tint);
    }
    
}