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
        private int[] keep = {5, 1};

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


        //TODO fix bests pourrait etre mieux opt
        public Sequence AdaptiveSearch(List<Sequence> sequences, int l, int d, int[] keep)
        {
            foreach (var baseCandidate in sequences)
            {
                Console.WriteLine("test candidate");
                var candidates = new List<HillMotif>();
                foreach (var sequence in baseCandidate.Mers(l))
                {
                    var m = new HillMotif(new Neighbour(sequence), value(sequence, sequences));
                    candidates.Add(m);
                    if ((int) m.dist <= d)
                        return m.Sequence.toSequence();
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


            return null;
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

                foreach (var s in _generator.NeighboursOfDist(best.Sequence, dist))
                {
                    var n = new HillMotif(s, value(s.toSequence(), sequences));

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
    }
}