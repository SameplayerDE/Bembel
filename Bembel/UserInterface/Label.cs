using Bembel.Data;
using Microsoft.Xna.Framework;

namespace Bembel.UserInterface;

public class Label : UserInterfaceNode
{
    public string Font;
    public string Text;
    public Binding<object>? TextBinding; // Optional, kann null sein
    public Color Tint = Color.White;
    
    public Action<Label>? Click { get; private set; }
    public Action<Label>? Enter { get; private set; }
    public Action<Label>? Leave { get; private set; }


    public Label(object? text = null, string font = "default") : base(UserInterfaceNodeType.Label)
    {
        Font = font;
        Text = Convert.ToString(text) ?? "";
    }

    public Label SetTextBinding(Binding<object> binding)
    {
        TextBinding = binding;
        TextBinding.ValueChanged += OnTextChanged;
        UpdateText();
        return this;
    }

    private void OnTextChanged(object value)
    {
        Text = Convert.ToString(value) ?? "";
    }

    private void UpdateText()
    {
        if (TextBinding != null)
        {
            Text = Convert.ToString(TextBinding.Value) ?? "";
        }
    }
    
    public override Label SetId(string id)
    {
        return (Label)base.SetId(id);
    }

    public Label SetText(string value)
    {
        Text = value;
        return this;
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