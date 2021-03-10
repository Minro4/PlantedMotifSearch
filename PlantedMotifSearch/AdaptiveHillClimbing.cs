using System;
using System.Collections.Generic;
using System.Linq;
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
            return AdaptiveSearch(sequences, l, d);
        }

        public Sequence AdaptiveSearch0(List<Sequence> sequences, int l, int d, int upTo = 2)
        {
            var bests = new List<HillMotif>();
            foreach (var sequence in sequences[0].Mers(l))
            {
                var m = new HillMotif(new Neighbour(sequence), value(sequence, sequences));
                bests.Add(m);
                if ((int) m.dist <= d)
                    return m.Sequence.original;
            }

            int keep = bests.Count / 20;

            bests = bests.OrderBy((x) => x.dist).ToList();

            for (int i = 1; i <= upTo; i++)
            {
                for (int j = 0; j < keep; j++)
                {
                    var res = ClimbOfDist0(bests[j], sequences, d, i);
                    bests.Add(res);

                    if ((int) res.dist <= d)
                        return res.Sequence.original;
                }

                bests = bests.OrderBy((x) => x.dist).ToList();
                keep /= 39;
            }

            return bests[0].Sequence.original;
        }

        //TODO fix bests pourrait etre mieux opt
        public Sequence AdaptiveSearch(List<Sequence> sequences, int l, int d, int upTo = 2)
        {
            var bests = new List<HillMotif>();
            foreach (var sequence in sequences[0].Mers(l))
            {
                var m = new HillMotif(new Neighbour(sequence), value(sequence, sequences));
                bests.Add(m);
                if ((int) m.dist <= d)
                    return m.Sequence.toSequence();
            }

            int keep = bests.Count / 20;

            bests = bests.OrderBy((x) => x.dist).ToList();

            for (int i = 1; i <= upTo; i++)
            {
                for (int j = 0; j < keep; j++)
                {
                    // var res0 = ClimbOfDist0(bests[j], sequences, d, i);
                    var res = ClimbOfDist(bests[j], sequences, d, i);
                    bests.Add(res);

                    if ((int) res.dist <= d)
                        return res.Sequence.toSequence();
                }

                bests = bests.OrderBy((x) => x.dist).ToList();
                keep /= 39;
            }

            return bests[0].Sequence.toSequence();
        }

        public HillMotif ClimbOfDist0(HillMotif motif, List<Sequence> sequences, int d, int dist)
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

                foreach (var s in _generator.NeighboursOfDist(best.Sequence.toSequence(), dist))
                {
                    var n = new HillMotif(new Neighbour(s), value(s, sequences));

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

                foreach (var s in _generator.NeighboursOfDist2(best.Sequence.toSequence(), dist))
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


        private double value(Sequence motif, List<Sequence> sequences)
        {
            var dists = sequences.Select(s => s.MotifHammingDist(motif));
            var intP = dists.Max();
            var dP = dists.Select(v => v * v).Sum();
            return intP + ((double) dP / 20000);
        }

        private double neighbourValue(Neighbour motif, List<Sequence> sequences)
        {
            var dists = sequences.Select(s => s.MotifHammingDist(motif));
            var intP = dists.Max();
            var dP = dists.Select(v => v * v).Sum();
            return intP + ((double) dP / 20000);
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