using System.Collections.Generic;

namespace PlantedMotifSearch
{
    public class Neighbour
    {
        public Sequence original { get; private set; }
        public List<Difference> differences { get; private set; }

        public Neighbour(Sequence original, List<Difference> differences = null)
        {
            this.original = original;
            this.differences = differences ?? new List<Difference>();
        }

        public Neighbour Clone()
        {
            return new Neighbour(original, differences);
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