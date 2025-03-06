// Copyright (c) 2025 Adita.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Text;

namespace Adita.PlexNet.Opc.Ua.Internal.Extensions
{
    internal static class StringExtensions
    {
        #region Public methods
        public static string ToPascalCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            StringBuilder pascalCase = new StringBuilder();
            TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
            bool newWord = true;

            foreach (char c in value)
            {
                if (c == '_' || c == '-')
                {
                    newWord = true;
                    continue;
                }

                if (newWord)
                {
                    pascalCase.Append(textInfo.ToUpper(c));
                    newWord = false;
                }
                else
                {
                    pascalCase.Append(c);
                }
            }

            return pascalCase.ToString();
        }
        #endregion Public methods
    }
}
