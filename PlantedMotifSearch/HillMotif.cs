using System;

namespace PlantedMotifSearch
{
    /***
     * Motif candidate and its distance
     */
    public struct HillMotif
    {
        public Neighbour Sequence;

        public double dist;


        public HillMotif(Neighbour sequence, double dist)
        {
            Sequence = sequence;
            this.dist = dist;
        }

        public bool IsMotif(int d)
        {
            return (int) dist <= d;
        }

        /*
        int IComparable.CompareTo(object obj)
        {
            HillMotif c=(HillMotif)obj;
            return String.Compare(this.make,c.make);
        }*/
    }
}
