using System.Collections.Generic;

namespace PlantedMotifSearch
{
    public interface PmsAlgorithm
    {
        Sequence Search(List<Sequence> sequences, int l, int d);
    }
}