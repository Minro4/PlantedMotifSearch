using System.Collections.Generic;

namespace PlantedMotifSearch
{
    public struct Neighbour
    {
        public Sequence original { get; private set; }
        public List<Difference> differences { get; private set; }

        public Neighbour(Sequence original, List<Difference> differences = null)
        {
            this.original = original;
            this.differences = differences ?? new List<Difference>();
        }

        public Sequence toSequence()
        {
            var s = original.Clone();
            foreach (var dif in differences)
            {
                s[dif.index] = dif.value;
            }

            return s;
        }

        public Neighbour Clone()
        {
            return new Neighbour(original, new List<Difference>(differences));
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