using System;
using PlantedMotifSearch.SequenceGeneration;

namespace PlantedMotifSearch
{
    class Program
    {
        private static readonly char[] alphabet = new[] {'A', 'C', 'G', 'T'};

        static void Main(string[] args)
        {
            Algo();
            //TestOnce();
            //TestMultiple();
        }

        static void Algo(int l = 26, int d = 11)
        {
            var gen = new SequenceGenerator(alphabet);

            (var motif, var s) = gen.PlantedMotif(l, d, 20, 600);


            var foundMotif = new AdaptiveHillClimbing(gen).AdaptiveSearch(s, l, d);

            Console.WriteLine("Motif: " + motif.toString());
            Console.WriteLine("Found motif: " + foundMotif.toString());
        }

        static void TestOnce(int l = 26, int d = 11, int sampleSize = 10)
        {
            var gen = new SequenceGenerator(alphabet);
            var (a, b) = new PmsAlgoTester(gen, new AdaptiveHillClimbing(gen)).Test(l, d, sampleSize);
            Console.WriteLine("success: " + a);
            Console.WriteLine("time: " + b);
        }

        //Writes to an excel sheet
        static void TestMultiple()
        {
            var gen = new SequenceGenerator(alphabet);
            new PmsAlgoTester(gen, new AdaptiveHillClimbing(gen)).TestMultiple(13, 50, 4, 25, 10, "test.xlsx");
        }
    }
}