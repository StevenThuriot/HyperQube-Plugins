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

using System.Collections.Generic;
using HyperQube.Library;
using HyperQube.Library.Questions;

namespace Qube.XBMC.Questions
{
    public class IpQuestion : TextQuestion, IValidatableQuestion
    {
        public IpQuestion(string initialValue = "")
            : base("IP", initialValue: initialValue)
        {
            var validation = new Validation(x => Regex.Ip.IsMatch((string) x), ValidationMessages.InvalidFormat);
            Validations = new[] {validation};
        }

        public IEnumerable<IValidation> Validations { get; private set; }
    }
}
