using System;
using System.Collections.Generic;
using System.Linq;
using PlantedMotifSearch.SequenceGeneration;

namespace PlantedMotifSearch
{
    public class HillClimbing2 : PmsAlgorithm
    {
        private SequenceGenerator _generator;

        public HillClimbing2(SequenceGenerator generator)
        {
            _generator = generator;
        }

        public (Sequence, int[]) Search(List<Sequence> sequences, int l, int d)
        {
            Sequence best = null;
            var bestValue = int.MaxValue;

            foreach (var sequence in sequences[0].Mers(l))
            {
                var current = sequence;

                for (int j = 0; j < 20; j++)
                {
                    var newC = RandomSearch(current, sequences);
                    if (newC == current)
                        break;
                    current = newC;
                    Console.WriteLine("small one:" + value(current, sequences) + " : " + subValueSqr(current, sequences));
                }

                var v = value(current, sequences);
                Console.WriteLine("big one:" + v);

                if (v < bestValue)
                {
                    best = current;
                    bestValue = v;
                }


                // return (current, new int[1]);
            }

            Console.WriteLine("BEST ONE:" + value(best, sequences));
            return (best, new int[1]);
        }


        public Sequence RandomSearch(Sequence motif, List<Sequence> sequences)
        {
            Sequence best = motif;
            var bestValue = value(motif, sequences);
            var bestSVS = subValueSqr(motif, sequences);

            var n = _generator.Neighbours(motif, 2);
            foreach (var s in n)
            {
                var val = value(s, sequences);
                var svs = subValueSqr(s, sequences);
                if (val < bestValue || (val == bestValue && svs < bestSVS))
                {
                    best = s;
                    bestValue = val;
                    bestSVS = svs;
                }
            }

            return best;
        }


        private int value(Sequence motif, List<Sequence> sequences)
        {
            var distances = sequences.Select(s => s.MotifHammingDist(motif));
            return distances.Max();
        }

        private int subValue(Sequence motif, List<Sequence> sequences)
        {
            return sequences.Select(s => s.MotifHammingDist(motif)).Sum();
        }

        private int subValueSqr(Sequence motif, List<Sequence> sequences)
        {
            return sequences.Select(s => s.MotifHammingDist(motif)).Select(v => v * v).Sum();
        }
    }
}