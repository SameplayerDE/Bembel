using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bembel;

public class Scene
{
    public string SceneName { get; private set; }
    protected ContentManager Content;
    protected GraphicsDevice GraphicsDevice;
    public Game Game;
    public List<SceneObject> SceneObjects { get; private set; }

    public Scene(string name, Game game)
    {
        SceneName = name;
        Game = game;
        GraphicsDevice = game.GraphicsDevice;
        SceneObjects = new List<SceneObject>();
        Content = new ContentManager(Game.Services, "Content");
    }

    protected void Exit()
    {
        Game.Exit();
    }
    
    public override bool Equals(object? obj)
    {
        return obj is Scene scene && SceneName.Equals(scene.SceneName);
    }

    public T AddSceneObject<T>(T sceneObject) where T : SceneObject
    {
        if (!SceneObjects.Contains(sceneObject))
        {
            sceneObject.Scene = this;
            SceneObjects.Add(sceneObject);
            return sceneObject;
        }
        return null;
    }

    public void RemoveSceneObject(SceneObject sceneObject)
    {
        if (SceneObjects.Remove(sceneObject))
        {
            sceneObject.Scene = null;
        }
    }

    public virtual void Activated(){}
    public virtual void Deactivated(){}

    public virtual void Initialize()
    {
        SceneObjects.ForEach(sceneObject => sceneObject.Initialize());
    }

    public virtual void LoadContent()
    {
        SceneObjects.ForEach(sceneObject => sceneObject.LoadContent(Content));
    }
        
    public virtual void UnloadContent()
    {
        SceneObjects.ForEach(sceneObject => sceneObject.UnloadContent());
    }

    public virtual void Draw(RenderContext renderContext, GameTime gameTime, float delta)
    {
        SceneObjects.ForEach(obj => obj.Draw(renderContext, gameTime, delta));
    }
    
    public virtual void Update(GameTime gameTime, float delta)
    {
        SceneObjects.ForEach(sceneObject => sceneObject.Update(gameTime, delta));
    }
}