using Microsoft.AspNetCore.Http;

namespace Sopropl_Backend.Helpers
{
    public static class Extentions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }

    public static class @short
    {
        public const short OTHER = 0;
        public const short MEMBER = 1;
        public const short OWNER = 2;

    }
    public static class AccessType
    {
        public const short OTHER = 0;
        public const short CONTRIBUTOR = 1;
        public const short MANAGER = 2;
    }
}