using System;

namespace CASS.Types
{
    /// <summary>
    /// Used to represent a platform dependent sized variable.
    /// On 32 bit platforms it is 4 bytes wide (int, uint), on 64 bit it is
    /// 8 bytes wide (long, ulong).
    /// 
    /// This class maps to the C/C++ native size_t data type.
    /// </summary>
    public struct SizeT
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance based on the given value.
        /// </summary>
        /// <param name="value">Integer value to represent.</param>
        public SizeT(int value)
        {
            this.value = new IntPtr(value);
        }

        /// <summary>
        /// Creates a new instance based on the given value.
        /// </summary>
        /// <param name="value">Integer value to represent.</param>
        public SizeT(uint value)
        {
            this.value = new IntPtr((int)value);
        }

        /// <summary>
        /// Creates a new instance based on the given value.
        /// </summary>
        /// <param name="value">Integer value to represent.</param>
        public SizeT(long value)
        {
            this.value = new IntPtr(value);
        }

        /// <summary>
        /// Creates a new instance based on the given value.
        /// </summary>
        /// <param name="value">Integer value to represent.</param>
        public SizeT(ulong value)
        {
            this.value = new IntPtr((long)value);
        }
        #endregion

        #region Cast Operators
        /// <summary>
        /// Converts the object to int.
        /// </summary>
        /// <param name="t">Object to convert.</param>
        /// <returns>Integer value represented by the object.</returns>
        public static implicit operator int(SizeT t)
        {
            return t.value.ToInt32();
        }

        /// <summary>
        /// Converts the object to uint.
        /// </summary>
        /// <param name="t">Object to convert.</param>
        /// <returns>Integer value represented by the object.</returns>
        public static implicit operator uint(SizeT t)
        {
            return (uint)t.value;
        }

        /// <summary>
        /// Converts the object to long.
        /// </summary>
        /// <param name="t">Object to convert.</param>
        /// <returns>Integer value represented by the object.</returns>
        public static implicit operator long(SizeT t)
        {
            return t.value.ToInt64();
        }

        /// <summary>
        /// Converts the object to ulong.
        /// </summary>
        /// <param name="t">Object to convert.</param>
        /// <returns>Integer value represented by the object.</returns>
        public static implicit operator ulong(SizeT t)
        {
            return (ulong)t.value;
        }

        /// <summary>
        /// Converts the given integer to an object.
        /// </summary>
        /// <param name="value">Integer value to convert.</param>
        /// <returns>New object representing this value.</returns>
        public static implicit operator SizeT(int value)
        {
            return new SizeT(value);
        }

        /// <summary>
        /// Converts the given integer to an object.
        /// </summary>
        /// <param name="value">Integer value to convert.</param>
        /// <returns>New object representing this value.</returns>
        public static implicit operator SizeT(uint value)
        {
            return new SizeT(value);
        }

        /// <summary>
        /// Converts the given integer to an object.
        /// </summary>
        /// <param name="value">Integer value to convert.</param>
        /// <returns>New object representing this value.</returns>
        public static implicit operator SizeT(long value)
        {
            return new SizeT(value);
        }

        /// <summary>
        /// Converts the given integer to an object.
        /// </summary>
        /// <param name="value">Integer value to convert.</param>
        /// <returns>New object representing this value.</returns>
        public static implicit operator SizeT(ulong value)
        {
            return new SizeT(value);
        }
        #endregion

        #region Comparison Operator
        /// <summary>
        /// Compares two SizeT objects.
        /// </summary>
        /// <param name="val1">First value to compare.</param>
        /// <param name="val2">Second value to compare.</param>
        /// <returns>true or false for the comparison result.</returns>
        public static bool operator !=(SizeT val1, SizeT val2)
        {
            return val1.value != val2.value;
        }

        /// <summary>
        /// Compares two SizeT objects.
        /// </summary>
        /// <param name="val1">First value to compare.</param>
        /// <param name="val2">Second value to compare.</param>
        /// <returns>true or false for the comparison result.</returns>
        public static bool operator ==(SizeT val1, SizeT val2)
        {
            return val1.value == val2.value;
        }
        #endregion

        #region Overriden Functions
        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance or null.</param>
        /// <returns>true if obj is an instance of System.IntPtr and equals the value of this instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }

        /// <summary>
        /// Converts the numeric value of the current object to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation of the value of this instance.</returns>
        public override string ToString()
        {
            return value.ToString();
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
        #endregion

        #region Internal Variable
        private IntPtr value;
        #endregion
    }
}
