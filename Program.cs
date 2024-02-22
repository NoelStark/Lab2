using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using Priority_Queue; // Include for Stopwatch

class Program
{
    static PerformanceMeasurement performance = new PerformanceMeasurement();
    static Node<char> n1, n2, n3, n4,n5, n6, n7,n8;

    static void FourNodeGraph(Graph<char> graph)
    {
        //Adding nodes to the graph with character identifiers
        n1 = graph.AddNode('A');
        n2 = graph.AddNode('B');
        n3 = graph.AddNode('C');
        n4 = graph.AddNode('D');

        // Adding edges between nodes
        graph.AddEdge(n1, n2, 2);
        graph.AddEdge(n1, n3, 5);
        graph.AddEdge(n2, n3, 1);
        graph.AddEdge(n2, n4, 4);
        graph.AddEdge(n3, n4, 3);
    }
    
    static void EightNodeGraph(Graph<char> graph)
    {
        //Adding nodes to the graph with character identifiers
        n1 = graph.AddNode('A');
        n2 = graph.AddNode('B');
        n3 = graph.AddNode('C');
        n4= graph.AddNode('D');
        n5= graph.AddNode('E');
        n6= graph.AddNode('F');
        n7= graph.AddNode('G');
        n8= graph.AddNode('H');

        // Adding edges between nodes
        graph.AddEdge(n1, n2, 9);
        graph.AddEdge(n1, n3, 2);
        graph.AddEdge(n2, n4, 1);
        graph.AddEdge(n3, n4, 5);
        graph.AddEdge(n3, n5, 2);
        graph.AddEdge(n4, n5, 6);
        graph.AddEdge(n4, n6, 2);
        graph.AddEdge(n5, n7, 5);
        graph.AddEdge(n6, n7, 1);
        graph.AddEdge(n7, n8, 1);
        graph.AddEdge(n8, n5, 3);

    }
    static void Main(string[] args)
    {
        // Initialize a graph
        Graph<char> graph = new Graph<char>();

        // Prompt the user to choose the size of the graph
        chooseGraph:
        Console.WriteLine("Choose size of graph:\n" + "1. Four Node graph\n" + "2. Eight Node Graph");
        string answer = Console.ReadLine();

        // Depending on user input, generate a graph
        if (answer == "1")
            FourNodeGraph(graph);
        else if(answer == "2")
            EightNodeGraph(graph);
        else
            goto chooseGraph;
        Console.Clear();

        // Continuously prompt the user for graph algorithm operations
        while (true)
        {
            Console.Clear();
            Console.WriteLine("----Measuring Graph Algorithms----\n");
            Console.WriteLine("1. List of Edge Directions");
            Console.WriteLine("2. Shortest Path between two Nodes");
            Console.WriteLine("3. Adjacency Matrix");
            Console.WriteLine("4. Dijkstra's Algorithm");
            Console.WriteLine("5. Floyd's Algorithm");
            Console.WriteLine("--------------------");
            Console.Write("Input (0 to exit): ");


            // Get user input
            bool input = int.TryParse(Console.ReadLine(), out int result);
            Console.WriteLine();
            if (input)
            {
                switch (result)
                {
                    case 1:
                        // Print the list of edges with weights
                        Console.WriteLine("Print the list of edges with weights");
                        graph.PrintEdges();
                        break;
                    case 2:
                        // Find the shortest path between two nodes and print it
                        List<Edge<char>> path = graph.GetShortestPathDijkstra(n1,  n4);

                        Console.WriteLine("Shortest path from A to D:");
                        foreach (Edge<char> edge in path)
                        {
                            Console.WriteLine($"{edge.Source.Value} -> {edge.Target.Value} with weight {edge.Weight}");
                        }
                        break;
                    case 3:
                        // Print the adjacency matrix
                        Console.WriteLine("Print the adjacency matrix");
                        graph.PrintAdjacencyMatrix();
                        break;
                    case 4:
                        // Run Dijkstra's algorithm and measure performance
                        performance.Start();
                        double[,] dijkstraMatrix = graph.GetShortestPathDijkstraMatrix();
                        performance.Dispose();
                        graph.PrintMatrix(dijkstraMatrix);
                        Console.WriteLine($"Wall-clock time: {performance.ElapsedWallClockTime.ToString("s\\.ffffff")} seconds");
                        Console.WriteLine($"CPU time: {performance.ElapsedCpuTime.ToString("s\\.ffff")} seconds");
                        
                        break;
                    case 5:
                        // Run Floyd's algorithm and measure performance
                        performance.Start();
                        double[,] floydMatrix = graph.GetShortestPathFloyd();
                        performance.Dispose();
                        graph.PrintMatrix(floydMatrix);

                        Console.WriteLine($"Wall-clock time: {performance.ElapsedWallClockTime.ToString("s\\.ffffff")} seconds");
                        Console.WriteLine($"CPU time: {performance.ElapsedCpuTime.ToString("s\\.ffff")} seconds");

                        break;
                    case 0:
                        // Exit the program
                        Environment.Exit(0);
                        break;
                }

                Console.WriteLine("--------------------");
                Console.ReadLine();
            }
        }
    }

}
