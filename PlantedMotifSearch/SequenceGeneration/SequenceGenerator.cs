using System;
using System.Collections.Generic;
using System.Linq;

namespace PlantedMotifSearch.SequenceGeneration
{
    public class SequenceGenerator
    {
        private readonly List<char> _alphabet;

        public SequenceGenerator(char[] alphabet)
        {
            _alphabet = new List<char>(alphabet);
        }

        public (Sequence, List<Sequence>) PlantedMotif(int l, int d, int nbrSequences, int seqLen)
        {
            var seqs = new List<Sequence>(nbrSequences);

            var motif = RandomSequence(l);

            var rnd = new Random();
            for (int i = 0; i < nbrSequences; i++)
            {
                var seq = RandomSequence(seqLen);

                //insertMotif
                var genD = rnd.Next(d + 1);
                //Console.WriteLine(genD);
                var rndMotif = RandomNeighbourOfDist(motif, genD);
                var startIdx = /*i == 0 ? 0 :*/ rnd.Next(seqLen - l);
                seq.SetSequence(rndMotif, startIdx);

                seqs.Add(seq);
            }

            return (motif, seqs);
        }


        public Sequence RandomSequence(int len)
        {
            var seq = new Sequence(len);

            var rnd = new Random();
            for (int i = 0; i < len; i++)
            {
                seq[i] = _alphabet[rnd.Next(0, _alphabet.Count)];
            }

            return seq;
        }

        /// <summary>
        /// Neughbours of dist d or less
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public IList<Sequence> Neighbours(Sequence sequence, int d)
        {
            var n = new List<Sequence>();
            //n.Add(sequence);

            for (int dist = 1; dist <= d; dist++)
            {
                n.AddRange(NeighboursOfDist(sequence, dist));
            }

            return n;
        }

        /// <summary>
        /// Neighbours of dist exactly d
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public IList<Sequence> NeighboursOfDist(Sequence sequence, int d)
        {
            var n = new List<Sequence>();

            var combs = Utils.Combination(d, sequence.Len);

            foreach (var combination in combs)
            {
                n.AddRange(SequencesWithDifferingCharAtIdx(sequence, combination));
            }

            return n;
        }

        private IEnumerable<Sequence> SequencesWithDifferingCharAtIdx(Sequence sequence, IEnumerable<int> indices)
        {
            var previousSequences = new List<Sequence> {sequence};

            foreach (var index in indices)
            {
                var newSequences = new List<Sequence>();

                var currentCharIdx = _alphabet.FindIndex((c) => c == sequence[index]);

                foreach (var seq in previousSequences)
                {
                    for (int l = 1; l < _alphabet.Count; l++)
                    {
                        var s = seq.Clone();
                        newSequences.Add(s);

                        var newLetter = _alphabet[(currentCharIdx + l) % _alphabet.Count];
                        s[index] = newLetter;
                    }
                }

                previousSequences = newSequences;
            }

            return previousSequences;
        }

        public IList<Neighbour> NeighboursOfDist2(Sequence sequence, int d)
        {
            var n = new List<Neighbour>();

            var combs = Utils.Combination(d, sequence.Len);

            foreach (var combination in combs)
            {
                n.AddRange(SequencesWithDifferingCharAtIdx2(sequence, combination));
            }

            return n;
        }

        private IEnumerable<Neighbour> SequencesWithDifferingCharAtIdx2(Sequence sequence, IEnumerable<int> indices)
        {
            var previousSequences = new List<Neighbour> {new Neighbour(sequence)};

            foreach (var index in indices)
            {
                var newSequences = new List<Neighbour>();

                var currentCharIdx = _alphabet.FindIndex((c) => c == sequence[index]);

                foreach (var seq in previousSequences)
                {
                    for (int l = 1; l < _alphabet.Count; l++)
                    {
                        var s = seq.Clone();


                        var newLetter = _alphabet[(currentCharIdx + l) % _alphabet.Count];
                        s.differences.Add(new Difference(index, newLetter));

                        newSequences.Add(s);
                    }
                }

                previousSequences = newSequences;
            }

            return previousSequences;
        }


        //TODO le random est poche
        public Sequence RandomNeighbour(Sequence sequence, int d)
        {
            if (d > sequence.Len)
                throw new Exception("Cannot distance cannot be greater than sequence length");

            var indices = new List<int>();
            var rand = new Random();
            while (indices.Count != d)
            {
                var rnd = rand.Next(sequence.Len);
                if (!indices.Contains(rnd))
                    indices.Add(rnd);
            }

            var rndSeq = sequence.Clone();
            foreach (var index in indices)
            {
                rndSeq[index] = _alphabet[rand.Next(_alphabet.Count)];
            }

            return rndSeq;
        }

        //TODO le random est poche
        public Sequence RandomNeighbourOfDist(Sequence sequence, int d)
        {
            if (d > sequence.Len)
                throw new Exception("Cannot distance cannot be greater than sequence length");

            var indices = new List<int>();
            var rand = new Random();
            while (indices.Count != d)
            {
                var rnd = rand.Next(sequence.Len);
                if (!indices.Contains(rnd))
                    indices.Add(rnd);
            }

            var rndSeq = sequence.Clone();
            foreach (var index in indices)
            {
                var previousLetterIdx = _alphabet.FindIndex((c) => c == sequence[index]);
                var newLetterIdx = (previousLetterIdx + 1 + rand.Next(_alphabet.Count - 1)) % _alphabet.Count;
                rndSeq[index] = _alphabet[newLetterIdx];
            }

            return rndSeq;
        }
    }
}