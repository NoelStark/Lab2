using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Xml.Linq;
using Priority_Queue;

public class Graph<T>
{
    public List<Node<T>> Nodes { get; set; } = new List<Node<T>>();

    public Graph() { }

    public Node<T> AddNode(T value)
    {
        Node<T> node = new Node<T>(Nodes.Count, value);
        Nodes.Add(node);
        return node;
    }

    public void AddEdge(Node<T> source, Node<T> target, int weight)
    {
        source.Neighbors.Add(target);
        source.Weights.Add(weight);
    }

    // Method to fill an array with a specific value
    private void Fill(int[] array, int value)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = value;
        }
    }

//1. Listing Edge Directions and Their Weights
// Method to print the list of edge directions and their weights
public void PrintEdges()
{
    // Print header
    Console.WriteLine("List of Edge Directions and Their Weights:");
    
    // Iterate over each node in the graph
    foreach (var node in Nodes)
    {
        // Iterate over each neighbor of the current node
        foreach (var neighbor in node.Neighbors)
        {
            // Retrieve the index of the current neighbor
            int index = node.Neighbors.IndexOf(neighbor);
            // Retrieve the weight of the edge between the current node and its neighbor
            int weight = node.Weights[index];
            // Print the edge direction and its weight
            Console.WriteLine($"{node.Value} -> {neighbor.Value} with weight {weight}");
        }
    }
}


// Method to generate and print the adjacency matrix
public void PrintAdjacencyMatrix()
{
    // Get the number of nodes in the graph
    int n = Nodes.Count;
    // Initialize a 2D array to store the adjacency matrix
    double[,] matrix = new double[n, n];

    // Initialize the matrix with infinity to indicate no direct path between nodes
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            if(i != j)
                matrix[i, j] = double.PositiveInfinity;
        }
    }

    // Fill the matrix with edge weights where applicable
    for (int i = 0; i < n; i++)
    {
        Node<T> node = Nodes[i];
        foreach (var neighbor in node.Neighbors)
        {
            int j = Nodes.IndexOf(neighbor);
            int weightIndex = node.Neighbors.IndexOf(neighbor);
            // Update the matrix with the weight of the edge from node i to node j
            matrix[i, j] = node.Weights[weightIndex];
        }
    }

    // Print the adjacency matrix
    Console.WriteLine("Adjacency Matrix:");
    Console.Write("   ");
    // Print column labels (node values)
    foreach (var node in Nodes)
    {
        Console.Write($"{node.Value}  ");
    }
    Console.WriteLine();

    // Print rows of the matrix along with node labels and edge weights
    for (int i = 0; i < n; i++)
    {
        // Print the label of the current node
        Console.Write($"{Nodes[i].Value}  ");
        for (int j = 0; j < n; j++)
        {
            // Check if there is a direct path from node i to node j
            if (matrix[i, j] == double.PositiveInfinity)
                Console.Write("\u221e  "); // Print "Inf" for infinity if no direct path
            else
                Console.Write($"{matrix[i, j]:0.##}  "); // Print edge weight if a direct path exists
        }
        Console.WriteLine(); // Move to the next row of the matrix
    }
}

 // Method to find the shortest path between two nodes using Dijkstra's algorithm
public List<Edge<T>> GetShortestPathDijkstra(Node<T> source, Node<T> target)
{
    // Array to store the indices of the previous nodes in the shortest path
    int[] previous = new int[Nodes.Count];
    Fill(previous, -1); // Initialize with -1 to indicate no previous node

    // Array to store the distances from the source node to all other nodes
    int[] distances = new int[Nodes.Count];
    Fill(distances, int.MaxValue); // Initialize with maximum integer value

    // Set distance to source node as 0
    distances[source.Index] = 0;

    // Priority queue to store nodes and their distances from the source
    SimplePriorityQueue<Node<T>> nodes = new SimplePriorityQueue<Node<T>>();

    // Enqueue all nodes with their respective distances
    for (int i = 0; i < Nodes.Count; i++)
    {
        nodes.Enqueue(Nodes[i], distances[i]);
    }

    // Dijkstra's algorithm to find shortest path
    while (nodes.Count != 0)
    {
        // Dequeue node with minimum distance
        Node<T> node = nodes.Dequeue();
        // Iterate through neighbors of the dequeued node
        foreach (var neighbor in node.Neighbors)
        {
            int index = neighbor.Index; // Index of the neighbor node
            int weight = node.Weights[node.Neighbors.IndexOf(neighbor)]; // Weight of the edge
            int totalWeight = distances[node.Index] + weight; // Total weight from source to neighbor
            // Update distance if shorter path is found
            if (totalWeight < distances[index])
            {
                distances[index] = totalWeight;
                previous[index] = node.Index; // Update previous node
                if (nodes.Contains(neighbor))
                {
                    nodes.UpdatePriority(neighbor, totalWeight); // Update priority in the queue

                }
            }
        }
    }

    // Path reconstruction
    List<int> indices = new List<int>(); // List to store indices of nodes in the shortest path
    for (int at = target.Index; at != -1; at = previous[at])
    {
        indices.Add(at); // Add node index to the list
    }
    indices.Reverse(); // Reverse the list to get the correct order of nodes in the path

    // Return an empty list if there's no path from source to target
    if (indices.Count == 1 && indices[0] == target.Index && source.Index != target.Index)
    {
        return new List<Edge<T>>();
    }

    // Create a list to store edges in the shortest path
    List<Edge<T>> path = new List<Edge<T>>();
    for (int i = 0; i < indices.Count - 1; i++)
    {
        // Create an edge between consecutive nodes in the path
        Node<T> nodeSource = Nodes[indices[i]];
        Node<T> nodeTarget = Nodes[indices[i + 1]];
        // Calculate the weight of the edge
        int edgeWeight = distances[nodeTarget.Index] - distances[nodeSource.Index];
        Edge<T> edge = new Edge<T>(nodeSource, nodeTarget, edgeWeight);
        path.Add(edge); // Add edge to the path list
    }

    return path; // Return the shortest path as a list of edges
}

// Dijkstra Shortest Path Matrix
// This method computes the shortest path matrix using Dijkstra's algorithm.
// It calculates the shortest paths between all pairs of nodes in the graph.
public double[,] GetShortestPathDijkstraMatrix()
{
    int n = Nodes.Count; // Number of nodes in the graph
    double[,] shortestPathMatrix = new double[n, n]; // Matrix to store shortest path distances between all pairs of nodes
    double[] distances = new double[n];

    foreach (var sourceNode in Nodes)
    {
        Dijkstra(sourceNode, shortestPathMatrix, n, distances);
    }

    return shortestPathMatrix; // Return the matrix containing shortest paths between all node pairs
}

private void Dijkstra(Node<T> sourceNode, double[,] shortestPathMatrix, int n, double[] distances)
{

    // Initialize priority queue for Dijkstra's algorithm
    var priorityQueue = new PriorityQueue<Node<T>, int>();

    // Initialize distances array with positive infinity
    for (int i = 0; i < n; i++)
    {
        distances[i] = double.PositiveInfinity;
    }
    // Set distance from source to itself as 0 and enqueue the source node
    distances[sourceNode.Index] = 0;
    priorityQueue.Enqueue(sourceNode, 0);

    // Dijkstra's algorithm loop
    while (priorityQueue.Count > 0)
    {
        var currentNode = priorityQueue.Dequeue();
        // Iterate through neighbors of the current node
        foreach (var neighbor in currentNode.Neighbors)
        {
            // Calculate  distance to neighbor through the current node
            double newDistance = distances[currentNode.Index] + currentNode.Weights[currentNode.Neighbors.IndexOf(neighbor)];

            // Update distance if shorter path found
            if (newDistance < distances[neighbor.Index])
            {
                distances[neighbor.Index] = newDistance;
                priorityQueue.Enqueue(neighbor, currentNode.Index);
            }
        }
    }
    // Store the shortest distances from the source node to all other nodes
    for (int i = 0; i < n; i++)
    {
        shortestPathMatrix[sourceNode.Index, i] = distances[i];
    }
}


//Floyd's Algorithm
public double[,] GetShortestPathFloyd()
{

    int n = Nodes.Count;
    double[,] dist = new double[n, n];


    // Initialize distance matrix with positive infinity
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            if(i != j)
                dist[i,j] = double.PositiveInfinity;
        }
    }

    // Populate distance matrix with direct edge weights
    for (int i = 0; i < n; i++)
    {
        foreach (var neighbor in Nodes[i].Neighbors)
        {
            // Update distance only if the neighbor index is valid
            if (neighbor.Index < n)
            {
                dist[i, neighbor.Index] = Nodes[i].Weights[Nodes[i].Neighbors.IndexOf(neighbor)];
            }
        }
    }

    double distance;
    // Floyd's algorithm triple loop
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            for (int k = 0; k < n; k++)
            {
                // Update distance if shorter path found through intermediate node
                distance = dist[i, j] + dist[j, k];
                if(distance < dist[i,k])
                        dist[i, k] = distance;
            }
        }
    }

    return dist;
}


//Print matrix method including headers are the header row

public void PrintMatrix(double[,] matrix)
    {
        
        Console.Write("   "); // Leading tab for alignment with row labels
        foreach (var node in this.Nodes)
        {
            Console.Write($"{node.Value}  ");
        }
        Console.WriteLine(); // End of the header row
        

        for (int i = 0; i < this.Nodes.Count; i++)
        { 
            // Print the row label (node identifier) if headers are needed
            Console.Write($"{this.Nodes[i].Value}  ");
            
            for (int j = 0; j < this.Nodes.Count; j++)
            {
                if (matrix[i, j] == double.PositiveInfinity)
                    Console.Write("\u221e  ");
                else
                    Console.Write($"{matrix[i, j]:0.##}  ");
            }
            Console.WriteLine(); // New line for the next row
        }
    }




}
