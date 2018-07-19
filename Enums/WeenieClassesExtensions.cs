using System;

namespace PhatACCacheBinParser.Enums
{
    static class WeenieClassesExtensions
    {
        public static string GetNameFormattedForDatabase(this WeenieClasses weenieClass)
        {
            if (!Enum.IsDefined(typeof(WeenieClasses), weenieClass))
                return "ace" + weenieClass;

            var result = weenieClass.ToString();

            // Remove the W_ at the front
            if (result.StartsWith("W_"))
                result = result.Substring(2);

            // Remove the trailing _CLASS
            if (result.EndsWith("_CLASS"))
                result = result.Substring(0, result.Length - 6);

            result = result.Replace("_", "-");

            result = result.ToLower();

            return result;
        }
    }
}
