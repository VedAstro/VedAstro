using System;
using System.Collections.Generic;

namespace VedAstro.Library
{
    /// <summary>
    /// Extension methods for <see cref="System.Collections.Generic.List{T}"/>
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Moves the item matching the <paramref name="itemSelector"/> to the end of the <paramref name="list"/>.
        /// </summary>
        public static void MoveToEnd<T>(this List<T> list, Predicate<T> itemSelector)
        {
            //Ensure.Argument.NotNull(list, "list");
            if (list.Count > 1)
                list.Move(itemSelector, list.Count - 1);
        }

        /// <summary>
        /// Moves the item matching the <paramref name="itemSelector"/> to the beginning of the <paramref name="list"/>.
        /// </summary>
        public static void MoveToBeginning<T>(this List<T> list, Predicate<T> itemSelector)
        {
            //Ensure.Argument.NotNull(list, "list");
            list.Move(itemSelector, 0);
        }

        /// <summary>
        /// Moves the item matching the <paramref name="itemSelector"/> to the <paramref name="newIndex"/> in the <paramref name="list"/>.
        /// </summary>
        public static void Move<T>(this List<T> list, Predicate<T> itemSelector, int newIndex)
        {
            var currentIndex = list.FindIndex(itemSelector);

            //if same return it
            if (currentIndex == newIndex) { return; }

            // Copy the item
            var item = list[currentIndex];

            // Remove the item from the list
            list.RemoveAt(currentIndex);

            // Finally insert the item at the new index
            list.Insert(newIndex, item);
        }
    }
}
