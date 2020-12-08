using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helper;

namespace AdventOfCode.Days
{
    public static class Day08
    {
        public static void Solve()
        {
            var lines = FileReader.ParseDataFromFile<string>("Inputs/Day08.txt");
            var consoleInstructions = lines.Select(line =>
            {
                var split = line.Split();
                return new Instruction
                {
                    Code = split[0].Trim(),
                    Value = int.Parse(split[1].Trim())
                };
            }).ToList();

            var accumulatorValue = GetAccumulatorForSequence(consoleInstructions);
            Console.WriteLine(accumulatorValue);

            var completedAccumulator = GetAccumulatorForTerminatedProgram(consoleInstructions);
            Console.WriteLine(completedAccumulator);
        }

        private static int GetAccumulatorForSequence(List<Instruction> gameInstructions)
        {
            var console = new HandheldConsole
            {
                Instructions = gameInstructions,
                Accumulation = 0
            };

            var accumulatorValue = console.Run();

            return accumulatorValue;
        }

        private static int GetAccumulatorForTerminatedProgram(IReadOnlyList<Instruction> gameInstructions)
        {
            var allJmpAndNopInstructions = gameInstructions
                .Select((instruction, i) => new {Instruction = instruction, Index = i})
                .Where(tuple => tuple.Instruction != null && (tuple.Instruction.Code == "nop" || tuple.Instruction.Code == "jmp"))
                .Select(tuple => tuple.Index);

            var accumulator = allJmpAndNopInstructions.Select(index =>
            {
                var gameInstructionsCopy = gameInstructions.Select(x => (Instruction) x.Clone()).ToList();
                gameInstructionsCopy[index].Code = gameInstructionsCopy[index].Code == "nop" ? "jmp" : "nop";
                var console = new HandheldConsole
                {
                    Instructions = gameInstructionsCopy,
                    Accumulation = 0,
                };
                var value = console.Run();

                return (value, console.Completed);
            }).First(tuple => tuple.Completed);

            return accumulator.value;
        }
    }
}
