using System;
using PlantedMotifSearch.SequenceGeneration;

namespace PlantedMotifSearch
{
    class Program
    {
        private static readonly char[] alphabet = new[] {'A', 'C', 'G', 'T'};
        private static readonly int l = 26;
        private static readonly int d = 11;

        static void Main(string[] args)
        {
            var gen = new SequenceGenerator(alphabet);
            (var motif, var s) = gen.PlantedMotif(l, d, 20, 60);


            (var foundMotif, var allo) = new HillClimbing(gen).Search(s, l, d);

            Console.WriteLine("Motif: " + motif.toString());
            Console.WriteLine("Found motif: " + foundMotif.toString());
        }
    }
}