namespace Altium.Generator.CommandOptions
{
	public interface IRandomnessGenerator
	{
		int GenerateRandomInteger();
		string GenerateRandomString();
		string GenerateRow();
		void Configure(List<string> words);
	}
}