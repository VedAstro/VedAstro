using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{
    public static class NLPTools
    {
        #region Vector Operations
        /// <summary>
        /// Computes the dot product of two vectors
        /// </summary>
        /// <param name="vector1">The first vector</param>
        /// <param name="vector2">The second vector</param>
        /// <returns></returns>
        public static double DotProduct(double[] vector1, double[] vector2)
        {
            return vector1.Zip(vector2, (a, b) => a * b).Sum();
        }

        /// <summary>
        /// Calculates the Euclidean distance from origin (0) of a vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double Magnitude(double[] vector)
        {
            return Math.Sqrt(vector.Sum(i => i * i));
        }

        /// <summary>
        /// Returns the cosine similarity between two vectors
        /// The cosine similarity measures the cosine of the angle
        /// between two non-zero vectors in a multi-dimensional space.
        /// It is a measure of orientation and not magnitude, returning
        /// a value between -1 and 1, where 1 means the vectors are identical,
        /// 0 means the vectors are orthogonal (i.e., not related),
        /// and -1 means the vectors are diametrically opposed.
        /// </summary>
        /// <param name="vector1">The first vector</param>
        /// <param name="vector2">The second vector</param>
        /// <returns></returns>
        public static double CosineSimilarity(double[] vector1, double[] vector2)
        {
            if (vector1 == null || vector2 == null || vector1.Length != vector2.Length)
                throw new ArgumentException("Invalid input.");

            double dotProduct = DotProduct(vector1, vector2);
            double magnitude1 = Magnitude(vector1);
            double magnitude2 = Magnitude(vector2);
            return dotProduct / (magnitude1 * magnitude2);
        }
        #endregion

        #region Simple Tokenizer
        /// <summary>
        /// A simple way to split text into tokens (words)
        /// </summary>
        public static List<string> Tokenize(string sentence)
        {
            string lowerSentence = sentence.ToLowerInvariant();
            int indexOfNextSpace = lowerSentence.IndexOf(' ');
            var result = new List<string>();

            while (indexOfNextSpace >= 0)
            {
                if (indexOfNextSpace > 0)
                    result.Add(lowerSentence.Substring(0, indexOfNextSpace));

                lowerSentence = lowerSentence.Remove(0, indexOfNextSpace + 1);
                indexOfNextSpace = lowerSentence.IndexOf(' ');
            }

            // Add remaining characters if any
            if (!string.IsNullOrEmpty(lowerSentence))
                result.Add(lowerSentence);

            return result;
        }
        #endregion

        /// <summary>
        /// Converts two strings into lists of integers representing the bag of words model
        /// </summary>
        /// <param name="sentence1">First sentence</param>
        /// <param name="sentence2">Second sentence</param>
        /// <returns>Two integer vectors</returns>
        public static Tuple<List<int>, List<int>> GetWordCountVectorFromText(string sentence1, string sentence2)
        {
            Dictionary<string, int> wordCountDict = new Dictionary<string, int>();

            foreach (var sentence in new[] { sentence1, sentence2 })
            {
                var tokens = Tokenize(sentence);
                foreach (var token in tokens)
                {
                    if (wordCountDict.ContainsKey(token))
                        wordCountDict[token]++;
                    else
                        wordCountDict[token] = 1;
                }
            }

            var orderedWords = wordCountDict.OrderByDescending(x => x.Value).Select(y => y.Key).ToList();
            IDictionary<string, int> sortedWordCountDict = new SortedDictionary<string, int>(orderedWords
                .ToDictionary(key => key, value => wordCountDict[value]));

            Func<string, int> getIndexForWord = word => sortedWordCountDict.Keys.ToList().IndexOf(word);
            Func<string, int> getWordCount = word => wordCountDict.TryGetValue(word, out int count) ? count : 0;

            List<int> vector1 = new List<int>();
            List<int> vector2 = new List<int>();

            foreach (var token in Tokenize(sentence1))
            {
                int idx = getIndexForWord(token);
                if (idx >= 0 && idx < sortedWordCountDict.Count)
                    vector1.Add(getWordCount(token));
            }

            foreach (var token in Tokenize(sentence2))
            {
                int idx = getIndexForWord(token);
                if (idx >= 0 && idx < sortedWordCountDict.Count)
                    vector2.Add(getWordCount(token));
            }

            return new Tuple<List<int>, List<int>>(vector1, vector2);
        }

    }
}
