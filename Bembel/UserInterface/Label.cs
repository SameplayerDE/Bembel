using Microsoft.Xna.Framework;

namespace Bembel.UserInterface;

public class Label : UserInterfaceNode
{
    public Action<Label>? Click { get; private set; }
    public Action<Label>? Enter { get; private set; }
    public Action<Label>? Leave { get; private set; }
    public string Font;
    public string Text;
    public Color Tint = Color.White;

    public Label(object? text = null, string font = "default") : base(UserInterfaceNodeType.Label)
    {
        Font = font;
        Text = Convert.ToString(text) ?? "";
    }
    
    public Label OnClick(Action<Label> action)
    {
        Click = action;
        return this;
    }
    
    public Label OnEnter(Action<Label> action)
    {
        Enter = action;
        return this;
    }
    
    public Label OnLeave(Action<Label> action)
    {
        Leave = action;
        return this;
    }
}