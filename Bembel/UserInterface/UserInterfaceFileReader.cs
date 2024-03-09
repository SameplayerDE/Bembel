using System.Reflection;
using System.Xml;

namespace Bembel.UserInterface;

public class UserInterfaceFileReader
{
    private XmlDocument _document;
    private Dictionary<string, Action> _clickActions;

    public UserInterfaceFileReader()
    {
        _document = new XmlDocument();
        _clickActions = new Dictionary<string, Action>();
    }

    public UserInterfaceNode ReadFromFile(string path)
    {
        _document.Load(path);
        var rootNode = _document.DocumentElement;
        return ParseNode(rootNode);
    }

    public void AddClickAction(string functionName, Action action)
    {
        _clickActions[functionName] = action;
    }

    private UserInterfaceNode ParseNode(XmlNode node)
    {
        UserInterfaceNode userInterfaceNode = node.Name switch
        {
            "HStack" => ParseHStack(node),
            "VStack" => ParseVStack(node),
            "Label" => ParseLabel(node),
            _ => throw new ArgumentException($"Unknown node type: {node.Name}")
        };

        return userInterfaceNode;
    }


    private HStack ParseHStack(XmlNode node)
    {
        var stack = new HStack();

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

        foreach (XmlNode childNode in node.ChildNodes)
        {
            var child = ParseNode(childNode);
            stack.Add(child);
        }

        return stack;
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
        var label = new Label
        {
            Text = node.InnerText.Trim()
        };
        foreach (XmlAttribute attribute in node.Attributes)
        {
            if (attribute.Name == "OnClick")
            {
                var functionName = attribute.Value;
                if (_clickActions.ContainsKey(functionName))
                {
                    label.OnClick(_clickActions[functionName]);
                }
                else
                {
                    Console.WriteLine(
                        $"Warning: The method '{functionName}' for the label '{label.Text}' was not found.");
                }

                var method = GetType().GetMethod(functionName, BindingFlags.NonPublic | BindingFlags.Instance);
                if (method != null)
                {
                    var action = (Action)Delegate.CreateDelegate(typeof(Action), this, method);
                    label.OnClick(action);
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