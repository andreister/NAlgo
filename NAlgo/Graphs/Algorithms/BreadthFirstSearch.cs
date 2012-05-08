using System;
using System.Collections.Generic;

namespace NAlgo.Graphs.Algorithms
{
	/// <summary>
	/// Implements a queue based BFS algorithm.
	/// </summary>
	/// <typeparam name="TId">Type of node identifier.</typeparam>
	/// <typeparam name="TValue">Type of the data associated with the node.</typeparam>
	public class BreadthFirstSearch<TId, TValue>
	{
		public bool Stop { get; set; }

		public void Run(Node<TId, TValue> startNode, Action<Node<TId, TValue>> process)
		{
			var queue = new Queue<Node<TId, TValue>>();
			queue.Enqueue(startNode);

			while (queue.Count != 0) {
				var node = queue.Dequeue();
				if (node.IsExplored) {
					continue;
				}

				foreach (var child in node.GetUnexploredChildren()) {
					queue.Enqueue(child);
				}

				node.IsExplored = true;
				process(node);

				if(Stop) {
					break;
				}
			}
		}
	}
}
