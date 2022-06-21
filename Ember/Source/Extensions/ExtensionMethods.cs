namespace System.Collections
{
    public static class MyExtensionMethods
    {
        public static bool EqualTo(this BitArray first, BitArray second)
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
    }
}