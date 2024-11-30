namespace Altium.GenSort.Random
{
	public interface IRandomnessGenerator
	{
		int GenerateRandomInteger();
		string GenerateRandomString();
		string GenerateRow();
		void Configure(List<string> words);
	}
}