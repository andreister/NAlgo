namespace NAlgo.Graphs
{
	/// <summary>
	/// Generic node in a graph.
	/// </summary>
	/// <typeparam name="T">Type of the node identifier (int, string, etc)</typeparam>
	public abstract class Node<T>
	{
		/// <summary>
		/// Unique identifier of the node (number, name, etc).
		/// </summary>
		public T Id { get; set; }

		/// <summary>
		/// Flag used by traversal algorithms to set or determine whether the node had been already visited.
		/// </summary>
		internal bool IsExplored { get; set; }

		/// <summary>
		/// Creates a new node.
		/// </summary>
		/// <param name="id">Unique identifier of the node (number, name, etc).</param>
		protected Node(T id)
		{
			Id = id;
		}
	}
}
