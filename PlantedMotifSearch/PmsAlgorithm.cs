using System.Collections.Generic;

namespace PlantedMotifSearch
{
    public interface PmsAlgorithm
    {
        (Sequence, int[]) Search(List<Sequence> sequences, int l, int d);
    }
}