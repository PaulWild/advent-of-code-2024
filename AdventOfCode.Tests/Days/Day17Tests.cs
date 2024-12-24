using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days;


public class Day17Tests
{
    private readonly ISolution _sut = new Day17();
    
    private readonly string[] _testData =
    {
        "Register A: 729",
        "Register B: 0",
        "Register C: 0",
        "",
        "Program: 0,1,5,4,3,0",
    };
    
    private readonly string[] _testDataPart2 =
    {
        "Register A: 2024",
        "Register B: 0",
        "Register C: 0",
        "",
        "Program: 0,3,5,4,3,0",
    };

    [Fact]
    public void PartOne_WhenCalled_DoesNotThrowNotImplementedException()
    {
        Action act = () => _sut.PartOne(_sut.Input());

        act.Should().NotThrow<NotImplementedException>();
    }
    
    [Fact]
    public void PartOne_WhenCalled_ReturnsCorrectTestAnswer()
    {
        var actual = _sut.PartOne(_testData);

        actual.Should().Be("4,6,3,5,6,3,5,2,1,0");
    }


    [Fact]
    public void PartTwo_WhenCalled_DoesNotThrowNotImplementedException()
    {
        Action act = () => _sut.PartTwo(_sut.Input());

        act.Should().NotThrow<NotImplementedException>();
    }
    
    [Fact]
    public void PartTwo_WhenCalled_ReturnsCorrectTestAnswer()
    {
        var actual = _sut.PartTwo(_testDataPart2);

        actual.Should().Be("117440");
    }
}