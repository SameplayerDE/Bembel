namespace Bembel.UserInterface;

public class UserInterfaceNode
{
    public static List<string> Registered = new();
    
    public readonly UserInterfaceNodeType Type;
    public float X;
    public float Y;
    public float Width;
    public float Height;
    public string Id;
    
    protected UserInterfaceNode(UserInterfaceNodeType type)
    {
        Type = type;
    }

    public virtual UserInterfaceNode SetId(string id)
    {
        if (Registered.Contains(id))
        {
            throw new Exception("tried to assign an id that has already been assigned");
        }
        Registered.Add(id);
        Id = id;
        return this;
    }
}