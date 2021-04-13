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

        private int[] keep =
        {
            5, 1
        };

        public AdaptiveHillClimbing(SequenceGenerator generator, int[] keep = null)
        {
            _generator = generator;
            if (keep != null)
                this.keep = keep;
        }

        public Sequence Search(List<Sequence> sequences, int l, int d)
        {
            return AdaptiveSearch(sequences, l, d, keep);
        }


        private Sequence AdaptiveSearch(List<Sequence> sequences, int l, int d, int[] keep)
        {
            var candidateGroups = sequences.Select(s => s.Mers(l)
                                                         .Select(lmers => new HillMotif(new Neighbour(lmers), value(lmers, sequences))));

            foreach (var startCandidates in candidateGroups)
            {
                var candidates = startCandidates;

                for (var i = 0; i <= keep.Length; i++)
                {
                    var newCandidates = new List<HillMotif>();

                    var candidatesToEvaluate = i == 0 ? candidates : candidates.Take(keep[i - 1]);

                    foreach (var candidate in candidatesToEvaluate)
                    {
                        var res = Climb(candidate, sequences, d, i);
                        newCandidates.Add(res);

                        if (res.IsMotif(d))
                            return res.Sequence.toSequence();

                    }

                    candidates = newCandidates.OrderBy((x) => x.dist);
                }
            }

            return null;
        }

        public HillMotif Climb(HillMotif motif, List<Sequence> sequences, int d, int dist)
        {
            var best = motif;

            if (dist == 0) return motif;

            for (var i = dist; i <= dist; i++)
            {
                var changed = true;
                while (changed)
                {
                    changed = false;

                    foreach (var s in _generator.NeighboursOfDist(best.Sequence, i))
                    {
                        var n = new HillMotif(s, value(s.toSequence(), sequences));

                        if (n.dist < best.dist)
                        {
                            best = n;
                            changed = true;
                            i = 1;

                            if (best.IsMotif(d))
                                return best;
                        }
                    }
                }
            }

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
    }
}
