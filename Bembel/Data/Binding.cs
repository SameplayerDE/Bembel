namespace Bembel.Data;

public class Binding<T>(T initialValue)
{
    public T Value
    {
        get => initialValue;
        set
        {
            if (Equals(initialValue, value)) return;
            initialValue = value;
            ValueChanged?.Invoke(value);
        }
    }

    public event Action<T>? ValueChanged;
}