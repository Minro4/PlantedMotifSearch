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

        public bool SearchOnce(List<Sequence> sequences, int l, int d)
        {
            var mers = sequences[0].SubSequence(0, l);

            var res = ClimbSingleChild(new HillMotif(mers, value(mers, sequences)), sequences, d);

            return (int) res.dist <= d;
        }

        public Sequence Search(List<Sequence> sequences, int l, int d)
        {
            var best = new HillMotif(null, double.MaxValue);

            foreach (var sequence in sequences[0].Mers(l))
            {
                var res = Climb(sequence, sequences, d, 1);

                Console.WriteLine(res.dist);

                if (res.dist < best.dist)
                {
                    best = res;
                }

                if ((int) best.dist <= d)
                    break;


                // return (current, new int[1]);
            }

            Console.WriteLine("BEST ONE:" + best.Sequence.MotifDistance(sequences));
            return best.Sequence;
        }

        //TODO fix bests pourrait etre mieux opt
        public Sequence AdaptiveSearch(List<Sequence> sequences, int l, int d, int upTo)
        {
            int keep = sequences.Count;

            var bests = new List<HillMotif>();
            foreach (var sequence in sequences[0].Mers(l))
            {
                bests.Add(new HillMotif(sequence, value(sequence, sequences)));
            }

            bests = bests.OrderBy((x) => x.dist).ToList();

            for (int i = 1; i <= upTo; i++)
            {
                for (int j = 0; j < keep; j++)
                {
                    var res = ClimbOfDist(bests[j], sequences, d, 1);
                    bests.Add(res);

                    if ((int) res.dist <= d)
                        return res.Sequence;
                }

                bests = bests.OrderBy((x) => x.dist).ToList();
                keep /= 4;
            }

            return bests[0].Sequence;
        }

        public List<Sequence> Test(List<Sequence> sequences, int l, int d)
        {
            var motifs = new List<Sequence>();

            foreach (var sequence in sequences[0].Mers(l))
            {
                var res = Climb2(sequence, sequences, d);

                Console.WriteLine(res.dist);
                Console.WriteLine(sequence.toString());
                Console.WriteLine(res.Sequence.toString());

                if ((int) res.dist <= d)
                    motifs.Add(res.Sequence);


                // return (current, new int[1]);
            }

            return motifs;
        }


        public HillMotif Climb(Sequence motif, List<Sequence> sequences, int d, int childExploration)
        {
            var l = new List<HillMotif>();
            l.Add(new HillMotif(motif, value(motif, sequences)));
            foreach (var s in _generator.Neighbours(motif, 1))
            {
                l.Add(new HillMotif(s, value(s, sequences)));
            }

            l = l.OrderBy((x) => x.dist).ToList();

            var best = new HillMotif(motif, value(motif, sequences));

            for (int i = 0; i <= childExploration; i++)
            {
                if ((int) l[i].dist <= d)
                    return l[i];

                if (l[i].Sequence != motif)
                {
                    var res = Climb(l[i].Sequence, sequences, d, childExploration / 2);

                    if (res.dist < best.dist)
                    {
                        best = res;
                    }
                }
            }

            return best;
        }

        public HillMotif Climb2(Sequence motif, List<Sequence> sequences, int maxDepth)
        {
            if (maxDepth == 0)
                return new HillMotif(motif, value(motif, sequences));

            var best = new HillMotif(motif, value(motif, sequences));
            foreach (var s in _generator.Neighbours(motif, 1))
            {
                var n = new HillMotif(s, value(s, sequences));

                if (n.dist < best.dist)
                {
                    best = n;
                }
            }

            return Climb2(best.Sequence, sequences, maxDepth - 1);
        }

        public HillMotif ClimbSingleChild(HillMotif motif, List<Sequence> sequences, int d)
        {
            var best = motif;

            if ((int) best.dist <= d)
                return best;

            foreach (var s in _generator.Neighbours(motif.Sequence, 1))
            {
                var n = new HillMotif(s, value(s, sequences));

                if (n.dist < best.dist)
                {
                    best = n;

                    if ((int) best.dist <= d)
                        return best;
                }
            }

            if (best.Sequence == motif.Sequence)
                return motif;

            return ClimbSingleChild(best, sequences, d);
        }

        //TODO faire le meme chose mais juste faire le adaptive climb pour les x meilleurs
        public HillMotif AdaptiveClimb(Sequence motif, List<Sequence> sequences, int upTo, int d)
        {
            var best = new HillMotif(motif, value(motif, sequences));


            if ((int) best.dist <= d)
                return best;

            for (int i = 1; i <= upTo; i++)
            {
                HillMotif newBest = best;
                do
                {
                    best = newBest;
                    foreach (var s in _generator.NeighboursOfDist(best.Sequence, i))
                    {
                        var n = new HillMotif(s, value(s, sequences));

                        if (n.dist < newBest.dist)
                        {
                            newBest = n;

                            if ((int) newBest.dist <= d)
                                return newBest;
                        }
                    }
                } while (newBest.Sequence != best.Sequence);
            }

            return best;
        }

        public HillMotif ClimbOfDist(HillMotif motif, List<Sequence> sequences, int dist, int d)
        {
            var best = motif;


            if ((int) best.dist <= d)
                return best;


            HillMotif newBest = best;
            do
            {
                best = newBest;
                foreach (var s in _generator.NeighboursOfDist(best.Sequence, dist))
                {
                    var n = new HillMotif(s, value(s, sequences));

                    if (n.dist < newBest.dist)
                    {
                        newBest = n;

                        if ((int) newBest.dist <= d)
                            return newBest;
                    }
                }
            } while (newBest.Sequence != best.Sequence);


            return best;
        }


        private double value(Sequence motif, List<Sequence> sequences)
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