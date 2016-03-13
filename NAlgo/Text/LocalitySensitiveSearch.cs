using System;
using System.Collections.Generic;
using System.Linq;

namespace NAlgo.Text
{
    /// <summary>
    /// LHS based on MinHash signatures.
    /// 
    /// Text gets split into shingles, and those get passed as parameters 
    /// to a list of randomized hash functions (but only min hash values gets stored in a signature).
    /// </summary>
    public class LocalitySensitiveSearch
    {
        private readonly List<Func<int, int>> _hashFunctions;
        private readonly int _wordsPerShingle;

        public LocalitySensitiveSearch(int permutationsCount = 100, int wordsPerShingle = 3, int seed = 123)
        {
            _hashFunctions = new List<Func<int, int>>(permutationsCount);
            _wordsPerShingle = wordsPerShingle;

            var random = new Random(seed);
            for (var i = 0; i < permutationsCount; i++) {
                //create a bunch of random hash functions (ax + b) mod c
                var a = (uint)random.Next(1000);    //random
                var b = (uint)random.Next(1000);    //random
                var c = (uint)2147483647;           //prime number
                _hashFunctions.Add((x) => {
                    return (int)((a * x + b) % c);
                });
            }
        }

        public DocumentSignature GetSignature(long documentId, string text)
        {
            var result = new int[_hashFunctions.Count];

            var cleanedText = Cleanup(text);
            var shingles = GetShingles(cleanedText);

            for (var functionId = 0; functionId < _hashFunctions.Count; functionId++) {
                var minHash = int.MaxValue;
                for (var shingleId = 0; shingleId < shingles.Count; shingleId++) {
                    var shingleHash = shingles[shingleId].Hash;
                    var randomHash = _hashFunctions[functionId](shingleHash);
                    minHash = Math.Min(randomHash, minHash);
                }
                result[functionId] = minHash;
            }

            return new DocumentSignature(documentId, result.ToArray());
        }

        private string Cleanup(string text)
        {
            var punctuation = new[] { ".", ",", ":", ";", "-", "\"", "'" };
            var whitespaces = new[] { "\n", "\r", "\r\n", "\t" };
            var stopwords = new[] { "and", "or", "but", "has", "have", "is", "in",
                                    "with", "for", "to", "from", "then", "when", "a",
                                    "an", "the", "not", "in", "which" };

            foreach (var x in punctuation) {
                text = text.Replace(x, "");
            }
            foreach (var x in whitespaces) {
                text = text.Replace(x, " ");
            }
            foreach (var x in stopwords) {
                text = text.Replace(" " + x + " ", " ");
            }
            return text.ToLower();
        }

        private List<Shingle> GetShingles(string text)
        {
            var result = new List<Shingle>();

            var words = text.Split();
            using (var md5 = System.Security.Cryptography.MD5.Create()) {
                for (var wordId = 0; wordId < words.Length; wordId++) {
                    var shingle = "";
                    for (var i = 0; i < _wordsPerShingle; i++) {
                        if (wordId + i == words.Length) {
                            //reached end of document
                            break;
                        }
                        shingle += words[wordId + i];
                    }

                    var shingleHash = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(shingle));
                    result.Add(new Shingle(shingle, shingleHash));
                }
            }

            return result;
        }

        public class DocumentSignature
        {
            private long _documentId;
            private int[] _minhashVector;

            public DocumentSignature(long documentId, int[] minhashVector)
            {
                _documentId = documentId;
                _minhashVector = minhashVector;
            }

            public decimal[] GetSimilarity(params DocumentSignature[] signatures)
            {
                var result = new List<decimal>(_minhashVector.Length);

                foreach (var signature in signatures) {
                    var equal = 0.0m;
                    var total = _minhashVector.Length;
                    for (var i = 0; i < _minhashVector.Length; i++) {
                        if (_minhashVector[i] == signature._minhashVector[i]) {
                            equal++;
                        }
                    }

                    var similarity = equal / total;
                    result.Add(similarity);
                }

                return result.ToArray();
            }
        }

        private class Shingle
        {
            public string Text { get; private set; }
            public int Hash { get; private set; }

            internal Shingle(string text, byte[] hash)
            {
                Text = text;
                Hash = BitConverter.ToInt32(hash, 0);
            }
        }
    }
}
