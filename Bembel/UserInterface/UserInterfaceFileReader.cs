using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using Bembel.Data;

namespace Bembel.UserInterface;

public class UserInterfaceFileReader
{
    private XmlDocument _document;
    private Dictionary<string, Action<UserInterfaceNode>> _clickActions;
    private Dictionary<string, Binding<object>> _textBindings;

    public UserInterfaceFileReader()
    {
        _document = new XmlDocument();
        _clickActions = new Dictionary<string, Action<UserInterfaceNode>>();
        _textBindings = new();
    }

    public UserInterfaceNode ReadFromFile(string path)
    {
        _document.Load(path);
        var rootNode = _document.DocumentElement;
        return ParseNode(rootNode);
    }

    public void AddClickAction(string functionName, Action<UserInterfaceNode> action)
    {
        _clickActions[functionName] = action;
    }
    
    public void AddTextBinding(string bindingName, Binding<object> binding)
    {
        _textBindings[bindingName] = binding;
    }

    private UserInterfaceNode ParseNode(XmlNode node)
    {
        UserInterfaceNode userInterfaceNode = node.Name switch
        {
            "HStack" => ParseHStack(node),
            "VStack" => ParseVStack(node),
            "Label" => ParseLabel(node),
            "Spacer" => new Spacer(),
            _ => throw new ArgumentException($"Unknown node type: {node.Name}")
        };
        ReadGeneralAttributes(ref userInterfaceNode, node);
        return userInterfaceNode;
    }

    private void ReadGeneralAttributes(ref UserInterfaceNode? userInterfaceNode, XmlNode node)
    {
        foreach (XmlAttribute attribute in node.Attributes)
        {
            if (attribute.Name == "Id" || attribute.Name == "Identification")
            {
                var idValue = attribute.Value;
                userInterfaceNode.SetId(idValue);
            }
        }
    }


    private HStack ParseHStack(XmlNode node)
    {
        var stack = new HStack();

        ReadStackAttributes<HStack>(ref stack, node);
        
        foreach (XmlNode childNode in node.ChildNodes)
        {
            var child = ParseNode(childNode);
            stack.Add(child);
        }

        return stack;
    }

    private VStack ParseVStack(XmlNode node)
    {
        var stack = new VStack();

        ReadStackAttributes<VStack>(ref stack, node);
        
        foreach (XmlNode childNode in node.ChildNodes)
        {
            var child = ParseNode(childNode);
            stack.Add(child);
        }

        return stack;
    }

    private void ReadStackAttributes<T>(ref T stack, XmlNode node) where T : Stack
    {
        foreach (XmlAttribute attribute in node.Attributes)
        {
            if (attribute.Name == "Padding")
            {
                var paddingValue = attribute.Value;
                var paddingValues = Array.ConvertAll(paddingValue.Split(" "), int.Parse);
                stack.SetPadding(paddingValues);
            }
            if (attribute.Name == "Spacing")
            {
                var spacingValue = attribute.Value;
                stack.SetSpacing(int.Parse(spacingValue));
            }
            if (attribute.Name == "Alignment")
            {
                var alignmentValue = attribute.Value;
                switch (alignmentValue)
                {
                    case "Top":
                        stack.SetAlignment(Alignment.Top);
                        break;
                    case "Right":
                        stack.SetAlignment(Alignment.Right);
                        break;
                    case "Bottom":
                        stack.SetAlignment(Alignment.Bottom);
                        break;
                    case "Left":
                        stack.SetAlignment(Alignment.Left);
                        break;
                    case "Center":
                        stack.SetAlignment(Alignment.Center);
                        break;
                }
            }
        }
    }

    public void PrintAllMethods()
    {
        MethodInfo[] methods =
            GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (MethodInfo method in methods)
        {
            Console.WriteLine(method.Name);
        }
    }
    
    private Label ParseLabel(XmlNode node)
    {
        var label = new Label();
        var hasInnerText = false;
        
        if (!string.IsNullOrWhiteSpace(node.InnerText))
        {
            hasInnerText = true;
            var textValue = node.InnerText;
            label.Text = textValue;
        }
        
        foreach (XmlAttribute attribute in node.Attributes)
        {
            if (attribute.Name == "Text")
            {
                if (hasInnerText)
                {
                    continue;
                }
                var textValue = attribute.Value;
                if (!string.IsNullOrWhiteSpace(textValue))
                {
                    if (textValue.StartsWith($"@"))
                    {
                        var bindingKey = textValue[1..];
                        bindingKey = bindingKey.ToLower();
                        if (_textBindings.TryGetValue(bindingKey, out var value))
                        {
                            label.SetTextBinding(value);
                        }
                        else
                        {
                            throw new Exception("binding not found");
                        }
                    }
                    else
                    {
                        label.Text = textValue;
                    }
                }
            }
            if (attribute.Name == "OnClick")
            {
                var functionName = attribute.Value;
                if (_clickActions.TryGetValue(functionName, out var clickAction))
                {
                    label.OnClick(clickAction);
                }
                else
                {
                    Console.WriteLine(
                        $"Warning: The method '{functionName}' for the label '{label.Text}' was not found.");
                }

                var method = GetType().GetMethod(functionName, BindingFlags.NonPublic | BindingFlags.Instance);
                if (method != null)
                {
                    var action = (Action<UserInterfaceNode>)Delegate.CreateDelegate(typeof(Action), this, method);
                    label.OnClick(action);
                }
                else
                {
                    Console.WriteLine(
                        $"Warning: The method '{functionName}' for the label '{label.Text}' was not found.");
                }
            }
            if (attribute.Name == "OnEnter")
            {
                var functionName = attribute.Value;
                if (_clickActions.ContainsKey(functionName))
                {
                    label.OnEnter(_clickActions[functionName]);
                }
                else
                {
                    Console.WriteLine(
                        $"Warning: The method '{functionName}' for the label '{label.Text}' was not found.");
                }

                var method = GetType().GetMethod(functionName, BindingFlags.NonPublic | BindingFlags.Instance);
                if (method != null)
                {
                    var action = (Action<UserInterfaceNode>)Delegate.CreateDelegate(typeof(Action), this, method);
                    label.OnEnter(action);
                }
                else
                {
                    Console.WriteLine(
                        $"Warning: The method '{functionName}' for the label '{label.Text}' was not found.");
                }
            }
            if (attribute.Name == "OnLeave")
            {
                var functionName = attribute.Value;
                if (_clickActions.ContainsKey(functionName))
                {
                    label.OnLeave(_clickActions[functionName]);
                }
                else
                {
                    Console.WriteLine(
                        $"Warning: The method '{functionName}' for the label '{label.Text}' was not found.");
                }

                var method = GetType().GetMethod(functionName, BindingFlags.NonPublic | BindingFlags.Instance);
                if (method != null)
                {
                    var action = (Action<UserInterfaceNode>)Delegate.CreateDelegate(typeof(Action), this, method);
                    label.OnLeave(action);
                }
                else
                {
                    Console.WriteLine(
                        $"Warning: The method '{functionName}' for the label '{label.Text}' was not found.");
                }
            }
        }

        return label;
    }
}