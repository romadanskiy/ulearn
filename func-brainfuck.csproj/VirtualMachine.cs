using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		private readonly Dictionary<char, Action<IVirtualMachine>> commands;

		public VirtualMachine(string program, int memorySize)
		{
			Instructions = program;
			Memory = new byte[memorySize];
			commands = new Dictionary<char, Action<IVirtualMachine>>();
		}

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			if (!commands.ContainsKey(symbol))
				commands.Add(symbol, execute);
		}

		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; }
		public int MemoryPointer { get; set; }
		public void Run()
		{
			while (InstructionPointer < Instructions.Length)
			{
				if (commands.ContainsKey(Instructions[InstructionPointer]))
				{
					commands[Instructions[InstructionPointer]](this);
					InstructionPointer++;
				}
				else
					InstructionPointer++;
			}
		}
	}
}