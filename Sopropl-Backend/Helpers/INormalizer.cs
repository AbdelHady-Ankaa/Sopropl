namespace Sopropl_Backend.Helpers
{
    public interface INormalizer<T>
    {
        T Normalize(T value);
    }
}