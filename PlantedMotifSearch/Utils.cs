using System;
using System.Collections.Generic;
using System.Linq;

namespace PlantedMotifSearch
{
    public static class Utils
    {
        public static IEnumerable<IEnumerable<int>> Combination(int k, int n)
        {
            var l = new List<int>(n);
            for (int i = 0; i < n; i++)
                l.Add(i);

            return GetKCombs(l, k);
        }

        public static IEnumerable<IEnumerable<T>>
            GetKCombs<T>(IEnumerable<T> list, int k) where T : IComparable
        {
            if (k == 1) return list.Select(t => new T[] {t});
            return GetKCombs(list, k - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                    (t1, t2) => t1.Concat(new T[] {t2}));
        }

        public static double NbrCombinations(int n, int r)
        {
            return Factorial(n, n - r) / Factorial(r);
        }

        public static double Factorial(int n, int stop = 0)
        {
            if (n == stop + 1)
                return stop + 1;

            return n * Factorial(n - 1, stop);
        }
    }
}