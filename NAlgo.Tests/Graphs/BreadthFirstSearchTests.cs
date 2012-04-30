using System.Collections.Generic;
using NAlgo.Graphs.Algorithms;
using NAlgo.Tests.Graphs.Extensions;
using NUnit.Framework;

namespace NAlgo.Tests.Graphs
{
	[TestFixture]
	public class BreadthFirstTests
	{
		[Test, TestCaseSource("Graphs")]
		public void TraverseGraph(string graphText, string expectedTraversal)
		{
			var graph = graphText.ToGraph<string>();
			
			string traversal = "";
			var algo = new BreadthFirstSearch<string>(graph);
			algo.Run(graph["start"], node => {
				traversal += node.Id + ",";
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
						"start,a,b,c,d,end"
					).SetName("start,a,b,c,d,end")
				};
			}
		}
	}
}
