using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GrandadAudioPlayerAdmin.Utils
{
    class AllowedExtensionValidationRule : ValidationRule
    {
        private static readonly Regex rx = new Regex(@"^\.[A-Za-z0-9]+(?:,\.[A-Za-z0-9]+)*$", RegexOptions.Compiled);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new ValidationResult(value is string testValue && rx.IsMatch(testValue), "Does not match comma separated list of extensions ");
        }
    }
}
