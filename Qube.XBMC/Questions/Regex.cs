using System.Text.RegularExpressions;

namespace Qube.XBMC.Questions
{
    static class Regex
    {
        public static readonly System.Text.RegularExpressions.Regex Ip   = Create(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$");
        public static readonly System.Text.RegularExpressions.Regex Port = Create(@"^([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$");

        private static System.Text.RegularExpressions.Regex Create(string regex, RegexOptions options = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase)
        {
            return new System.Text.RegularExpressions.Regex(regex, options);
        }
    }
}
