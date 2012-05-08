using System;
using System.Collections.Generic;
using NAlgo.Graphs.Algorithms;
using NAlgo.Tests.Graphs.Extensions;
using NUnit.Framework;
using System.Linq;

namespace NAlgo.Tests.Graphs
{
	[TestFixture]
	public class TopologicalSortTests
	{
		[Test, TestCaseSource("Graphs")]
		public void SetNodesPosition(string graphText, string expectedPositions)
		{
			var graph = graphText.ToDirectedGraph<string, int>().ToDictionary(x => x.Key, x => x.Value);

			var algo = new TopologicalSort<string>(graph);
			algo.Run();

			foreach (var nodePosition in expectedPositions.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)) {
				var pair = nodePosition.Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries);
				var node = graph[pair[0].Trim()];
				int expectedPosition = int.Parse(pair[1]);

				Assert.That(node.Value, Is.EqualTo(expectedPosition), "Node " + node.Id + " has unexpected topological order.");
			}
		}

		public IEnumerable<TestCaseData> Graphs
		{
			get
			{
				return new List<TestCaseData> {
					new TestCaseData(
						"start v, start w, v end, w end",
						"start-1, v-3, w-2, end-4"
					).SetName("4 nodes graph"),
					new TestCaseData(
						"a b, b c, b d, c e, c f, d e, d f, e f, f i",
						"a-1, b-2, c-4, d-3, e-5, f-6, i-7"
					).SetName("7 nodes graph")
				};
			}
		}
	}
}
