using System.Text;
using Altium.GenSort;
using Altium.GenSort.CommandOptions;
using NSubstitute;
using System.Text.RegularExpressions;
using Spectre.Console.Cli;

namespace Altium.GenSort.Tests;

public class SortCommandTests
{
    [Fact]
    public void Execute_GeneratesValidData_ZeroReturned()
    {

		//Arrange
		var remainingArguments = Substitute.For<IRemainingArguments>();
		var commandContext = new CommandContext(new List<string>(), remainingArguments, "test", new());

		string exampleLine = "1. Test";
		string chunkName = "tes";
        string[] linesToBeSorted = { "2. Apple", "1. Apple", "2. Banana"};
        string[] sortedLines = { "1. Apple", "2. Apple", "2. Banana" };
		
		var fileHandler = Substitute.For<IFileHandler>();

        fileHandler.ReadLines(Arg.Any<string>()).Returns(new[]{ exampleLine });
        fileHandler.ChunkNames.Returns(new[] { chunkName });
        fileHandler.ReadChunkLines(Arg.Is($"{chunkName}")).Returns(linesToBeSorted, sortedLines);

		var options = new SortCommandOptions()
        {
	        InputPath = "output.txt"
        };

		var sut = new SortCommand(fileHandler);

        //Act
        int result = sut.Execute(commandContext, options);

        //Assert

        Assert.Equal(0, result);
        fileHandler.Received().SaveLineIntoChunk(Arg.Is<string>(x => x == chunkName), Arg.Is<string>(x=> x == exampleLine));
        fileHandler.Received().SaveLinesIntoChunk(Arg.Is<string>(x=> x == chunkName), Arg.Is<string[]>(x => x.SequenceEqual(sortedLines)));
        fileHandler.Received().WriteLines(Arg.Is<string[]>(x=>x.SequenceEqual(sortedLines)));
    }

    [Fact]
    public void Execute_GeneratesValidDataAndProgress_ZeroReturned()
    {

	    //Arrange
	    var remainingArguments = Substitute.For<IRemainingArguments>();
	    var commandContext = new CommandContext(new List<string>(), remainingArguments, "test", new());

	    string exampleLine = "1. Test";
	    string chunkName = "tes";
	    string[] linesToBeSorted = { "2. Apple", "1. Apple", "2. Banana" };
	    string[] sortedLines = { "1. Apple", "2. Apple", "2. Banana" };

	    var fileHandler = Substitute.For<IFileHandler>();

	    fileHandler.ReadLines(Arg.Any<string>()).Returns(new[] { exampleLine });
	    fileHandler.ChunkNames.Returns(new[] { chunkName });
	    fileHandler.ReadChunkLines(Arg.Is($"{chunkName}")).Returns(linesToBeSorted, sortedLines);

	    var options = new SortCommandOptions()
	    {
		    InputPath = "output.txt",
			ShowProgress = true
	    };

	    var sut = new SortCommand(fileHandler);

	    //Act
	    int result = sut.Execute(commandContext, options);

	    //Assert

	    Assert.Equal(0, result);
	    fileHandler.Received().SaveLineIntoChunk(Arg.Is<string>(x => x == chunkName), Arg.Is<string>(x => x == exampleLine));
	    fileHandler.Received().SaveLinesIntoChunk(Arg.Is<string>(x => x == chunkName), Arg.Is<string[]>(x => x.SequenceEqual(sortedLines)));
	    fileHandler.Received().WriteLines(Arg.Is<string[]>(x => x.SequenceEqual(sortedLines)));
    }

	[Fact]
    public void Execute_MultipleChunks_CorrectlySorted()
    {

		//Arrange
		var remainingArguments = Substitute.For<IRemainingArguments>();
		var commandContext = new CommandContext(new List<string>(), remainingArguments, "test", new());

		string exampleLine = "1. Foo";
		string exampleLine2 = "2. Bar";
		string chunkName = "foo";
		string chunkName2 = "bar";
        string[] linesToBeSorted = { "2. Fooa", "1. Fooa", "2. Foob"};
        string[] sortedLines = { "1. Fooa", "2. Fooa", "2. Foob" };
		string[] linesToBeSorted2 = { "1. Bara", "2. Bara", "2. Barb"};
        string[] sortedLines2 = { "1. Bara", "2. Bara", "2. Barb" };
		
		var fileHandler = Substitute.For<IFileHandler>();

        fileHandler.ReadLines(Arg.Any<string>()).Returns(new[]{ exampleLine, exampleLine2 });
        fileHandler.ChunkNames.Returns(new[] { chunkName, chunkName2 });
        fileHandler.ReadChunkLines(Arg.Is($"{chunkName}")).Returns(linesToBeSorted, sortedLines);
		fileHandler.ReadChunkLines(Arg.Is($"{chunkName2}")).Returns(linesToBeSorted2, sortedLines2);

		var options = new SortCommandOptions()
        {
	        InputPath = "output.txt"
        };

		var sut = new SortCommand(fileHandler);

        //Act
        int result = sut.Execute(commandContext, options);

        //Assert

        Assert.Equal(0, result);
        fileHandler.Received().SaveLineIntoChunk(Arg.Is<string>(x => x == chunkName), Arg.Is<string>(x=> x == exampleLine));
        fileHandler.Received().SaveLineIntoChunk(Arg.Is<string>(x => x == chunkName2), Arg.Is<string>(x=> x == exampleLine2));

		fileHandler.Received().SaveLinesIntoChunk(Arg.Is<string>(x=> x == chunkName), Arg.Is<string[]>(x => x.SequenceEqual(sortedLines)));
        fileHandler.Received().SaveLinesIntoChunk(Arg.Is<string>(x=> x == chunkName2), Arg.Is<string[]>(x => x.SequenceEqual(sortedLines2)));

		fileHandler.Received().WriteLines(Arg.Is<string[]>(x=>x.SequenceEqual(sortedLines)));
		fileHandler.Received().WriteLines(Arg.Is<string[]>(x => x.SequenceEqual(sortedLines2)));
	}

	[Fact]
	public void Execute_FileNotFound_NegativeResult()
	{

		//Arrange
		var remainingArguments = Substitute.For<IRemainingArguments>();
		var commandContext = new CommandContext(new List<string>(), remainingArguments, "test", new());
		var fileHandler = Substitute.For<IFileHandler>();
		fileHandler.When(x => x.Configure(Arg.Any<string>())).Throw<FileNotFoundException>();

		var options = new SortCommandOptions()
		{
			InputPath = string.Empty,
		};

		var sut = new SortCommand(fileHandler);

		//Act
		int result = sut.Execute(commandContext, options);

		//Assert
		Assert.Equal(-3, result);
	}

	[Fact]
	public void Execute_FileAccessException_NegativeResult()
	{

		//Arrange
		var remainingArguments = Substitute.For<IRemainingArguments>();
		var commandContext = new CommandContext(new List<string>(), remainingArguments, "test", new());

		var fileHandler = Substitute.For<IFileHandler>();
		fileHandler.When(x => x.Configure(Arg.Any<string>())).Throw<UnauthorizedAccessException>();

		fileHandler.BytesWritten.Returns((ulong)0, (ulong)100);

		var options = new SortCommandOptions()
		{
			InputPath = string.Empty
		};

		var sut = new SortCommand(fileHandler);

		//Act
		int result = sut.Execute(commandContext, options);

		//Assert
		Assert.Equal(-2, result);
	}


	[Fact]
	public void Execute_OtherException_NegativeResult()
	{

		//Arrange
		var remainingArguments = Substitute.For<IRemainingArguments>();
		var commandContext = new CommandContext(new List<string>(), remainingArguments, "test", new());

		var fileHandler = Substitute.For<IFileHandler>();
		fileHandler.When(x => x.Configure(Arg.Any<string>())).Throw<Exception>();

		fileHandler.BytesWritten.Returns((ulong)0, (ulong)100);

		var options = new SortCommandOptions()
		{
			InputPath = string.Empty
		};

		var sut = new SortCommand(fileHandler);

		//Act
		int result = sut.Execute(commandContext, options);

		//Assert
		Assert.Equal(-1, result);
	}

	[Fact]
	public void Execute_TestSorting1()
	{

		//Arrange
		string[] linesToBeSorted = { "415. Apple", "30432. Something something something", "1. Apple", "32. Cherry is the best", "2. Banana is yellow" };
		string[] sortedLines = { "1. Apple", "415. Apple", "2. Banana is yellow", "32. Cherry is the best", "30432. Something something something" };

		var fileHandler = Substitute.For<IFileHandler>();

		var options = new SortCommandOptions()
		{
			InputPath = "output.txt"
		};

		var sut = new SortCommand(fileHandler);

		//Act
		var result = sut.CustomSort(linesToBeSorted);


		//Assert
		Assert.True(result.SequenceEqual(sortedLines));
	}

	[Fact]
	public void Execute_TestSorting2()
	{

		//Arrange
		string[] linesToBeSorted = { "1. Apple", "1. Apple", "2. Banana is yellow" };
		string[] sortedLines = { "1. Apple", "1. Apple", "2. Banana is yellow" };

		var fileHandler = Substitute.For<IFileHandler>();

		var options = new SortCommandOptions()
		{
			InputPath = "output.txt"
		};

		var sut = new SortCommand(fileHandler);

		//Act
		var result = sut.CustomSort(linesToBeSorted);


		//Assert
		Assert.True(result.SequenceEqual(sortedLines));
	}

	[Fact]
	public void Execute_TestSorting3()
	{

		//Arrange
		string[] linesToBeSorted = { "1. Apple", "1. A big brown fox", "2. Banana is yellow" };
		string[] sortedLines = { "1. A big brown fox", "1. Apple", "2. Banana is yellow" };

		var fileHandler = Substitute.For<IFileHandler>();

		var options = new SortCommandOptions()
		{
			InputPath = "output.txt"
		};

		var sut = new SortCommand(fileHandler);

		//Act
		var result = sut.CustomSort(linesToBeSorted);


		//Assert
		Assert.True(result.SequenceEqual(sortedLines));
	}

	[Fact]
	public void Execute_TestSorting4()
	{

		//Arrange
		string[] linesToBeSorted = { "1. Apple", "1. A big brown fox", "2. B anana is yellow" };
		string[] sortedLines = { "1. A big brown fox", "1. Apple", "2. B anana is yellow" };

		var fileHandler = Substitute.For<IFileHandler>();

		var options = new SortCommandOptions()
		{
			InputPath = "output.txt"
		};

		var sut = new SortCommand(fileHandler);

		//Act
		var result = sut.CustomSort(linesToBeSorted);


		//Assert
		Assert.True(result.SequenceEqual(sortedLines));
	}

}