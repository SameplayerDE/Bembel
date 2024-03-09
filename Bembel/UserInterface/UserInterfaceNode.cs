namespace Bembel.UserInterface;

public class UserInterfaceNode
{
    public readonly UserInterfaceNodeType Type;
    public float X;
    public float Y;
    public float Width;
    public float Height;
    
    protected UserInterfaceNode(UserInterfaceNodeType type)
    {
        Type = type;
    }
}