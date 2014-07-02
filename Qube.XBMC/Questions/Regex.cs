#region License

//  Copyright 2014 Steven Thuriot
//   
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  
//  http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

#endregion

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
