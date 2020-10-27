using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			vm.RegisterCommand('.', b => write((char)b.Memory[b.MemoryPointer]));

			vm.RegisterCommand('+', b => { 
				var a = (int)b.Memory[b.MemoryPointer];
				a++;
				if (a > 255) b.Memory[b.MemoryPointer] = 0;
				else b.Memory[b.MemoryPointer] = (byte)a;
			});

			vm.RegisterCommand('-', b => {
				var a = (int)b.Memory[b.MemoryPointer];
				a--;
				if (a < 0) b.Memory[b.MemoryPointer] = 255;
				else b.Memory[b.MemoryPointer] = (byte)a;
			});

			vm.RegisterCommand(',', b => b.Memory[b.MemoryPointer] = (byte)read());

			vm.RegisterCommand('>', b => { 
				b.MemoryPointer++;
				if (b.MemoryPointer >= b.Memory.Length) b.MemoryPointer = 0;
			});

			vm.RegisterCommand('<', b => {
				b.MemoryPointer--;
				if (b.MemoryPointer < 0) b.MemoryPointer = b.Memory.Length - 1;
			});

			var symbols = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
			foreach (var symbol in symbols)
			{
				vm.RegisterCommand(symbol, b => b.Memory[b.MemoryPointer] = (byte)symbol);
			}
		}
	}
}