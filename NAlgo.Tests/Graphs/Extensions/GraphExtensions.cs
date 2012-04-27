using System;
using System.Collections.Generic;
using NAlgo.Graphs;

namespace NAlgo.Tests.Graphs.Extensions
{
	internal static class GraphExtensions
	{
		internal static Dictionary<int, DirectedGraphNode<int>> ToDirectedGraph(this string graphText)
		{
			var graph = new Dictionary<int, DirectedGraphNode<int>>();

			foreach (var edge in graphText.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)) {
				var nodes = edge.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
				int fromId;
				int toId;
				if (nodes.Length != 2 || !int.TryParse(nodes[0], out fromId) || !int.TryParse(nodes[1], out toId)) {
					throw new ArgumentException("Invalid input for an edge: " + edge);
				}

				AddNode(graph, fromId).Outgoing.Add(toId);
				AddNode(graph, toId).Incoming.Add(fromId);
			}
			return graph;
		}

		private static DirectedGraphNode<int> AddNode(Dictionary<int, DirectedGraphNode<int>> graph, int id)
		{
			if (!graph.ContainsKey(id)) {
				graph.Add(id, new DirectedGraphNode<int>(id));
			}
			return graph[id];
		}}
}
