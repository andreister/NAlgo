using System.Collections.Generic;
using NAlgo.Text;
using NUnit.Framework;

namespace NAlgo.Tests.Text
{
	[TestFixture]
	public class SimilarityCheckerTests
	{
		[Test, TestCaseSource("Strings")]
		public void ComputeDistance(string from, string to, decimal expectedDistance)
		{
			var checker = new SimilarityChecker();
			var distance = checker.Distance(from, to);

			distance = decimal.Round(distance, 2);
			Assert.That(distance, Is.EqualTo(expectedDistance));
		}

		public IEnumerable<TestCaseData> Strings
		{
			get
			{
				return new List<TestCaseData> {
					new TestCaseData("this is a text", "this is a text", 1m).SetName("same strings"),
					new TestCaseData("this is a text", "thIS Is a tEXT", 1m).SetName("same strings, difference case"),
					new TestCaseData("france", "french", 0.4m).SetName("france, french"),
					new TestCaseData("gggg", "gg", 0.5m).SetName("gggg, gg"),
					new TestCaseData("healed", "sealed", 0.8m).SetName("healed, sealed"),
					new TestCaseData("healed", "healthy", 0.55m).SetName("healed, healthy"),
					new TestCaseData("healed", "help", 0.25m).SetName("healed, help"),
					new TestCaseData("healed", "sold", 0m).SetName("healed, sold"),
				};
			}
		}
	}
}
