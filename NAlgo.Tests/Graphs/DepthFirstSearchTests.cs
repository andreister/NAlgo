using System.Collections.Generic;
using NAlgo.Graphs;
using NAlgo.Graphs.Algorithms;
using NAlgo.Tests.Graphs.Extensions;
using NUnit.Framework;
using System.Linq;

namespace NAlgo.Tests.Graphs
{
	[TestFixture]
	public class DepthFirstSearchTests
	{
		[Test, TestCaseSource("Graphs")]
		public void TraverseGraph(string graphText, string expectedTraversal)
		{
			var graph = graphText.ToGraph<string, object>();

			string traversal = "";
			var algo = new DepthFirstSearch<string, object>();
			algo.Run(graph["start"], x => {
				traversal += x.Id + ",";
			});

			traversal = traversal.TrimEnd(new[] { ',' });
			Assert.That(traversal, Is.EqualTo(expectedTraversal), "Search path was not correct.");
		}

		public IEnumerable<TestCaseData> Graphs
		{
			get
			{
				return new List<TestCaseData> {
					new TestCaseData(
						"start a, start b, a start, a c, b start, b c, b d, c a, c b, c d, c end, d b, d c, d end, end c, end d",
						"end,d,b,c,a,start"
					).SetName("end,d,b,c,a,start")
				};
			}
		}
	}
}
