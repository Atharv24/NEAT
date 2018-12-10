using System;
using System.Collections.Generic;

public class Network
{
    private List<Genome.NodeGene> nodes;
    private Dictionary<int, Genome.ConnectionGene> connections;
    private float fitness;
	
    public Network(Genome genome)
    {
        nodes = genome.GetNodes();
        connections = genome.GetConnections();

    }

    public float GetFitness()
    {
        return fitness;
    }

    public void SetFitness(float fit)
    {
        fitness = fit;
    }

    public void AddFitness(float fit)
    {
        fitness += fit;
    }

}
