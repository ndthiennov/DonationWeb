using System.Globalization;
using System.Text;

namespace DonationAppDemo.Helper
{
    public class StringExtension
    {
        public static string? NormalizeString(string? input)
        {
            if(input == null)
            {
                return null;
            }
            // Convert to lowercase
            string lowerCase = input.ToLower();

            // Remove diacritics
            string normalized = lowerCase.Normalize(NormalizationForm.FormD);
            string withoutDiacritics = new string(normalized
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());

            // Remove space
            string result = withoutDiacritics.Replace(" ", "");
            return result;
        }
    }
}
