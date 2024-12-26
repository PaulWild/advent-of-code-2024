using AdventOfCode.Days;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Tests.Days;


public class Day23Tests
{
    private readonly ISolution _sut = new Day23();
    
    private readonly string[] _testData =
    {
        "kh-tc",
        "qp-kh",
        "de-cg",
        "ka-co",
        "yn-aq",
        "qp-ub",
        "cg-tb",
        "vc-aq",
        "tb-ka",
        "wh-tc",
        "yn-cg",
        "kh-ub",
        "ta-co",
        "de-co",
        "tc-td",
        "tb-wq",
        "wh-td",
        "ta-ka",
        "td-qp",
        "aq-cg",
        "wq-ub",
        "ub-vc",
        "de-ta",
        "wq-aq",
        "wq-vc",
        "wh-yn",
        "ka-de",
        "kh-ta",
        "co-tc",
        "wh-qp",
        "tb-vc",
        "td-yn"
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

        actual.Should().Be("7");
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

        actual.Should().Be("co,de,ka,ta");
    }
}