using System.ComponentModel.DataAnnotations;

namespace Neodenit.ActiveReader.Common.Attributes
{
    public class MaxChoicesValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var maxChoices = (int)value;

            var minLength = CoreSettings.Default.MaxChoicesMinOption;
            var maxLength = CoreSettings.Default.MaxChoicesMaxOption;

            var isValid = minLength <= maxChoices && maxChoices <= maxLength;
            return isValid;
        }
    }
}
