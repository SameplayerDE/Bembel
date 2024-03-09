namespace Bembel.UserInterface;

public class VStack : UserInterfaceNode
{
    public List<UserInterfaceNode> Children;

    public float Spacing { get; private set; } = 20;
    public float PaddingLeft { get; private set; }
    public float PaddingTop { get; private set; }
    public float PaddingRight { get; private set; }
    public float PaddingBottom { get; private set; }
    
    public VStack() : base(UserInterfaceNodeType.VStack)
    {
        Children = new List<UserInterfaceNode>();
    }

    public void Add(UserInterfaceNode node)
    {
        Children.Add(node);
    }
    
    public VStack SetSpacing(float value)
    {
        Spacing = value;
        return this;
    }
    
    public VStack SetPadding(float value)
    {
        SetPaddingLeft(value);
        SetPaddingTop(value);
        SetPaddingRight(value);
        SetPaddingBottom(value);
        return this;
    }
    
    public VStack SetPaddingLeft(float value)
    {
        PaddingLeft = value;
        return this;
    }

    public VStack SetPaddingTop(float value)
    {
        PaddingTop = value;
        return this;
    }

    public VStack SetPaddingRight(float value)
    {
        PaddingRight = value;
        return this;
    }

    public VStack SetPaddingBottom(float value)
    {
        PaddingBottom = value;
        return this;
    }
    
}