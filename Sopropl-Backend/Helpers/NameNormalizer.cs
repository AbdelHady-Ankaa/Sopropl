namespace Sopropl_Backend.Helpers
{
    public class NameNormalizer : INormalizer<string>
    {
        public string Normalize(string value)
        {
            return value.ToLower();
        }
    }
}