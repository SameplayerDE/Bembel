namespace Bembel.UserInterface;

public class HStack : Stack
{
    public HStack() : base(UserInterfaceNodeType.HStack)
    {
        Alignment = Alignment.Top;
    }
    
    public override HStack SetId(string id)
    {
        return (HStack)base.SetId(id);
    }
    
}