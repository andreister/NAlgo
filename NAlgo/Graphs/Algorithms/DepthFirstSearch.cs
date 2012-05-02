using System;
using System.Collections.Generic;
using System.Linq;

namespace NAlgo.Graphs.Algorithms
{
	/// <summary>
	/// Implements an iterative version of DFS algorithm.
	/// </summary>
	public class DepthFirstSearch<T>
	{
		public void Run(Node<T> startNode, Func<Node<T>, IEnumerable<Node<T>>> getUnexploredChildren, Action<Node<T>> process)
		{
			var stack = new Stack<Node<T>>();
			stack.Push(startNode);

			while (stack.Count != 0) {
				var node = stack.Pop();
				node.IsExplored = true;

				var unexploredChild = getUnexploredChildren(node).FirstOrDefault();
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
