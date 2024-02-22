public class Node<T>
{
    public int Index { get; set; }
    public T Value { get; set; }
    public List<Node<T>> Neighbors { get; set; } = new List<Node<T>>();
    public List<int> Weights { get; set; } = new List<int>();

    public Node(int index, T value)
    {
        Index = index;
        Value = value;
    }
}
