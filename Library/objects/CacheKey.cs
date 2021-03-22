using System;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// Uniquely identifies a specific call to the method
    /// It holds the method name and the params used to call the method (only hashes, for performance)
    /// Note: Use class over struct for performance
    /// </summary>
    [Serializable()]
    public class CacheKey
    {
        public string Function;
        //public object[] Args;
        private int ultimateHash;

        //CTOR
        public CacheKey(string function, params object[] args)
        {
            Function = function;
            //Args = args;

            //get hashes of all values
            var functionNameHash = function.GetHashCode();
            var allArgumentsHash = GetHashCodeForArray(args);

            //combine them together
            ultimateHash = functionNameHash + allArgumentsHash;
        }


        //PUBLIC METHODS
        public override bool Equals(object value)
        {
            if (value.GetType() == typeof(CacheKey))
            {
                //cast to cache key
                var possibleMatch = (CacheKey)value;

                //Check equality
                bool returnValue = (this.GetHashCode() == possibleMatch.GetHashCode());

                return returnValue;
            }
            else
            {
                //Return false if value is null
                return false;
            }
        }

        public override int GetHashCode() => ultimateHash;
        //{
        //    //get hash of all the fields & combine them
        //    var hash1 = Function.GetHashCode();

        //    //get the hash for each param in args & add it together
        //    var hash2 = 0;
        //    foreach (var arg in Args)
        //    {
        //        hash2 += arg.GetHashCode();
        //    }

        //    return hash1 + hash2;
        //}


        //PRIVARE METHODS

        /// <summary>
        /// Gets the hash code for the contents of the array since the default hash code
        /// for an array is unique even if the contents are the same.
        /// </summary>
        /// <remarks>
        /// See Jon Skeet (C# MVP) response in the StackOverflow thread 
        /// http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
        /// </remarks>
        /// <param name="array">The array to generate a hash code for.</param>
        /// <returns>The hash code for the values in the array.</returns>
        private int GetHashCodeForArray(object[] array)
        {
            // if non-null array then go into unchecked block to avoid overflow
            if (array != null)
            {
                unchecked
                {
                    int hash = 17;

                    // get hash code for all items in array
                    foreach (var item in array)
                    {
                        hash = hash * 23 + ((item != null) ? item.GetHashCode() : 0);
                    }

                    return hash;
                }
            }

            // if null, hash code is zero
            return 0;
        }
    }
}