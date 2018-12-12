using System;
using System.Collections.Generic;

public class Species : IComparable<Species>
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

    public Genome GetRandomGenome(Random r)
    {
        return members[r.Next(members.Count)];
    }

    public void RandomizeMascot(Random r)
    {
        mascot = members[r.Next(members.Count)];
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

    public int CompareTo(Species other)
    {
        return other.GetFitness().CompareTo(fitness);
    }
}
