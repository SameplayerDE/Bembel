using Microsoft.Xna.Framework.Graphics;

namespace Bembel.Data;

public class TextureUtils
{
    public static Texture2D LoadTextureFromPath(string path, GraphicsDevice graphicsDevice)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException();
        }
        
        var stream = File.Open(path, FileMode.Open);
        var texture = Texture2D.FromStream(graphicsDevice, stream);
        
        stream.Close();
        stream.Dispose();

        return texture;
    }
}