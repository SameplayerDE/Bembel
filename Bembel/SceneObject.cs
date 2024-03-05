using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Bembel;

public class SceneObject
{
    public SceneObject? Parent;
    public List<SceneObject> Children;
    private Scene? _scene;

    public Scene? Scene
    {
        get => _scene ?? Parent?.Scene;
        set => _scene = value;
    }

    public bool IsVisible = true;
    public bool IsActive = true;

    public SceneObject()
    {
        Children = new List<SceneObject>();
    }

    public virtual void Initialize()
    {
        Children.ForEach(child => child.Initialize());
    }

    public virtual void LoadContent(ContentManager contentManager)
    {
        Children.ForEach(child => child.LoadContent(contentManager));
    }

    public virtual void UnloadContent()
    {
        Children.ForEach(child => child.UnloadContent());
    }

    public virtual void Draw(RenderContext renderContext, GameTime gameTime, float delta)
    {
        if (IsVisible)
        {
            Children.ForEach(child => child.Draw(renderContext, gameTime, delta));
        }
    }

    public virtual void Update(GameTime gameTime, float delta)
    {
        if (IsActive)
        {
            Children.ForEach(child => child.Update(gameTime, delta));
        }
    }
}