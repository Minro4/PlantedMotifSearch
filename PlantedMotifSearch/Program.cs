using System;
using PlantedMotifSearch.SequenceGeneration;

namespace PlantedMotifSearch
{
    class Program
    {
        private static readonly char[] alphabet = new[] {'A', 'C', 'G', 'T'};

        static void Main(string[] args)
        {
            var gen = new SequenceGenerator(alphabet);
            PmsAlgorithm algo = new AdaptiveHillClimbing(gen);

            //RunAlgo(algo, gen);
            TestOnce(algo, gen, 26, 11, 100);
            //TestMultiple(algo, gen);
        }

        static void RunAlgo(PmsAlgorithm algo, SequenceGenerator gen, int l = 26, int d = 11)
        {
            (var motif, var s) = gen.PlantedMotif(l, d, 20, 600);

            var foundMotif = new AdaptiveHillClimbing(gen).Search(s, l, d);

            Console.WriteLine("Motif: " + motif.toString());
            Console.WriteLine("Found motif: " + foundMotif.toString());
        }

        static void TestOnce(PmsAlgorithm algo, SequenceGenerator gen, int l = 26, int d = 11, int sampleSize = 10)
        {
            var (a, b) = new PmsAlgoTester(gen, algo).Test(l, d, sampleSize);
            Console.WriteLine("success: " + a);
            Console.WriteLine("time: " + b);
        }

        //Writes to an excel sheet
        static void TestMultiple(PmsAlgorithm algo, SequenceGenerator gen, string fileName = "test.xlsx")
        {
            new PmsAlgoTester(gen, algo).TestMultiple(13, 50, 4, 25, 10, fileName);
        }
    }
}