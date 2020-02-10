using System.ComponentModel.DataAnnotations;

namespace Neodenit.ActiveReader.Common
{
    public class PrefixLengthValidationAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var prefixLength = (int)value;

            var minPrefixLength = 1;
            var maxPrefixLength = CoreSettings.Default.PrefixLength;

            var isValid = minPrefixLength <= prefixLength && prefixLength <= maxPrefixLength;
            return isValid;
        }
    }
}
