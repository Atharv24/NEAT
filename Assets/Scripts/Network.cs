using System;
using System.Collections.Generic;

public class Network
{
    private List<Genome.NodeGene> nodeGenes;
    private Dictionary<int, Genome.ConnectionGene> connectionGenes;
    private List<Node> nodes;
    private int inputNodes;
    private int outputNodes;
    private float fitness;
	
    public Network(Genome genome, int input, int output)
    {
        nodeGenes = genome.GetNodes();
        connectionGenes = genome.GetConnections();
        inputNodes = input;
        outputNodes = output;
        MakeNetwork();
    }

    public class Node
    {
        int ID;
        float value;
        List<Connection> inConnections;
        List<Connection> outConnections;

        public Node(int id)
        {
            ID = id;
            value = 0f;
            inConnections = new List<Connection>();
            outConnections = new List<Connection>();
        }

        public int GetID()
        {
            return ID;
        }

        public void AddInConnection(Connection con)
        {
            inConnections.Add(con);
        }

        public void AddOutConnection(Connection con)
        {
            outConnections.Add(con);
        }

        public void SetValue(float val)
        {
            value = val;
        }

        public void CalculateValue()
        {
            foreach(Connection con in inConnections)
            {
                value += con.GetValue();
            }
        }

        public void TransmitValue()
        {
            foreach(Connection con in outConnections)
            {
                con.SetValue(value);
            }
        }
    }

    public class Connection
    {
        private float value;
        private float weight;
        private bool done;

        public Connection(float weight)
        {
            value = 0;
            done = false;
            this.weight = weight;
        }

        public float GetValue()
        {
            return value * weight;
        }

        public void SetValue(float val)
        {
            value = val;
            done = true;
        }

        public bool GetStatus()
        {
            return done;
        }

        public void Reset()
        {
            value = 0;
            done = false;
        }
    }

    public void MakeNetwork()
    {
        foreach (Genome.NodeGene nodeGene in nodeGenes)
        {
            nodes.Add(new Node(nodeGene.GetID()));
        }

        foreach (Node node in nodes)
        {
            foreach(Genome.ConnectionGene con in connectionGenes.Values)
            {
                if(con.GetOutNode() == node.GetID() && con.IsExpressed())
                {
                    node.AddInConnection(new Connection(con.GetWeight()));
                }
                else if(con.GetInNode() == node.GetID() && con.IsExpressed())
                {
                    node.AddOutConnection(new Connection(con.GetWeight()));
                }
            }
        }
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
