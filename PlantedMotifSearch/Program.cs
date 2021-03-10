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
            // new Accuracy(gen, new AdaptiveHillClimbing(gen)).TestMultiple(13, 50, 4, 25, 10, "test.xlsx");
            var (a, b) = new Accuracy(gen, new AdaptiveHillClimbing(gen)).Test(26, 11, 10);
            Console.WriteLine("success: " + a);
            Console.WriteLine("time: " + b);
            /*  (var motif, var s) = gen.PlantedMotif(l, d, 20, 600);
  
  
              var foundMotif = new AdaptiveHillClimbing(gen).AdaptiveSearch(s, l, d);
  
              Console.WriteLine("Motif: " + motif.toString());
              Console.WriteLine("Found motif: " + foundMotif.toString());*/
        }
    }
}