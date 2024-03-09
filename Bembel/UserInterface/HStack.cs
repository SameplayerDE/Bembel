namespace Bembel.UserInterface;

public class HStack : UserInterfaceNode
{
    public List<UserInterfaceNode> Children;

    public float Spacing { get; private set; } = 20;
    public float PaddingLeft { get; private set; }
    public float PaddingTop { get; private set; }
    public float PaddingRight { get; private set; }
    public float PaddingBottom { get; private set; }
    
    public HStack() : base(UserInterfaceNodeType.HStack)
    {
        Children = new List<UserInterfaceNode>();
    }

    public void Add(UserInterfaceNode node)
    {
        Children.Add(node);
    }
    
    public HStack SetSpacing(float value)
    {
        Spacing = value;
        return this;
    }
    
    public HStack SetPadding(float value)
    {
        SetPaddingLeft(value);
        SetPaddingTop(value);
        SetPaddingRight(value);
        SetPaddingBottom(value);
        return this;
    }
    
    public HStack SetPaddingLeft(float value)
    {
        PaddingLeft = value;
        return this;
    }

    public HStack SetPaddingTop(float value)
    {
        PaddingTop = value;
        return this;
    }

    public HStack SetPaddingRight(float value)
    {
        PaddingRight = value;
        return this;
    }

    public HStack SetPaddingBottom(float value)
    {
        PaddingBottom = value;
        return this;
    }
}