using System;
using System.Collections.Generic;
using NAlgo.Graphs;

namespace NAlgo.Tests.Graphs.Extensions
{
	internal static class GraphExtensions
	{
		internal static Dictionary<T, DigraphNode<T>> ToDirectedGraph<T>(this string graphText)
		{
			var graph = new Dictionary<T, DigraphNode<T>>();

			foreach (var edge in graphText.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)) {
				var nodes = edge.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
				T fromId;
				T toId;
				if (nodes.Length != 2 || !TryContvert(nodes[0], out fromId) || !TryContvert(nodes[1], out toId)) {
					throw new ArgumentException("Invalid input for an edge: " + edge);
				}

				AddNode(graph, fromId).Outgoing.Add(toId);
				AddNode(graph, toId).Incoming.Add(fromId);
			}
			return graph;
		}

		internal static Dictionary<T, GraphNode<T>> ToGraph<T>(this string graphText)
		{
			var graph = new Dictionary<T, GraphNode<T>>();

			foreach (var edge in graphText.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)) {
				var nodes = edge.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
				T fromId;
				T toId;
				if (nodes.Length != 2 || !TryContvert(nodes[0], out fromId) || !TryContvert(nodes[1], out toId)) {
					throw new ArgumentException("Invalid input for an edge: " + edge);
				}

				AddNode(graph, fromId).Adjacent.Add(toId);
			}
			return graph;
		}

		private static DigraphNode<T> AddNode<T>(Dictionary<T, DigraphNode<T>> graph, T id)
		{
			if (!graph.ContainsKey(id)) {
				graph.Add(id, new DigraphNode<T>(id));
			}
			return graph[id];
		}
		
		private static GraphNode<T> AddNode<T>(Dictionary<T, GraphNode<T>> graph, T id)
		{
			if (!graph.ContainsKey(id)) {
				graph.Add(id, new GraphNode<T>(id));
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
