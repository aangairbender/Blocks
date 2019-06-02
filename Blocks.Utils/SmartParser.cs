using System.Globalization;

namespace Blocks.Utils
{
    public static class SmartParser
    {
        public static bool ToFloat(this string str, out float res)
        {
            return float.TryParse(str, out res);
        }

        public static bool ToInt(this string str, out int res)
        {
            return int.TryParse(str, out res);
        }
    }
}