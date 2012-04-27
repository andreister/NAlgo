using System.Collections.Generic;
using System.Linq;
using NAlgo.Graphs;
using NAlgo.Graphs.Algorithms;
using NAlgo.Tests.Graphs.Extensions;
using NUnit.Framework;

namespace NAlgo.Tests.Graphs
{
	[TestFixture]
	public class KosarajuTests
	{
		[Test, TestCaseSource("Graphs")]
		public void ComputeStronglyConnectedComponents(Dictionary<int, DirectedGraphNode<int>> graph, int[] expectedSizes)
		{
			var algo = new Kosaraju<int>(graph);
			var components = algo.GetConnectedComponents().OrderByDescending(x => x.Value.Count).ToList();

			Assert.That(components.Count, Is.EqualTo(expectedSizes.Length), "Not all strongly connected components were found.");
			for (int i = 0; i < expectedSizes.Length; i++) {
				var componentSize = components[i].Value.Count;
				Assert.That(componentSize, Is.EqualTo(expectedSizes[i]), "Invalid component size.");
			}
		}

		public IEnumerable<TestCaseData> Graphs
		{
			get
			{
				return new List<TestCaseData> {
					new TestCaseData(
						"1 1, 1 3, 3 2, 2 1, 3 5, 4 1, 4 2, 4 12, 4 13, 5 6, 5 8, 6 7, 6 8, 6 10, 7 10, 8 9, 8 10, 9 5, 9 11, 10 9, 10 11, 10 14, 11 12, 11 14, 12 13, 13 11, 13 15, 14 13, 15 14".ToDirectedGraph(), 
						new [] {6,5,3,1}
					).SetName("{6,5,3,1}"),
					new TestCaseData(
						"1 2, 2 3, 2 4, 3 1, 3 8, 3 11, 4 5, 4 7, 5 6, 6 7, 7 5, 8 7, 8 9, 8 10, 9 10, 9 6, 10 11, 11 8".ToDirectedGraph(), 
						new [] {4,3,3,1}
					).SetName("{4,3,3,1}"),
					new TestCaseData(
						"1 4, 2 8, 3 6, 4 7, 5 2, 6 9, 7 1, 8 5, 8 6, 9 3, 9 7".ToDirectedGraph(), 
						new [] {3,3,3}
					).SetName("{3,3,3}"),
					new TestCaseData(
						"1 4, 1 3, 4 2, 5 1, 5 2, 5 3".ToDirectedGraph(), 
						new [] {1,1,1,1,1}
					).SetName("{1,1,1,1,1}"),
				};
			}
		}
	}
}
