using System.ComponentModel.DataAnnotations;

namespace Neodenit.ActiveReader.Common.Attributes
{
    public class PrefixLengthValidationAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var prefixLength = (int)value;

            var minPrefixLength = CoreSettings.Default.PrefixLengthMinOption;
            var maxPrefixLength = CoreSettings.Default.PrefixLengthMaxOption;

            var isValid = minPrefixLength <= prefixLength && prefixLength <= maxPrefixLength;
            return isValid;
        }
    }
}
