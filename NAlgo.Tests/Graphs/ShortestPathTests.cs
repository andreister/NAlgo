using System.Collections.Generic;
using NAlgo.Graphs.Algorithms;
using NAlgo.Tests.Graphs.Extensions;
using NUnit.Framework;

namespace NAlgo.Tests.Graphs
{
	[TestFixture]
	public class ShortestPathTests
	{
		[Test, TestCaseSource("Graphs")]
		public void ComputeDistance(string graphText, int expectedDistance)
		{
			var graph = graphText.ToGraph<string>();

			var algo = new ShortestPath<string>(graph);
			var distance = algo.ComputeDistance(graph["start"], graph["end"]);

			Assert.That(distance, Is.EqualTo(expectedDistance), "Shortest path not found.");
		}

		public IEnumerable<TestCaseData> Graphs
		{
			get
			{
				return new List<TestCaseData> {
					new TestCaseData(
						"start a, start c, a start, a b, c start, c e, c end, b a, b d, d b, d e, e c, e end, e f, f e, f end, end f, end e, end c",
						2
					).SetName("2 layers graph"),
					new TestCaseData(
						"start a, start b, a start, a c, b start, b c, b d, c a, c b, c d, c end, d b, d c, d end, end c, end d",
						3
					).SetName("3 layers graph"),
					new TestCaseData(
						"start a, a start, a b, b a, b c, c b, c d, d end, end d",
						5
					).SetName("5 layers graph")
				};
			}
		}
	}
}
