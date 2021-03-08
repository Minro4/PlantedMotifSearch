using System;
using PlantedMotifSearch.SequenceGeneration;

namespace PlantedMotifSearch
{
    class Program
    {
        private static readonly char[] alphabet = new[] {'A', 'C', 'G', 'T'};
        private static readonly int l = 27;
        private static readonly int d = 13;

        static void Main(string[] args)
        {
            var gen = new SequenceGenerator(alphabet);
            new Accuracy(gen).TestMultiple(13, 50, 4, 25, 10, "test.xlsx");

            /* (var motif, var s) = gen.PlantedMotif(l, d, 20, 600);


            var foundMotif = new HillClimbing2(gen).Search(s, l, d);

            Console.WriteLine("Motif: " + motif.toString());
            Console.WriteLine("Found motif: " + foundMotif.toString());*/
        }
    }
}