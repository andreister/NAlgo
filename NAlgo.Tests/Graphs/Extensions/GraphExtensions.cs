using System;
using System.Collections.Generic;
using NAlgo.Graphs;

namespace NAlgo.Tests.Graphs.Extensions
{
	internal static class GraphExtensions
	{
		internal static Dictionary<TId, DigraphNode<TId, TValue>> ToDirectedGraph<TId, TValue>(this string graphText)
		{
			var graph = new Dictionary<TId, DigraphNode<TId, TValue>>();

			foreach (var edge in graphText.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)) {
				var nodes = edge.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
				TId fromId;
				TId toId;
				if (nodes.Length != 2 || !TryContvert(nodes[0], out fromId) || !TryContvert(nodes[1], out toId)) {
					throw new ArgumentException("Invalid input for an edge: " + edge);
				}

				AddNode(graph, fromId).Outgoing.Add(toId);
				AddNode(graph, toId).Incoming.Add(fromId);
			}
			return graph;
		}

		internal static Dictionary<TId, GraphNode<TId, TValue>> ToGraph<TId, TValue>(this string graphText)
		{
			var graph = new Dictionary<TId, GraphNode<TId, TValue>>();

			foreach (var edge in graphText.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)) {
				var nodes = edge.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
				TId fromId;
				TId toId;
				if (nodes.Length != 2 || !TryContvert(nodes[0], out fromId) || !TryContvert(nodes[1], out toId)) {
					throw new ArgumentException("Invalid input for an edge: " + edge);
				}

				AddNode(graph, fromId).Adjacent.Add(toId);
			}
			return graph;
		}

		private static DigraphNode<TId, TValue> AddNode<TId, TValue>(Dictionary<TId, DigraphNode<TId, TValue>> graph, TId id)
		{
			if (!graph.ContainsKey(id)) {
				graph.Add(id, new DigraphNode<TId, TValue>(id, graph));
			}
			return graph[id];
		}

		private static GraphNode<TId, TValue> AddNode<TId, TValue>(Dictionary<TId, GraphNode<TId, TValue>> graph, TId id)
		{
			if (!graph.ContainsKey(id)) {
				graph.Add(id, new GraphNode<TId, TValue>(id, graph));
			}
			return graph[id];
		}

		private static bool TryContvert<T>(string text, out T result)
		{
			if (typeof(T) == typeof(int)) {
				int value;
				var succeeded = int.TryParse(text, out value);
				result = (T)Convert.ChangeType(value, typeof(T));
				return succeeded;
			}
			if (typeof(T) == typeof(string)) {
				result = (T)Convert.ChangeType(text, typeof(T));
				return true;
			}

			result = default(T);
			return false;
		}
	}
}
