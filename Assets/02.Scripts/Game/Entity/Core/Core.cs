public class Core
{
    public EntityHealth Health = new();
    public IntegerBase CoreLevel = new();

    public Core()
    {
        Health.MaxValue = 100.0f;
        Health.SetValue(100.0f);
        CoreLevel.SetValue(1);
    }
}
