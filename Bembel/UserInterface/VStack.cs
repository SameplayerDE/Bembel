namespace Bembel.UserInterface;

public class VStack : Stack
{
    public VStack() : base(UserInterfaceNodeType.VStack)
    {
        Alignment = Alignment.Left;
    }
    
    public override VStack SetId(string id)
    {
        return (VStack)base.SetId(id);
    }
    
}