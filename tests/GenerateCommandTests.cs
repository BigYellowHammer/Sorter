using Altium.Generator;
using Altium.Generator.CommandOptions;
using NSubstitute;
using System.Text.RegularExpressions;

namespace tests;

public class GenerateCommandTests
{
    [Fact]
    public void Execute_GeneratesValidData_ZeroReturned()
    {

        //Arrange
        var fileHandler = Substitute.For<IFileHandler>();

        fileHandler.BytesWritten.Returns((ulong)0, (ulong)100);

        var options = new GenerateCommandOptions
        {
            ShowProgress = false,
            Size = 100,
            InputPath = string.Empty,
            OutputPath = "output.txt"
        };

		var sut = new GenerateCommand(fileHandler, new RandomnessGenerator());

        //Act
        int result = sut.Execute(null, options);

        //Assert
        var pattern = @"^\d+\.\s[\w\s]+$";

		Assert.Equal(0, result);
        fileHandler.Received().Write(Arg.Any<string>());
		fileHandler.Received().Write(Arg.Is<string>(x => Regex.IsMatch(x, pattern)));
	}

	[Fact]
	public void Execute_DictionaryProvidedAndUsed_ZeroReturned()
	{

		//Arrange
		var fileHandler = Substitute.For<IFileHandler>();
		fileHandler.ReadAllText(Arg.Any<string>()).Returns("foo");
		fileHandler.BytesWritten.Returns((ulong)0, (ulong)100);

		var options = new GenerateCommandOptions
		{
			ShowProgress = false,
			Size = 100,
			InputPath = "test",
			OutputPath = "output.txt"
		};

		var sut = new GenerateCommand(fileHandler, new RandomnessGenerator());

		//Act
		int result = sut.Execute(null, options);

		//Assert
		var pattern = @"^\d+\.\sFoo\s[\w\s]+$";

		Assert.Equal(0, result);
		fileHandler.Received().Write(Arg.Any<string>());
		fileHandler.Received().Write(Arg.Is<string>(x => Regex.IsMatch(x, pattern)));
	}

	[Fact]
	public void Execute_FileNotFound_NegativeResult()
	{

		//Arrange
		var fileHandler = Substitute.For<IFileHandler>();
		fileHandler.When(x => x.Configure(Arg.Any<string>())).Throw<FileNotFoundException>();

		fileHandler.BytesWritten.Returns((ulong)0, (ulong)100);

		var options = new GenerateCommandOptions
		{
			ShowProgress = false,
			Size = 100,
			InputPath = string.Empty,
			OutputPath = "output.txt"
		};

		var sut = new GenerateCommand(fileHandler, new RandomnessGenerator());

		//Act
		int result = sut.Execute(null, options);

		//Assert
		Assert.Equal(-3, result);
	}

	[Fact]
	public void Execute_FileAccessException_NegativeResult()
	{

		//Arrange
		var fileHandler = Substitute.For<IFileHandler>();
		fileHandler.When(x => x.Configure(Arg.Any<string>())).Throw<UnauthorizedAccessException>();

		fileHandler.BytesWritten.Returns((ulong)0, (ulong)100);

		var options = new GenerateCommandOptions
		{
			ShowProgress = false,
			Size = 100,
			InputPath = string.Empty,
			OutputPath = "output.txt"
		};

		var sut = new GenerateCommand(fileHandler, new RandomnessGenerator());

		//Act
		int result = sut.Execute(null, options);

		//Assert
		Assert.Equal(-2, result);
	}


	[Fact]
	public void Execute_OtherException_NegativeResult()
	{

		//Arrange
		var fileHandler = Substitute.For<IFileHandler>();
		fileHandler.When(x => x.Configure(Arg.Any<string>())).Throw<Exception>();

		fileHandler.BytesWritten.Returns((ulong)0, (ulong)100);

		var options = new GenerateCommandOptions
		{
			ShowProgress = false,
			Size = 100,
			InputPath = string.Empty,
			OutputPath = "output.txt"
		};

		var sut = new GenerateCommand(fileHandler, new RandomnessGenerator());

		//Act
		int result = sut.Execute(null, options);

		//Assert
		Assert.Equal(-1, result);
	}
}