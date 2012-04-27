using System;
using System.Collections.Generic;

namespace NAlgo.Graphs.Algorithms
{
	/// <summary>
	/// Implements an iterative version of DFS algorithm.
	/// </summary>
	public class DepthFirstSearch<T>
	{
		public void Run(Node<T> root, Action<Stack<Node<T>>, Node<T>> process)
		{
			var stack = new Stack<Node<T>>();
			stack.Push(root);

			while (stack.Count != 0) {
				var node = stack.Pop();
				process(stack, node);
			}
		}
	}
}
