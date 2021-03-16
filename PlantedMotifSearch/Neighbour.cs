using System.Collections.Generic;

namespace PlantedMotifSearch
{
    /***
     * Sequence neighbour
     * is used to optimise hamming dist calculations because once we calculate the original sequence distance, we only need to calculate the differences.
     */
    public struct Neighbour
    {
        public Sequence original { get; private set; }
        public IDictionary<int, char> differences { get; private set; }

        public char this[int i]
        {
            get
            {
                char value;
                if (differences.TryGetValue(i, out value)) return value;
                else return original[i];
            }
            set => differences[i] = value;
        }

        public Neighbour(Sequence original, SortedDictionary<int, char> differences = null)
        {
            this.original = original;
            this.differences = differences ?? new SortedDictionary<int, char>();
        }

        public Sequence toSequence()
        {
            var s = original.Clone();
            foreach (var dif in differences)
            {
                s[dif.Key] = dif.Value;
            }

            return s;
        }

        public Neighbour Clone()
        {
            return new Neighbour(original, new SortedDictionary<int, char>(differences));
        }
    }

    public struct Difference
    {
        public int index { get; private set; }
        public char value { get; private set; }

        public Difference(int index, char value)
        {
            this.index = index;
            this.value = value;
        }
    }
}