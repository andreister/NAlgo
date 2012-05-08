using System;
using System.Collections.Generic;
using System.Linq;

namespace NAlgo.Graphs.Algorithms
{
	/// <summary>
	/// Implements an iterative version of DFS algorithm.
	/// </summary>
	/// <typeparam name="TId">Type of node identifier.</typeparam>
	/// <typeparam name="TValue">Type of the data associated with the node.</typeparam>
	public class DepthFirstSearch<TId, TValue>
	{
		public void Run(Node<TId, TValue> startNode, Action<Node<TId, TValue>> process)
		{
			var stack = new Stack<Node<TId, TValue>>();
			stack.Push(startNode);

			while (stack.Count != 0) {
				var node = stack.Pop();
				node.IsExplored = true;

				var unexploredChild = node.GetUnexploredChildren().FirstOrDefault();
				if (unexploredChild != null) {
					unexploredChild.IsExplored = true;
					stack.Push(node);				//push the node because there may be more unexplored children
					stack.Push(unexploredChild);	//push the unexplored child to DFS it on the next step
					continue;
				}

				process(node);
			}
		}
	}
}
