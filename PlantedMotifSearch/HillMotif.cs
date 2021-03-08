using System;

namespace PlantedMotifSearch
{
    public struct HillMotif
    {
        public Sequence Sequence;
        public double dist;


        public HillMotif(Sequence sequence, double dist)
        {
            Sequence = sequence;
            this.dist = dist;
        }

        /*
        int IComparable.CompareTo(object obj)
        {
            HillMotif c=(HillMotif)obj;
            return String.Compare(this.make,c.make);
        }*/
    }
}