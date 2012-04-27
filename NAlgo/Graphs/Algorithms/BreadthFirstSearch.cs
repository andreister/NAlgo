using System;
using System.Collections.Generic;

namespace NAlgo.Graphs.Algorithms
{
	/// <summary>
	/// Implements a queue based BFS algorithm.
	/// </summary>
	public class BreadthFirstSearch<T>
	{
		public void Run(Node<T> root, Action<Node<T>> process)
		{
			var queue = new Queue<Node<T>>();
			queue.Enqueue(root);

			while (queue.Count != 0) {
				var node = queue.Dequeue();
				process(node);
			}
		}
	}
}
