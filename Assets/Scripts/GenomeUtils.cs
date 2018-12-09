using System;
using System.Collections.Generic;

public class GenomeUtils
{
    private const float c1 = 1f;
    private const float c2 = 1f;
    private const float c3 = 1f;

    public static float CompatiblityDistance(Genome g1, Genome g2)
    {
        Dictionary<int, Genome.ConnectionGene> d1 = g1.GetConnections();
        Dictionary<int, Genome.ConnectionGene> d2 = g2.GetConnections();

        return c3 * AverageWeightDifference(d1, d2);
    }

    public static float AverageWeightDifference(Dictionary<int, Genome.ConnectionGene> d1, Dictionary<int, Genome.ConnectionGene> d2)
    {
        int matchingGenes = 0;
        float weightDifference = 0f;
        foreach (Genome.ConnectionGene con1 in d1.Values)
        {
            if(d2.ContainsKey(con1.GetInnovation()))
            {
                matchingGenes++;
                weightDifference += Math.Abs(con1.GetWeight() - d2[con1.GetInnovation()].GetWeight());
            }
        }
        float avgWeightDifference = weightDifference / matchingGenes;
        return avgWeightDifference;
    }

}
