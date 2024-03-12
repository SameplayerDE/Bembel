using System.Reflection.Metadata;
using Bembel.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bembel.UserInterface;

public class UserInterfaceRenderer
{

    public Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
    
    public UserInterfaceRenderer()
    {
    }
    
    public void CalculateLayout(UserInterfaceNode? node)
    {
        if (node == null)
        {
            return;
        }

        switch (node.Type)
        {
            case UserInterfaceNodeType.HStack:
                CalculateHStackLayout((HStack)node);
                break;
            case UserInterfaceNodeType.VStack:
                CalculateVStackLayout((VStack)node);
                break;
            case UserInterfaceNodeType.Label:
                CalculateLabelLayout((Label)node);
                break;
            case UserInterfaceNodeType.Spacer:
                CalculateSpacerLayout((Spacer)node);
                break;
            default:
                throw new ArgumentException("Unknown UI node type.");
        }
    }

    private void CalculateSpacerLayout(Spacer node)
    {
        
    }

    private void CalculateLabelLayout(Label node)
    {
        var label = (Label)node;
        var text = "";
        text = Convert.ToString(label.Text) ?? "";
        var dimensions = Fonts[label.Font].MeasureString(text);
        label.Height = dimensions.Y;
        label.Width = dimensions.X;
    }
    
    private void CalculateVStackLayout(VStack stack)
    {
        var currentY = stack.Y + stack.PaddingTop;
        var currentX = stack.X + stack.PaddingLeft;
        var maxWidth = 0f;
        var totalHeight = stack.PaddingTop + stack.PaddingBottom;

        for (var index = 0; index < stack.Children.Count; index++)
        {
            var child = stack.Children[index];
                
            child.X = currentX;
            child.Y = currentY;

            CalculateLayout(child);

            currentY += child.Height + stack.Spacing;
            totalHeight += child.Height;
            if (index != stack.Children.Count - 1)
            {
                totalHeight += stack.Spacing;
            }
                
            if (child.Width > maxWidth)
            {
                maxWidth = child.Width;
            }
        }

        stack.Width = maxWidth + stack.PaddingLeft + stack.PaddingRight;
        stack.Height = totalHeight;
        
        foreach (var child in stack.Children)
        {
            if (stack.Alignment == Alignment.Center)
            {
                if (child.Width < maxWidth)
                {
                    child.X = stack.X + stack.Width / 2 - child.Width / 2;
                    CalculateLayout(child);
                }
            }
            if (stack.Alignment == Alignment.Right)
            {
                if (child.Width < maxWidth)
                {
                    child.X = stack.X + stack.Width - child.Width;
                    CalculateLayout(child);
                }
            }
        }
        
    }
    
    private void CalculateHStackLayout(HStack stack)
    {
        var currentY = stack.Y + stack.PaddingTop;
        var currentX = stack.X + stack.PaddingLeft;
        var maxHeight = 0f;
        var totalWidth = stack.PaddingLeft + stack.PaddingRight;

        for (var index = 0; index < stack.Children.Count; index++)
        {
            var child = stack.Children[index];
            
            child.X = currentX;
            child.Y = currentY;

            CalculateLayout(child);

            currentX += child.Width + stack.Spacing;
            totalWidth += child.Width;
            if (index != stack.Children.Count - 1)
            {
                totalWidth += stack.Spacing;
            }

            if (child.Height > maxHeight)
            {
                maxHeight = child.Height;
            }
        }

        stack.Height = maxHeight + stack.PaddingTop + stack.PaddingBottom;
        stack.Width = totalWidth;
        
        foreach (var child in stack.Children)
        {
            if (stack.Alignment == Alignment.Center)
            {
                if (child.Height < maxHeight)
                {
                    child.Y = stack.Y;
                    child.Y += stack.Height / 2;
                    child.Y -= child.Height / 2;
                    CalculateLayout(child);
                }
            }
        }
        
    }
    
    public void Draw(UserInterfaceNode? node, RenderContext context, GameTime gameTime, float delta)
    {
        if (node == null)
        {
            return;
        }
        
        if (node.Type == UserInterfaceNodeType.HStack)
        {
            var stack = (HStack)node;
            foreach (var child in stack.Children)
            {
                Draw(child, context, gameTime, delta);
            }
        }

        
        if (node.Type == UserInterfaceNodeType.VStack)
        {
            var stack = (VStack)node;
            foreach (var child in stack.Children)
            {
                Draw(child, context, gameTime, delta);
            }
        }

        if (node.Type == UserInterfaceNodeType.Label)
        {
            var label = (Label)node;
            context.SpriteBatch.DrawString(Fonts[label.Font], Convert.ToString(label.Text) ?? "", new Vector2(label.X, label.Y), label.Tint);
        }
    }
    
    public void HandleInput(UserInterfaceNode? node, InputHandler inputHandler)
    {
        if (node == null)
        {
            return;
        }
        
        if (node.Type == UserInterfaceNodeType.Label)
        {
            var label = (Label)node;
            var labelRect = new Rectangle((int)label.X, (int)label.Y, (int)label.Width, (int)label.Height);

            if (labelRect.Contains(inputHandler.GetMousePosition()) && inputHandler.IsLeftMousePressed())
            {
                label.Click?.Invoke(label);
            }
            
            if (labelRect.Contains(inputHandler.GetMousePosition()) && !labelRect.Contains(inputHandler.GetPrevMousePosition()))
            {
                label.Enter?.Invoke(label);
            }
            
            if (!labelRect.Contains(inputHandler.GetMousePosition()) && labelRect.Contains(inputHandler.GetPrevMousePosition()))
            {
                label.Leave?.Invoke(label);
            }
        }
        
        switch (node)
        {
            case HStack container:
            {
                foreach (var child in container.Children)
                {
                    HandleInput(child, inputHandler);
                }

                break;
            }
            case VStack container:
            {
                foreach (var child in container.Children)
                {
                    HandleInput(child, inputHandler);
                }

                break;
            }
        }
    }
    
    /*public bool HitTest(UserInterfaceNode? node)
    {
        if (node == null)
        {
            throw new NullReferenceException();
        }
        
        if (!node.IsClickable)
        {
            if (node is UserInterfaceNodeContainer container)
            {
                foreach (var child in container.Children)
                {
                    if (HitTest(child))
                    {
                        var area = new Rectangle((int)child.X, (int)child.Y, (int)child.Width, (int)child.Height);
                        if (area.Contains(Context.Input.GetMousePosition()))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        return true;
    }*/
}