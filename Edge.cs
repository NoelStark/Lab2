public class Edge<T>
{
    public Node<T> Source { get; set; }
    public Node<T> Target { get; set; }
    public int Weight { get; set; }

    public Edge(Node<T> source, Node<T> target, int weight)
    {
        Source = source;
        Target = target;
        Weight = weight;
    }
}
