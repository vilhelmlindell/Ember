using System;
using System.Collections;

namespace Ember.Extensions
{
    public static class BitArrayExtensions
    {
        public static bool EqualBits(this BitArray first, BitArray second)
        {
            // Short-circuit if the arrays are not equal in size
            if (first.Length != second.Length)
                return false;

            // Convert the arrays to int[]s
            int[] firstInts = new int[(int)Math.Ceiling((decimal)first.Count / 32)];
            first.CopyTo(firstInts, 0);
            int[] secondInts = new int[(int)Math.Ceiling((decimal)second.Count / 32)];
            second.CopyTo(secondInts, 0);

            // Look for differences
            bool areDifferent = false;
            for (int i = 0; i < firstInts.Length && !areDifferent; i++)
                areDifferent = firstInts[i] != secondInts[i];

            return !areDifferent;
        }
        public static BitArray UnmodifiedAnd(this BitArray first, BitArray second)
        {
            return new BitArray(first).And(second);
        }
        public static BitArray UnmodifiedOr(this BitArray first, BitArray second)
        {
            return new BitArray(first).Or(second);
        }
        public static BitArray UnmodifiedXor(this BitArray first, BitArray second)
        {
            return new BitArray(first).Xor(second);
        }
        public static BitArray UnmodifiedNot(this BitArray first)
        {
            return new BitArray(first).Not();
        }
        public static BitArray UnmodifiedRightShift(this BitArray first, int count)
        {
            return new BitArray(first).RightShift(count);
        }
        public static BitArray UnmodifiedLeftShift(this BitArray first, int count)
        {
            return new BitArray(first).LeftShift(count);
        }
    }
}