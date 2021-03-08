using System;
using System.Collections.Generic;
using System.Linq;

namespace PlantedMotifSearch
{
    public class Sequence
    {
        private char[] s;
        private int startIdx;
        private int len;

        public char this[int i]
        {
            get => this.s[i + startIdx];
            set => this.s[i + startIdx] = value;
        }

        public int Len => len;

        public Sequence(char[] s = null, int startIdx = 0, int len = -1)
        {
            this.s = s ?? Array.Empty<char>();
            this.startIdx = startIdx;
            this.len = len == -1 ? this.s.Length : len;
        }


        public Sequence(int len)
        {
            s = new char[len];
            this.startIdx = 0;
            this.len = len;
        }

        public void SetSequence(Sequence other, int startIdx = 0)
        {
            if (Len - startIdx < other.Len)
                throw new Exception("Sequence is too long to be inserted there");

            for (int i = 0; i < other.Len; i++)
            {
                s[i + startIdx] = other.s[i];
            }
        }

        public Sequence SubSequence(int startIdx, int len)
        {
            if (Len - (this.startIdx + startIdx) < len)
                throw new Exception("Specified length is too long");

            return new Sequence(this.s, this.startIdx + startIdx, len);

            /*
                    var sub = new Sequence(len);
                    for (int i = 0; i < len; i++)
                    {
                        sub[i] = this[i + startIdx];
                    }
        
                    return sub;*/
        }

        public IEnumerable<(Sequence, List<Sequence>)> CommonMers(List<Sequence> otherMers, int l, int d)
        {
            return Mers(l).Select(m => (m, m.Neighbours(otherMers, d).ToList())).Where(t => t.Item2.Any());
        }

        public IEnumerable<Sequence> Neighbours(List<Sequence> potentialNeighbours, int d)
        {
            return potentialNeighbours.Where(n => HammingDist(n) <= d);
        }

        public List<Sequence> Mers(int l)
        {
            var list = new List<Sequence>();

            for (int i = 0; i < s.Length - l + 1; i++)
            {
                list.Add(this.SubSequence(i, l));
            }

            return list;
        }

        public int HammingDist(Sequence other, int startIdx = 0)
        {
            int dist = 0;
            for (int i = 0; i < other.Len; i++)
            {
                if (this[i + startIdx] != other[i]) dist++;
            }

            return dist;
        }

        public int MotifHammingDist(Sequence motif)
        {
            if (motif.Len > Len)
            {
                throw new Exception("Motif cannot be longer than sequence");
            }

            var bestDist = int.MaxValue;

            for (int i = 0; i < Len - motif.Len + 1; i++)
            {
                bestDist = Math.Min(bestDist, HammingDist(motif, i));
            }

            return bestDist;
        }

        public Sequence Clone()
        {
            return new(s.Clone() as char[], startIdx, len);
        }

        public string toString()
        {
            string s = "";
            for (int i = 0; i < Len; i++)
                s += this[i];
            return s;
        }

        public int MotifDistance(List<Sequence> sequences)
        {
            return sequences.Select(s => s.MotifHammingDist(this)).Max();
        }
    }
}