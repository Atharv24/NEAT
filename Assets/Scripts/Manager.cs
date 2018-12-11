using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private List<Genome> genomes;
    private List<Network> nets;
    private List<Species> speciesList;
    private int generation;
    public int population;
    public int inputNodes;
    public int outputNodes;
    public float[] parameters;                  //c1, c2, c3, Compatiblity Threshold


    void Start ()
    {
        generation = 1;
        genomes = new List<Genome>();
        nets = new List<Network>();
        speciesList = new List<Species>();
        for(int i = 0; i<population; i++)
        {
            Genome genome = new Genome(inputNodes, outputNodes);
            genomes.Add(genome);
            nets.Add(new Network(genome, inputNodes, outputNodes));
        }
	}
	
	
	void Update ()
    {
        AssignSpecies();
        //Assign nets to players
        //Let game play
        //check if game ended
        //sort nets by fitness
        //make new generation
    }

    private void AssignSpecies()
    {
        foreach(Genome gen in genomes)
        {
            bool found = false;
            foreach (Species species in speciesList)
            {
                if(GenomeUtils.CompatiblityDistance(gen, species.GetMascot(), parameters[0], parameters[1], parameters[2]) <= parameters[3])
                {
                    species.AddMember(gen);
                    found = true;
                    break;
                }
            }

            if(!found)
            {
                Species species = new Species(gen);
                speciesList.Add(species);
            }

        }
    }

    public class Species
    {
        private Genome mascot;
        private List<Genome> members;

        public Species(Genome firstMember)
        {
            members = new List<Genome>();
            members.Add(firstMember);
            mascot = firstMember;
        }

        public void AddMember(Genome genome)
        {
            members.Add(genome);
        }

        public Genome GetMascot()
        {
            return mascot;
        }

    }
}
