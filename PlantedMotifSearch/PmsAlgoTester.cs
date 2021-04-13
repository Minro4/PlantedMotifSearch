using System;
using System.Diagnostics;
using Aspose.Cells;
using PlantedMotifSearch.SequenceGeneration;

namespace PlantedMotifSearch
{
    public class PmsAlgoTester
    {
        private PmsAlgorithm algorithm;

        private SequenceGenerator generator;

        private int nbrSequences = 20;

        private int nbrCharacters = 600;

        public PmsAlgoTester(SequenceGenerator generator, PmsAlgorithm algorithm, int nbrSequences, int nbrCharacters)
        {
            this.algorithm = algorithm;
            this.generator = generator;
            this.nbrSequences = nbrSequences;
            this.nbrCharacters = nbrCharacters;
        }

        public void TestMultiple(int startL, int endL, int startD, int endD, int sampleSize, string fileName)
        {
            int lenL = endL - startL + 1;
            int lenD = endD - startD + 1;
            int[,] accuracy = new int[lenL + 1, lenD + 1];
            int[,] time = new int[lenL + 1, lenD + 1];

            //Add columns and row indicator
            for (int l = 0; l < lenL; l++)
            {
                accuracy[l + 1, 0] = l + startL;
                time[l + 1, 0] = l + startL;
            }

            for (int d = 0; d < lenD; d++)
            {
                accuracy[0, d + 1] = d + startD;
                time[0, d + 1] = d + startD;
            }

            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];

            for (int l = 0; l < lenL; l++)
            {
                for (int d = 0; d < lenD; d++)
                {
                    if (d + startD < l + startL)
                    {
                        var (a, t) = Test(l + startL, d + startD, sampleSize);
                        accuracy[l + 1, d + 1] = a;
                        time[l + 1, d + 1] = (int) t;

                        Console.WriteLine($"Done L: {l}; D: {d}");
                    }
                }


                worksheet.Cells.ImportArray(accuracy, 0, 0);
                worksheet.Cells.ImportArray(time, lenL + 5, 0);
                workbook.Save(l + fileName);
            }

            worksheet.Cells.ImportArray(accuracy, 0, 0);
            worksheet.Cells.ImportArray(time, lenL + 1, 0);
            workbook.Save(fileName);
        }

        public (int, long) Test(int l, int d, int sampleSize)
        {
            var successCount = 0;
            long avgTime = 0;

            for (int i = 0; i < sampleSize; i++)
            {
                //Console.WriteLine(i);
                var (motif, s) = generator.PlantedMotif(l, d, nbrSequences, nbrCharacters);

                var watch = new Stopwatch();
                watch.Start();
                var resp = algorithm.Search(s, l, d);
                watch.Stop();
                avgTime += watch.ElapsedMilliseconds / sampleSize;

                //if (resp.MotifDistance(s) <= d)
                if (resp != null)
                    successCount++;
            }

            return (successCount, avgTime);
        }
    }
}
