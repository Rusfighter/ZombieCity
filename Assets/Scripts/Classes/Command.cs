public interface Command<T>
{
    void Execute(T t);
}
