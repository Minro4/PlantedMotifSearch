using System;
using System.Diagnostics;
using Aspose.Cells;
using PlantedMotifSearch.SequenceGeneration;

namespace PlantedMotifSearch
{
    public class Accuracy
    {
        private PmsAlgorithm algorithm;
        private SequenceGenerator generator;

        public Accuracy(SequenceGenerator generator, PmsAlgorithm algorithm)
        {
            this.algorithm = algorithm;
            this.generator = generator;
        }

        public void TestMultiple(int startL, int endL, int startD, int endD, int sampleSize, string fileName)
        {
            int lenL = endL - startL + 1;
            int lenD = endD - startD + 1;
            int[,] accuracy = new int[lenL, lenD];
            int[,] time = new int[lenL, lenD];

            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];

            for (int l = 0; l < lenL; l++)
            {
                for (int d = 0; d < lenD; d++)
                {
                    if (d + startD < l + startL)
                    {
                        var (a, t) = Test(l + startL, d + startD, sampleSize);
                        accuracy[l, d] = a;
                        time[l, d] = (int) t;
                    }
                }


                worksheet.Cells.ImportArray(accuracy, 0, 0);
                worksheet.Cells.ImportArray(time, lenL + 1, 0);
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
                Console.WriteLine(i);
                var (motif, s) = generator.PlantedMotif(l, d, 20, 600);

                var watch = new Stopwatch();
                watch.Start();
                var resp = algorithm.Search(s, l, d);
                watch.Stop();
                avgTime += watch.ElapsedMilliseconds / sampleSize;

                if (resp.MotifDistance(s) <= d)
                    successCount++;
            }

            return (successCount, avgTime);
        }
    }
}