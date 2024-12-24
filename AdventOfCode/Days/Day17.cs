using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Days;

public partial class Day17 : ISolution
{
    public static long Mod(long x, long m) {
        return (x%m + m)%m;
    }
    
    public string PartOne(IEnumerable<string> input)
    {
        var (registerA, registerB, registerC, program) = ParseInput(input);
        var outs = RunProgram(program, registerA, registerB, registerC);

        return string.Join(",", outs);
    }

    private static (long, long, long, List<long>) ParseInput(IEnumerable<string> input)
    {
        var inputList = input.ToList();
        var registerA = int.Parse(Digits().Match(inputList[0]).Value);
        var registerB = int.Parse(Digits().Match(inputList[1]).Value);
        var registerC = int.Parse(Digits().Match(inputList[2]).Value);

        var program = Digits().Matches(inputList[4]).Select(x => long.Parse(x.Value)).ToList();
        return (registerA, registerB, registerC, program);
    }

    private static List<long> RunProgram(List<long> program, long registerA, long registerB, long registerC)
    {
        var outs = new List<long>();
        for (int i = 0; i < program.Count; i=i+2)
        {
            var instruction = program[i];
            var operand = program[i + 1];
            var comboOperandValue = ComboOperandValue(operand);
            
            switch (instruction)
            {
                case 0:
                {
                    var result = (long)(registerA / Math.Pow(2, comboOperandValue));
                    registerA = result;
                    break;
                }
                case 1:
                    registerB ^= operand;
                    break;
                case 2:
                    registerB = Mod(comboOperandValue, 8);
                    break;
                case 3 when registerA == 0:
                    continue;
                case 3:
                    i = (int)operand-2;
                    break;
                case 4:
                    registerB ^= registerC;
                    break;
                case 5:
                    outs.Add(Mod(comboOperandValue, 8));
                    break;
                case 6:
                {
                    var result = (long)(registerA / Math.Pow(2, comboOperandValue));
                    registerB = result;
                    break;
                }
                case 7:
                {
                    var result = (long)(registerA / Math.Pow(2, comboOperandValue));
                    registerC = result;
                    break;
                }
            }
        }

        return outs;

        long ComboOperandValue  (long comboOperand)
        {
            return comboOperand switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => registerA,
                5 => registerB,
                6 => registerC,
                7 => throw new ArgumentException("combo operand is invalid."),
                _ => throw new ArgumentException("combo operand is invalid.")
            };
        }
    }


    long ToBase10(List<int> digits)
    {
        var octal = string.Join("", digits);
        return Convert.ToInt64(octal, 8);
    }
    
    public string PartTwo(IEnumerable<string> input)
    {
        var (_, _, _, program) = ParseInput(input);
        
        var digits = Enumerable.Repeat(0, program.Count).ToList();
        
        bool IsPotential(List<int> digits, int digit)
        {
            var potential = ToBase10(digits);
            var result =  RunProgram(program, potential, 0, 0);

            if (result.Count != program.Count)
            {
                return false;
            }

            if (result[result.Count - (digit+1)] == program[result.Count - (digit+1)])
            {
                return true;
            }

            return false;
        }
        
        bool Backtrack(List<int> digits, int digit)
        {
            if (digit >= digits.Count)
            {
                return true;
            }
            
            for (var j = 0; j < 8; j++)
            {
                var newDigis = digits.Select(x => x).ToList();
                newDigis[digit] = j;
                if (!IsPotential(newDigis, digit)) continue;
                
                digits[digit] = j;
                if (Backtrack(digits, digit + 1))
                    return true;
                digits[digit] = 0;
            }

            return false;
        }

        Backtrack(digits, 0);
        return ToBase10(digits).ToString();
    }

    public int Day => 17;

    [GeneratedRegex(@"\d+")]
    private static partial Regex Digits();
}
