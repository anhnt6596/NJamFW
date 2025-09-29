using System;
using System.Collections.Generic;

public static class RandomHelper
{
    private static Random rng = new Random();

    public static List<T> RandomUniqueList<T>(List<T> source, int length)
    {
        if (length > source.Count)
            throw new ArgumentException("Source Length must be greater than length");

        List<T> copy = new List<T>(source);

        int n = copy.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = copy[k];
            copy[k] = copy[n];
            copy[n] = value;
        }

        return copy.GetRange(0, length);
    }
}