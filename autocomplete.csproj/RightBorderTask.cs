using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocomplete
{
    public class RightBorderTask
    {
        /// <returns>
        /// Возвращает индекс правой границы. 
        /// То есть индекс минимального элемента, который не начинается с prefix и большего prefix.
        /// Если такого нет, то возвращает items.Length
        /// </returns>
        /// <remarks>
        /// Функция должна быть НЕ рекурсивной
        /// и работать за O(log(items.Length)*L), где L — ограничение сверху на длину фразы
        /// </remarks>
        public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
        {
            // IReadOnlyList похож на List, но у него нет методов модификации списка.

            if (prefix == "" || phrases.Count == 0)
                return phrases.Count;

            while (left < right - 1)
            {
                var middle = left + (right - left) / 2;
                if (string.Compare(prefix, phrases[middle], StringComparison.OrdinalIgnoreCase) <= 0
                    && !phrases[middle].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    right = middle;
                else left = middle;
            }

            if (right < phrases.Count
                && string.Compare(prefix, phrases[right], StringComparison.OrdinalIgnoreCase) <= 0
                && !phrases[right].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return right;

            return phrases.Count;
        }
    }
}