using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
		public static void RegisterTo(IVirtualMachine vm)
		{
			var dict = new Dictionary<int, int>();
			var stack = new Stack<int>();
			for(var i = 0; i < vm.Instructions.Length; i++)
			{
				if (vm.Instructions[i] == '[')
					stack.Push(i);
				if (vm.Instructions[i] == ']')
				{
					dict.Add(stack.Peek(), i);
					dict.Add(i, stack.Pop());
				}
			}

			vm.RegisterCommand('[', b => {
				if (b.Memory[b.MemoryPointer] == 0)
					b.InstructionPointer = dict[b.InstructionPointer];
			});

			vm.RegisterCommand(']', b => {
				if (b.Memory[b.MemoryPointer] != 0)
					b.InstructionPointer = dict[b.InstructionPointer];
			});
		}
	}
}