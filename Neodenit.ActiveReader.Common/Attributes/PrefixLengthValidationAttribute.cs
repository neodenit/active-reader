using System.ComponentModel.DataAnnotations;

namespace Neodenit.ActiveReader.Common.Attributes
{
    public class PrefixLengthValidationAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var prefixLength = (int)value;

            var minLength = CoreSettings.Default.PrefixLengthMinOption;
            var maxLength = CoreSettings.Default.PrefixLengthMaxOption;

            var isValid = minLength <= prefixLength && prefixLength <= maxLength;
            return isValid;
        }
    }
}
