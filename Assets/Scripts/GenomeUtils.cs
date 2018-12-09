using System;
using System.Collections.Generic;

public class GenomeUtils
{
    private const float c1 = 1f;
    private const float c2 = 1f;
    private const float c3 = 1f;

    public static float CompatiblityDistance(Genome g1, Genome g2)
    {
        float distance;
        int[] disjointExcess = DisjointAndExcess(g1.GetConnections(), g2.GetConnections(), g1.GetMaxInnovation(), g2.GetMaxInnovation());
        int disjointGenes = disjointExcess[0];
        int excessGenes = disjointExcess[1];
        float avgWeightDifference = AverageWeightDifference(g1.GetConnections(), g2.GetConnections());

        distance = c1 * excessGenes + c2 * disjointGenes + avgWeightDifference;

        return distance;
    }

    public static int[] DisjointAndExcess(Dictionary<int, Genome.ConnectionGene> d1, Dictionary<int, Genome.ConnectionGene> d2, int maxInno1, int maxInno2)
    {
        int disjointGenes = 0;
        int excessGenes = 0;
        
        int checkUpto = (maxInno1 > maxInno2 ? maxInno2 : maxInno1);
        foreach(Genome.ConnectionGene con in d1.Values)
        {
            if(con.GetInnovation()<=checkUpto)
            {
                disjointGenes += d2.ContainsKey(con.GetInnovation()) ? 0 : 1;
            }

            else
            {
                excessGenes += d2.ContainsKey(con.GetInnovation()) ? 0 : 1;
            }
        }

        foreach (Genome.ConnectionGene con in d2.Values)
        {
            if (con.GetInnovation() <= checkUpto)
            {
                disjointGenes += d1.ContainsKey(con.GetInnovation()) ? 0 : 1;
            }

            else
            {
                excessGenes += d1.ContainsKey(con.GetInnovation()) ? 0 : 1;
            }
        }

        int[] number = { disjointGenes, excessGenes };
        return number;
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
