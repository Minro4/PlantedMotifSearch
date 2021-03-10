using System;
using System.Collections.Generic;
using System.Linq;
using PlantedMotifSearch.SequenceGeneration;

namespace PlantedMotifSearch
{
    public class RandomHillClimbing : PmsAlgorithm
    {
        private SequenceGenerator _generator;

        public RandomHillClimbing(SequenceGenerator generator)
        {
            _generator = generator;
        }

        public Sequence Search(List<Sequence> sequences, int l, int d)
        {
            Sequence best = null;
            var bestValue = int.MaxValue;

            foreach (var sequence in sequences[0].Mers(l))
            {
                var current = sequence;
                for (int i = l; i > 0; i--)
                {
                    for (int j = 0; j < 250; j++)
                    {
                        var newC = RandomSearch(current, sequences, i, 250);
                        if (newC == current)
                            break;
                        current = newC;

                        Console.WriteLine("small one:" + value(current, sequences) + " : " + subValueSqr(current, sequences));
                    }

                    var intern = value(current, sequences);
                    Console.WriteLine("intern:" + value(current, sequences) + " : " + subValueSqr(current, sequences));
                }

                var v = value(current, sequences);


                if (v < bestValue)
                {
                    best = current;
                    bestValue = v;
                }

                Console.WriteLine("big one:" + v);
                Console.WriteLine("best one:" + bestValue);
                // return (current, new int[1]);
            }

            return best;
        }


        public Sequence RandomSearch(Sequence motif, List<Sequence> sequences, int d, int generations)
        {
            if (d == 0)
                return motif;

            if (d == 1)
                return NeighbourSearch(motif, sequences, d);

            Sequence best = motif;
            var bestValue = value(motif, sequences);
            var bestSVS = subValueSqr(motif, sequences);

            for (int i = 0; i < generations; i++)
            {
                Sequence s = _generator.RandomNeighbour(motif, d);
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

        public Sequence NeighbourSearch(Sequence motif, List<Sequence> sequences, int d)
        {
            Sequence best = motif;
            var bestValue = value(motif, sequences);
            var bestSVS = subValueSqr(motif, sequences);

            var n = _generator.Neighbours(motif, d);
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

        private int subValueSqr(Sequence motif, List<Sequence> sequences)
        {
            return sequences.Select(s => s.MotifHammingDist(motif)).Select(v => v * v).Sum();
        }
    }
}