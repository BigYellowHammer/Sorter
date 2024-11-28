using System.Text;

namespace Altium.Generator.CommandOptions
{
	internal class RandomnessGenerator : IRandomnessGenerator
	{
		private List<string> avaliableWords;
		private Random random = new Random();
		const string chars = "abcdefghijklmnopqrstuvwxyz";

		public void Configure(List<string> words)
		{
			avaliableWords = words;
		}

		public string GenerateRow()
		{
			return $"{GenerateRandomInteger()}. {GenerateRandomString()}";
		}

		public int GenerateRandomInteger()
		{
			return random.Next(0, int.MaxValue);
		}

		public string GenerateRandomString()
		{
			var numberOfWords = NumberOfWords();
			return GenerateSentence(numberOfWords);
		}

		private int NumberOfWords()
		{
			return random.Next(1, 12);
		}

		private string GenerateWord()
		{
			if (avaliableWords.Count == 0)
			{
				var length = random.Next(3, 15); //arbitrary
				char[] stringChars = new char[length];

				for (int i = 0; i < length; i++)
				{
					stringChars[i] = chars[random.Next(chars.Length)];
				}

				return new string(stringChars);
			}
			else
			{
				return avaliableWords[random.Next(avaliableWords.Count)];
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