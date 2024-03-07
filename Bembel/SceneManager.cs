using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bembel;

public class SceneManager
{
        
    public Dictionary<string, Scene> Resources = new Dictionary<string, Scene>();
        
    public static SceneManager Instance { get; } = new SceneManager();
    public RenderContext RenderContext;
        
    public Scene? Previous;
    public Scene? Current;
    public Scene? Next;
        
    public string PreviousKey;
    public string CurrentKey;
    public string NextKey;

    static SceneManager()
    {
    }
        
    private SceneManager()
    {
        RenderContext = new RenderContext();
    }

    public void Initialize()
    {
        foreach (var scene in Resources.Values)
        {
            scene.Initialize();
        }
    }
        
    public bool Has(string name)
    {
        return Resources.ContainsKey(name);
    }
        
    public Scene Find(string name)
    {
        if (Has(name))
        {
            return Resources[name];
        }
        throw new NullReferenceException();
    }

    public void Add(Scene value, bool overwrite = false)
    {
        if (overwrite)
        {
            Resources[value.SceneName] = value;
        }
        else
        {
            Resources.Add(value.SceneName, value);
        }
    }
        
    public void Grab()
    {
        if (Next == null) return;
        Previous?.UnloadContent();
        if (Current != null)
        {
            Previous = Current;
            PreviousKey = CurrentKey;
        }

        CurrentKey = NextKey;
        Current = Next;
        Next = null;
        NextKey = string.Empty;
    }
        
    public void Stage(string key)
    {
        if (!Has(key)) return;
        if (NextKey == key) return;
        if (CurrentKey == key) return;
        var value = Find(key);
        if (PreviousKey != key)
        {
            Next?.UnloadContent();
            NextKey = key;
            Next = value;
            Next.LoadContent();
        }
        else
        {
            NextKey = PreviousKey;
            Next = Previous;
            Previous = null;
            PreviousKey = string.Empty;
        }
    }
        
    public void UnStage(string key)
    {
        if (!Has(key)) return;
        var value = Find(key);
        Previous = value;
        Previous.UnloadContent();
    }
        
    public void Load(string key)
    {
        if (!Has(key)) return;
        var value = Find(key);
        value.LoadContent();
    }
        
    public void UnLoad(string key)
    {
        if (!Has(key)) return;
        var value = Find(key);
        value.UnloadContent();
    }

    public void Update(GameTime gameTime, float delta)
    {
        Current?.Update(gameTime, delta);
    }

    public void Draw(GameTime gameTime, float delta)
    {
        if (Current == null)
            return;
            
        RenderContext.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        Current.Draw(RenderContext, gameTime, delta);
        RenderContext.SpriteBatch.End();
    }
        
}