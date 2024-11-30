using System.Text;

namespace Altium.GenSort.Random
{
	internal class RandomnessGenerator : IRandomnessGenerator
	{
		private List<string> _avaliableWords = new List<string>();
		private readonly System.Random _random = new();
		private const string _chars = "abcdefghijklmnopqrstuvwxyz";

		public void Configure(List<string> words)
		{
			_avaliableWords = words;
		}

		public string GenerateRow()
		{
			return $"{GenerateRandomInteger()}. {GenerateRandomString()}";
		}

		public int GenerateRandomInteger()
		{
			return _random.Next(0, int.MaxValue);
		}

		public string GenerateRandomString()
		{
			var numberOfWords = NumberOfWords();
			return GenerateSentence(numberOfWords);
		}

		private int NumberOfWords()
		{
			return _random.Next(1, 12);
		}

		private string GenerateWord()
		{
			if (_avaliableWords.Count == 0)
			{
				var length = _random.Next(3, 15); //arbitrary
				char[] stringChars = new char[length];

				for (int i = 0; i < length; i++)
				{
					stringChars[i] = _chars[_random.Next(_chars.Length)];
				}

				return new string(stringChars);
			}
			else
			{
				return _avaliableWords[_random.Next(_avaliableWords.Count)];
			}
		}

		private string GenerateSentence(int numberOfWords)
		{
			StringBuilder sentence = new StringBuilder();
			for (int i = 0; i < numberOfWords; i++)
			{
				sentence.Append(GenerateWord());
				sentence.Append(" ");
			}

			return CapitalizeFirstLetter(sentence.ToString().TrimEnd());
		}

		private static string CapitalizeFirstLetter(string input)
		{
			return char.ToUpper(input[0]) + input.Substring(1);
		}
	}
}