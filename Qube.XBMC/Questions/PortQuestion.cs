using System.Collections.Generic;
using HyperQube.Library;
using HyperQube.Library.Questions;

namespace Qube.XBMC.Questions
{
    public class PortQuestion : TextQuestion, IValidatableQuestion
    {
        public PortQuestion(string initialValue = "")
            : base("Port", initialValue: initialValue)
        {
            var validation = new Validation(x => Regex.Port.IsMatch((string)x), ValidationMessages.InvalidPort);
            Validations = new[] {validation};
        }

        public IEnumerable<IValidation> Validations { get; private set; }
    }
}
