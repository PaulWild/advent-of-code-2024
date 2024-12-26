using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days;


public class Day21Tests
{
    private readonly ISolution _sut = new Day21();
    
    private readonly string[] _testData =
    {
        "029A",
        "980A",
        "179A",
        "456A",
        "379A"
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

        actual.Should().Be("126384");
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
        var actual = _sut.PartTwo(_testData);

        actual.Should().Be("SomeString");
    }
}