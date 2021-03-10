using System;

namespace PlantedMotifSearch
{
    public struct HillMotif
    {
        public Neighbour Sequence;
        public double dist;


        public HillMotif(Neighbour sequence, double dist)
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