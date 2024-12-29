using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days;


public class Day25Tests
{
    private readonly ISolution _sut = new Day25();
    
    private readonly string[] _testData =
    {
        "#####",
        ".####",
        ".####",
        ".####",
        ".#.#.",
        ".#...",
        ".....",
        "",
        "#####",
        "##.##",
        ".#.##",
        "...##",
        "...#.",
        "...#.",
        ".....",
        "",
        ".....",
        "#....",
        "#....",
        "#...#",
        "#.#.#",
        "#.###",
        "#####",
        "",
        ".....",
        ".....",
        "#.#..",
        "###..",
        "###.#",
        "###.#",
        "#####",
        "",
        ".....",
        ".....",
        ".....",
        "#....",
        "#.#..",
        "#.#.#",
        "#####"
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

        actual.Should().Be("3");
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

        actual.Should().Be("AOC_2025_DONE");
    }
}