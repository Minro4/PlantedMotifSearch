using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlantedMotifSearch.SequenceGeneration;

namespace PlantedMotifSearch
{
    public class AdaptiveHillClimbing : PmsAlgorithm
    {
        private SequenceGenerator _generator;

        public AdaptiveHillClimbing(SequenceGenerator generator)
        {
            _generator = generator;
        }

        public Sequence Search(List<Sequence> sequences, int l, int d)
        {
            var keep = new[] {10};
            return AdaptiveSearch(sequences, l, d, keep);
        }


        //TODO fix bests pourrait etre mieux opt
        public Sequence AdaptiveSearch(List<Sequence> sequences, int l, int d, int[] keep)
        {
            foreach (var baseCandidate in sequences)
            {
                var candidates = baseCandidate.Mers(l).AsParallel().Select((sequence, i) =>
                    new HillMotif(new Neighbour(sequence), value(sequence, sequences))).ToList();

                var instantFind = candidates.Where((motif => (int) motif.dist <= d));
                if (instantFind.Any())
                {
                    Console.WriteLine("instant find");
                    return instantFind.First().Sequence.toSequence();
                }

                candidates = candidates.OrderBy((x) => x.dist).ToList();

                for (int i = 0; i < keep.Length; i++)
                {
                    for (int j = 0; j < keep[i] && j < candidates.Count; j++)
                    {
                        var res = ClimbOfDist(candidates[j], sequences, d, i + 1);
                        candidates.Add(res);

                        if ((int) res.dist <= d)
                        {
                            Console.WriteLine("found on: " + j);
                            return res.Sequence.toSequence();
                        }
                    }

                    candidates = candidates.OrderBy((x) => x.dist).ToList();
                }
            }


            return sequences[0].SubSequence(0, l);
        }

        public HillMotif ClimbOfDist(HillMotif motif, List<Sequence> sequences, int d, int dist)
        {
            var best = motif;


            if ((int) best.dist <= d)
                return best;


            HillMotif newBest = best;
            bool changed;
            do
            {
                changed = false;
                best = newBest;

                foreach (var s in _generator.NeighboursOfDist2(best.Sequence, dist))
                {
                    var n = new HillMotif(s, neighbourValue(s, sequences));

                    if (n.dist < newBest.dist)
                    {
                        newBest = n;
                        changed = true;

                        if ((int) newBest.dist <= d)
                            return newBest;
                    }
                }
            } while (changed);


            return best;
        }

        /***
         * Returns the distance for a motif in a list of sequences
         * The integer part of the value is the maximum distance of the best motif match in all sequences.
         * The fractional part is the sum of squared distances for the best motif match in all sequences.
         */
        private static double value(Sequence motif, List<Sequence> sequences)
        {
            var dists = sequences.AsParallel().Select(s => s.MotifHammingDist(motif));
            var intP = dists.Max();
            var dP = dists.Select(v => v * v).Sum();
            return intP + ((double) dP / 20000);
        }

        /***
          * Returns the distance for a motif in a list of sequences
          * The integer part of the value is the maximum distance of the best motif match in all sequences.
          * The fractional part is the sum of squared distances for the best motif match in all sequences.
          */
        private static double neighbourValue(Neighbour motif, List<Sequence> sequences)
        {
            var dists = sequences.AsParallel().Select(s => s.MotifHammingDist(motif));
            var intP = dists.Max();
            var dP = dists.Select(v => v * v).Sum();
            return intP + ((double) dP / 20000);
        }
    }
}