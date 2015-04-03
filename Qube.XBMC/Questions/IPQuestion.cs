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
