﻿// Copyright 2019 OrbintSoft - Stefano Balzarotti
// Copyright 2017 LOSTALLOY
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace LOSTALLOY.LocalHistory
{
    using System;
    using System.Text;
    using JetBrains.Annotations;
    using Pri.LongPath;

    /// <summary>
    /// This class adds some string extensions methods for string manipulation.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// This method implements a stringreplace with a StringComparison parameter.
        /// </summary>
        /// <param name="str">The source string.</param>
        /// <param name="oldValue">The value to be replaced.</param>
        /// <param name="newValue">The new value to be used as replacement.</param>
        /// <param name="comparison">The comparisation option.</param>
        /// <returns>The string with replaced value.</returns>
        [NotNull]
        public static string Replace([NotNull] this string str, [NotNull] string oldValue, [NotNull] string newValue, StringComparison comparison)
        {
            if (str is null)
            {
                throw new ArgumentNullException(str);
            }

            if (oldValue is null)
            {
                throw new ArgumentNullException(oldValue);
            }

            if (newValue is null)
            {
                throw new ArgumentNullException(newValue);
            }

            var sb = new StringBuilder();
            var previousIndex = 0;
            var index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }

            sb.Append(str.Substring(previousIndex));
            return sb.ToString();
        }

        /// <summary>
        ///     Returns true if <paramref name="path" /> starts with the path <paramref name="baseDirPath" />.
        ///     The comparison is case-insensitive, handles / and \ slashes as folder separators and
        ///     only matches if the base dir folder name is matched exactly ("c:\foobar\file.txt" is not a sub path of "c:\foo").
        /// </summary>
        public static bool IsSubPathOf(this string path, string baseDirPath) 
        {
            var normalizedPath = Utils.NormalizePath(path);
            var normalizedBaseDirPath = Path.GetFullPath(Utils.NormalizePath(baseDirPath));
            return normalizedPath.StartsWith(normalizedBaseDirPath, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Returns <paramref name="str" /> with the minimal concatenation of <paramref name="ending" /> (starting from end)
        ///     that
        ///     results in satisfying .EndsWith(ending).
        /// </summary>
        /// <example>"hel".WithEnding("llo") returns "hello", which is the result of "hel" + "lo".</example>
        /// <returns>The resulting string.</returns>
        [NotNull]
        public static string WithEnding([CanBeNull] this string str, [NotNull] string ending) {
            if (str == null)
            {
                return ending;
            }

            var result = str;

            // Right() is 1-indexed, so include these cases
            // * Append no characters
            // * Append up to N characters, where N is ending length
            for (var i = 0; i <= ending.Length; i++) {
                var tmp = result + ending.Right(i);
                if (tmp.EndsWith(ending, StringComparison.Ordinal)) {
                    return tmp;
                }
            }

            return result;
        }

        /// <summary>Gets the rightmost <paramref name="length" /> characters from a string.</summary>
        /// <param name="value">The string to retrieve the substring from.</param>
        /// <param name="length">The number of characters to retrieve.</param>
        /// <returns>The substring.</returns>
        public static string Right([NotNull] this string value, int length) {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (length < 0)
            {
                // ReSharper disable once LocalizableElement
                throw new ArgumentOutOfRangeException(nameof(length), length, "Length is less than zero");
            }

            return length < value.Length ? value.Substring(value.Length - length) : value;
        }
    }
}
