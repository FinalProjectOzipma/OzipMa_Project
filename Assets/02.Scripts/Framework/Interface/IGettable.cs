public interface IGettable
{
    public T GetClassAddress<T>() where T : UserObject;
}
