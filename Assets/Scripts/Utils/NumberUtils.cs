using System;

public static class NumberUtils
{
    public static double Round(double value, int dec = 2)
    {
        var d = Math.Pow(10, dec);
        return Math.Ceiling(value * d) / d;
    }
}