using System;
using System.Collections.Generic;
using System.Linq;

namespace NAlgo.Text
{
	/// <summary>
	/// A ranking string comparison algorithm. Calculates a "distance"
	/// between the two strings via character pair similarity.
	/// 
	/// Inspired by Simon White http://www.catalysoft.com/articles/StrikeAMatch.html
	/// </summary>
	public class SimilarityChecker
	{
		/// <summary>
		/// Calculates the distance between the two strings by checking
		/// how many adjacent character pairs are contained in both strings.
		/// Returns a percentage, where 1 means the strings are identical.
		/// </summary>
		/// <param name="from">First string.</param>
		/// <param name="to">Second string.</param>
		public decimal Distance(string from, string to)
		{
			var fromChunks = GetLetterChunks(from);
			var toChunks = GetLetterChunks(to);
			var totalChunks = fromChunks.Count + toChunks.Count;

			int commonChunks = fromChunks.Sum(x => ComputeCommonChunks(x, toChunks));
			return (2m * commonChunks) / totalChunks;
		}

		private List<string> GetLetterChunks(string text)
		{
			var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			return words.SelectMany(GetLetterChunksInWord).ToList();
		}

		private IEnumerable<string> GetLetterChunksInWord(string word)
		{
			if (word.Length <= 2) {
				yield return word;
				yield break;
			}
			for (int i = 0; i < word.Length - 1; i++) {
				yield return word.Substring(i, 2);
			}
		}

		private int ComputeCommonChunks(string chunk, List<string> chunkList)
		{
			int result = 0;
			for (int i = 0; i < chunkList.Count; i++) {
				if (chunk.Equals(chunkList[i], StringComparison.InvariantCultureIgnoreCase)) {
					//whenever a match is found, the chunk is removed from the list,
					//to prevent us from matching against the same character chunk multiple
					//times (otherwise, 'GGGGG' would score a perfect match against 'GG').
					chunkList.RemoveAt(i);

					result++;
					break;
				}
			}
			return result;
		}
	}
}
