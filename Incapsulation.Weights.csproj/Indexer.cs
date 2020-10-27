using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Weights
{
	public class Indexer
	{
		private readonly double[] arr;
		private readonly int start;
		public int Length { get; }

		public Indexer(double[] arr, int start, int length)
		{
			if (start < 0 || length < 0 || start + length > arr.Length)
				throw new ArgumentException();
			this.arr = arr;
			this.start = start;
			Length = length;
		}

		public double this[int index]
		{
			get
			{
				if (index < 0 || index >= Length)
					throw new IndexOutOfRangeException();
				return arr[start + index];
			}
			set
			{
				if (index < 0 || index >= Length)
					throw new IndexOutOfRangeException();
				arr[start + index] = value;
			}
		}
	}
}
