using System;
using System.Collections.Generic;

namespace NAlgo.Graphs.Algorithms
{
	/// <summary>
	/// Implements a queue based BFS algorithm.
	/// </summary>
	public class BreadthFirstSearch<T>
	{
		public bool Stop { get; set; }

		public void Run(Node<T> startNode, Func<Node<T>, IEnumerable<Node<T>>> getUnexploredChildren, Action<Node<T>> process)
		{
			var queue = new Queue<Node<T>>();
			queue.Enqueue(startNode);

			while (queue.Count != 0) {
				var node = queue.Dequeue();
				if (node.IsExplored) {
					continue;
				}

				foreach (var child in getUnexploredChildren(node)) {
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
