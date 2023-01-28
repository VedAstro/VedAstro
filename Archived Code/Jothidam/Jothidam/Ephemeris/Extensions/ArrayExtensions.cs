using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwissEphNet
{
    /// <summary>
    /// Array extensions
    /// </summary>
    public static class ArrayExtensions
    {

        /// <summary>
        /// Make an CPointer from an array
        /// </summary>
        public static CPointer<T> GetPointer<T>(this T[] array, int index = 0) {
            return new CPointer<T>(array, index);
        }

    }
}
