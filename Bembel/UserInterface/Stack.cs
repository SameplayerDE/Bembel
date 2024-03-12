namespace Bembel.UserInterface;

public enum Alignment
{
    Left,
    Top,
    Right,
    Bottom,
    Center
}

public abstract class Stack : UserInterfaceNode
{
    public List<UserInterfaceNode> Children;

    public Alignment Alignment { get; protected set; }
    public float Spacing { get; protected set; }
    public float PaddingLeft { get; protected set; }
    public float PaddingTop { get; protected set; }
    public float PaddingRight { get; protected set; }
    public float PaddingBottom { get; protected set; }

    protected Stack(UserInterfaceNodeType type) : base(type)
    {
        Children = new List<UserInterfaceNode>();
    }
    
    public void Add(UserInterfaceNode node)
    {
        Children.Add(node);
    }
    
    public Stack SetAlignment(Alignment value)
    {
        Alignment = value;
        return this;
    }
    
    public Stack SetSpacing(int value)
    {
        Spacing = value;
        return this;
    }
    
    public Stack SetPadding(int value)
    {
        SetPaddingTop(value);
        SetPaddingRight(value);
        SetPaddingBottom(value);
        SetPaddingLeft(value);
        return this;
    }
    
    public Stack SetPadding(int[] values)
    {
        if (values.Length != 4)
        {
            switch (values.Length)
            {
                case 1:
                    SetPadding(values[0]);
                    break;
                case 2:
                    SetPaddingTop(values[0]);
                    SetPaddingRight(values[1]);
                    SetPaddingBottom(values[0]);
                    SetPaddingLeft(values[1]);
                    break;
            }
            return this;
        }
        SetPaddingTop(values[0]);
        SetPaddingRight(values[1]);
        SetPaddingBottom(values[2]);
        SetPaddingLeft(values[3]);
        return this;
    }
    
    public Stack SetPaddingLeft(float value)
    {
        PaddingLeft = value;
        return this;
    }

    public Stack SetPaddingTop(float value)
    {
        PaddingTop = value;
        return this;
    }

    public Stack SetPaddingRight(float value)
    {
        PaddingRight = value;
        return this;
    }

    public Stack SetPaddingBottom(float value)
    {
        PaddingBottom = value;
        return this;
    }
}