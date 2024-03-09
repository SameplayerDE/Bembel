namespace Bembel.UserInterface;

public class Label : UserInterfaceNode
{
    public Action? Action { get; private set; }
    public string Font;
    public string Text;

    public Label(object? text = null, string font = "default") : base(UserInterfaceNodeType.Label)
    {
        Font = font;
        Text = Convert.ToString(text) ?? "";
    }
    
    public Label OnClick(Action action)
    {
        Action = action;
        return this;
    }

    public void Invoke()
    {
        Action?.Invoke();
    }
}