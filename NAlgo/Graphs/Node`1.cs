namespace NAlgo.Graphs
{
	/// <summary>
	/// Generic node in a graph.
	/// </summary>
	/// <typeparam name="T">Type of the node identifier (int, string, etc)</typeparam>
	public class Node<T>
	{
		public T Id { get; set; }

		public Node(T id)
		{
			Id = id;
		}
	}
}
