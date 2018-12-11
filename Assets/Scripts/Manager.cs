using System;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private List<Genome> genomes;
    private List<Network> nets;
    private Dictionary<Genome, Network> networkMap;
    private Dictionary<Genome, Species> speciesMap;
    private List<Species> speciesList;
    private int generation;
    public int population;
    public int inputNodes;
    public int outputNodes;
    public const float C1 = 1f;
    public const float C2 = 1f;
    public const float C3 = 0.3f;
    public const float compatiblityThreshold = 3f;
    public const float survivalChance = 0.1f;
    public const float weightMutationChance = 0.8f;
    public const float randomWeightChance = 0.1f;
    public const float addNodeChance = 0.03f;
    public const float addConnectionChance = 0.05f;


    void Start ()
    {
        generation = 1;
        genomes = new List<Genome>();
        speciesList = new List<Species>();
        for(int i = 0; i<population; i++)
        {
            Genome genome = new Genome(inputNodes, outputNodes);
            genomes.Add(genome);
        }
	}
	
	
	void Update ()
    {
        AssignSpecies();
        MakeNetworks();
        //Assign nets to players
        //Let game play
        //check if game ended
        SortNets();
        NextGen();
    }

    private void AssignSpecies()
    {
        speciesMap = new Dictionary<Genome, Species>();
        foreach (Genome gen in genomes)
        {
            bool found = false;
            foreach (Species species in speciesList)
            {
                if(GenomeUtils.CompatiblityDistance(gen, species.GetMascot(), C1, C2, C3) < compatiblityThreshold)
                {
                    species.AddMember(gen);
                    speciesMap.Add(gen, species);
                    found = true;
                    break;
                }
            }

            if(!found)
            {
                Species species = new Species(gen);
                speciesList.Add(species);
                speciesMap.Add(gen, species);
            }

        }
    }

    private void MakeNetworks()
    {
        nets = new List<Network>();
        networkMap = new Dictionary<Genome, Network>();

        foreach (Genome genome in genomes)
        {
            Network net = new Network(genome);
            nets.Add(net);
            networkMap.Add(genome, net);
        }


    }

    private void SortNets()
    {
        foreach (Network net in nets)
        {
            net.SetFitness(net.GetFitness()/speciesMap[net.GetGenome()].GetCount());
            speciesMap[net.GetGenome()].AddFitness(net.GetFitness());
        }

        nets.Sort();
    }

    private void NextGen()
    {
        generation++;
        float totalFitness = 0;
        float leftPopulation = population * (1 - survivalChance);
        List<Genome> nextGenomes = new List<Genome>();

        foreach (Species species in speciesList)
        {
            totalFitness += species.GetFitness();
        }

        for(int i=0; i<population*survivalChance; i++)
        {
            nextGenomes.Add(nets[i].GetGenome());
        }

        foreach(Species species in speciesList)
        {
            for(int i=0; i<species.GetFitness()*leftPopulation/totalFitness; i++)
            {
                Genome parent1 = species.GetRandomGenome();
                Genome parent2 = species.GetRandomGenome();
                Genome child = new Genome();

                if(networkMap[parent1].GetFitness()> networkMap[parent2].GetFitness())
                {
                    child = GenomeUtils.Crossover(parent1, parent2);
                }
                else
                {
                    child = GenomeUtils.Crossover(parent2, parent1);
                }
                
                nextGenomes.Add(child);
            }
        }

        foreach(Genome genome in nextGenomes)
        {
            System.Random r = new System.Random();
            double roll = r.NextDouble();

            if(roll<weightMutationChance)
            {
                genome.Mutate(randomWeightChance);
            }
            else if(roll<weightMutationChance + addNodeChance)
            {
                genome.AddNodeMutation();
            }
            else if(roll< weightMutationChance + addNodeChance + addConnectionChance)
            {
                genome.AddConnectionMutation();
            }
        }

        foreach(Species species in speciesList)
        {
            species.Reset();
        }

        genomes = nextGenomes;
    }

    public class Species
    {
        private Genome mascot;
        private List<Genome> members;
        private float fitness;

        public Species(Genome firstMember)
        {
            members = new List<Genome>
            {
                firstMember
            };
            mascot = firstMember;
            fitness = 0;
        }

        public Genome GetRandomGenome()
        {
            System.Random r = new System.Random();
            return members[r.Next(members.Count)];
        }

        public void AddMember(Genome genome)
        {
            members.Add(genome);
        }

        public void AddFitness(float fit)
        {
            fitness += fit;
        }

        public float GetFitness()
        {
            return fitness;
        }

        public int GetCount()
        {
            return members.Count;
        }

        public Genome GetMascot()
        {
            return mascot;
        }

        public void Reset()
        {
            members.Clear();
            fitness = 0;
        }

    }
}
