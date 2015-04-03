using System;
using System.Collections.Generic;
using HyperQube.Library;
using HyperQube.Library.Questions;

namespace Qube.XBMC.Questions
{
    public class UriQuestion : TextQuestion, IValidatableQuestion
    {
        public UriQuestion(string title = "Uri", string initialValue = "")
            : base(title, initialValue: initialValue)
        {
            var validation = new Validation(x => Uri.IsWellFormedUriString((string)x, UriKind.Absolute), ValidationMessages.InvalidUri);
            Validations = new[] {validation};
        }

        public IEnumerable<IValidation> Validations { get; private set; }
    }
}
