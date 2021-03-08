using System.Collections.Generic;
using PlantedMotifSearch.SequenceGeneration;

namespace PlantedMotifSearch
{
    public class Stuff : PmsAlgorithm
    {
        private SequenceGenerator _generator;

        public Stuff(SequenceGenerator generator)
        {
            _generator = generator;
        }

        public Sequence Search(List<Sequence> sequences, int l, int d)
        {
            var a = sequences[0].CommonMers(sequences[1].Mers(l), l, d * 2);
            throw new System.NotImplementedException();
        }
    }
}