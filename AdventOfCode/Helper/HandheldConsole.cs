using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Helper
{
    public class HandheldConsole
    {
        public List<Instruction> Instructions { get; set; }
        public int Accumulation { get; set; }
        public bool Completed { get; set; }

        public int Run()
        {
            var seen = new bool[Instructions.Count];
            var i = 0;
            bool Booted(bool[] seenValues) => seenValues.All(s => s);
            while (true)
            {
                if (i >= Instructions.Count || Booted(seen))
                {
                    Completed = true;
                    break;
                }
                if (seen[i % Instructions.Count])
                {
                    break;
                }

                i %= Instructions.Count;

                seen[i] = true;
                var instruction = Instructions[i];
                var gameCode = instruction.Code;
                var instructionValue = instruction.Value;
                switch (gameCode)
                {
                    case "acc":
                        Accumulation += instructionValue;
                        i++;
                        break;
                    case "jmp":
                        i = i + instructionValue;
                        break;
                    case "nop":
                        i++;
                        break;
                    default:
                        throw new ArgumentException($"Invalid game code {gameCode}");
                }
            }

            return Accumulation;
        }
    }

    public class Instruction : ICloneable
    {
        public string Code { get; set; }
        public int Value { get; set; }
        public object Clone()
        {
            return new Instruction
            {
                Code = Code,
                Value = Value
            };

        }
    }
}
