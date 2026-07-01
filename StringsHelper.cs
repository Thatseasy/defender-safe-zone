using System.Globalization;
using System.Resources;
using System.Threading;
using System.Reflection;

namespace DefenderSafeZoneTool
{
    public static class Strings
    {
        private static readonly ResourceManager ResourceManager;

        static Strings()
        {
            ResourceManager = new ResourceManager("DefenderSafeZoneTool.Resources.Strings", typeof(Strings).Assembly);
        }

        public static string Get(string key)
        {
            return ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? $"[{key}]";
        }
        
        public static string Get(string key, params object[] args)
        {
            var str = Get(key);
            return string.Format(str, args);
        }
    }
}
