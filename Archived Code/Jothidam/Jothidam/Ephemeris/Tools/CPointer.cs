using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwissEphNet
{

    /// <summary>
    /// Simulate a C pointer
    /// </summary>
    public struct CPointer<T>
    {
        T[] BaseArray;

        int BaseIndex;

        /// <summary>
        /// Create new struct
        /// </summary>
        public CPointer(T[] baseArray)
            : this(baseArray, 0) {
        }

        /// <summary>
        /// Create new struct
        /// </summary>
        public CPointer(T[] baseArray, int baseIndex)
            : this() {
            this.BaseArray = baseArray;
            this.BaseIndex = baseIndex;
        }

        /// <summary>
        /// HashCode
        /// </summary>
        public override int GetHashCode() {
            return (BaseArray != null ? BaseArray.GetHashCode() : 0) ^ BaseIndex.GetHashCode();
        }

        /// <summary>
        /// Determine if <paramref name="obj"/> is equals with this one
        /// </summary>
        public override bool Equals(object obj) {
            if (obj is T[]) {
                return this.BaseArray == obj && BaseIndex == 0;
            } else if (obj is CPointer<T>) {
                return this.BaseArray == ((CPointer<T>)obj).BaseArray && BaseIndex == ((CPointer<T>)obj).BaseIndex;
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Read value pointed by access
        /// </summary>
        public static implicit operator T(CPointer<T> array) {
            return array.BaseArray[array.BaseIndex];
        }

        /// <summary>
        /// Implicit conversion of an array to an ArrayAccess
        /// </summary>
        public static implicit operator CPointer<T>(T[] array) {
            return new CPointer<T>(array);
        }

        /// <summary>
        /// Explicit converision of an ArrayAccess to an array
        /// </summary>
        public static explicit operator T[](CPointer<T> array) {
            return array.BaseArray == null ? null : array.BaseArray.Skip(array.BaseIndex).ToArray();
        }

        /// <summary>
        /// Increment an array access 'pointer'
        /// </summary>
        public static CPointer<T> operator +(CPointer<T> array, int offset) {
            return new CPointer<T>(array.BaseArray, array.BaseIndex + offset);
        }

        /// <summary>
        /// Decrement an array access 'pointer'
        /// </summary>
        public static CPointer<T> operator -(CPointer<T> array, int offset) {
            return new CPointer<T>(array.BaseArray, array.BaseIndex - offset);
        }

        /// <summary>
        /// Increment an array access 'pointer'
        /// </summary>
        public static CPointer<T> operator ++(CPointer<T> array) {
            return new CPointer<T>(array.BaseArray, array.BaseIndex + 1);
        }

        /// <summary>
        /// Decrement an array access 'pointer'
        /// </summary>
        public static CPointer<T> operator --(CPointer<T> array) {
            return new CPointer<T>(array.BaseArray, array.BaseIndex - 1);
        }

        /// <summary>
        /// Test if an inner array is the same of an array
        /// </summary>
        public static bool operator ==(CPointer<T> access, T[] array) {
            return access.BaseArray == array && access.BaseIndex == 0;
        }

        /// <summary>
        /// Test if an inner array is not the same of an array
        /// </summary>
        public static bool operator !=(CPointer<T> access, T[] array) {
            return access.BaseArray != array && access.BaseIndex == 0;
        }

        /// <summary>
        /// Test if an inner array is the same of an array
        /// </summary>
        public static bool operator ==(T[] array, CPointer<T> access) {
            return access.BaseArray == array && access.BaseIndex == 0;
        }

        /// <summary>
        /// Test if an inner array is not the same of an array
        /// </summary>
        public static bool operator !=(T[] array, CPointer<T> access) {
            return access.BaseArray != array && access.BaseIndex == 0;
        }

        /// <summary>
        /// Test two pointers equality
        /// </summary>
        public static bool operator ==(CPointer<T> p1, CPointer<T> p2) {
            return p1.BaseArray == p2.BaseArray && p1.BaseIndex == p2.BaseIndex;
        }

        /// <summary>
        /// Test two pointers inequality
        /// </summary>
        public static bool operator !=(CPointer<T> p1, CPointer<T> p2) {
            return p1.BaseArray != p2.BaseArray || p1.BaseIndex != p2.BaseIndex;
        }

        /// <summary>
        /// Inform if this AccessArray is null
        /// </summary>
        public bool IsNull { get { return BaseArray == null; } }

        /// <summary>
        /// Get or set the array values
        /// </summary>
        public T this[int idx] {
            get { return BaseArray[BaseIndex + idx]; }
            set { BaseArray[BaseIndex + idx] = value; }
        }

        /// <summary>
        /// Length
        /// </summary>
        public int Length {
            get { return BaseArray != null ? BaseArray.Length - BaseIndex : 0; }
        }

    }

}
