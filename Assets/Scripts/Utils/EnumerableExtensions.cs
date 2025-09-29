using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtensions
{
    public static T GetOrDefault<T>(this IEnumerable<T> source, int index, T defaultValue = default)
    {
        if (source == null || index < 0)
            return defaultValue;

        using (var enumerator = source.GetEnumerator())
        {
            for (int i = 0; i <= index; i++)
            {
                if (!enumerator.MoveNext())
                    return defaultValue;
            }
            return enumerator.Current;
        }
    }
}