using System;
using PlantedMotifSearch.SequenceGeneration;

namespace PlantedMotifSearch
{
    class Program
    {
        private static readonly char[] alphabet = new[]
        {
            'A', 'C', 'G', 'T'
        };

        private static readonly int nbrSequences = 20;

        private static readonly int nbrCharacters = 600;

        static void Main(string[] args)
        {
            var gen = new SequenceGenerator(alphabet);
            PmsAlgorithm algo = new AdaptiveHillClimbing(gen);

            RunAlgo(algo, gen);
            //TestOnce(algo, gen, 26, 11, 25);
            //TestMultiple(algo, gen, "newTest.xlsx");
        }

        static void RunAlgo(PmsAlgorithm algo, SequenceGenerator gen, int l = 26, int d = 11)
        {
            (var motif, var s) = gen.PlantedMotif(l, d, nbrSequences, nbrCharacters);

            var foundMotif = algo.Search(s, l, d);

            Console.WriteLine("Motif: " + motif.toString());
            Console.WriteLine("Found motif: " + foundMotif.toString());
        }

        static void TestOnce(PmsAlgorithm algo, SequenceGenerator gen, int l = 26, int d = 11, int sampleSize = 10)
        {
            var (a, b) = new PmsAlgoTester(gen, algo, nbrSequences, nbrCharacters).Test(l, d, sampleSize);
            Console.WriteLine("success: " + a);
            Console.WriteLine("time: " + b);
        }

        //Writes to an excel sheet
        static void TestMultiple(PmsAlgorithm algo, SequenceGenerator gen, string fileName = "test.xlsx")
        {
            new PmsAlgoTester(gen, algo, nbrSequences, nbrCharacters).TestMultiple(13, 50, 4, 25, 10, fileName);
        }
    }
}
