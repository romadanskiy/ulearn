using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace hashes
{
	public class ReadonlyBytes : IEnumerable<byte>
	{
		private readonly byte[] bytes;
		private readonly int hash;

		public ReadonlyBytes(params byte[] bytes)
		{
			if (bytes == null) throw new ArgumentNullException();
			this.bytes = bytes;

			const int fnv = 7511337;
			hash = 0;
			unchecked
			{
				foreach (var e in bytes)
				{
					hash ^= e.GetHashCode();
					hash *= fnv;
				}
			}
		}

		public byte this[int index]
		{
			get
			{
				if (index < 0 || index >= bytes.Length) throw new IndexOutOfRangeException();
				return bytes[index];
			}
			set
			{
				if (index < 0 || index >= bytes.Length) throw new IndexOutOfRangeException();
				bytes[index] = value;
			}
		}

		public int Length { get { return bytes.Length; } }

		public override bool Equals(object obj)
		{
			if (!(obj is ReadonlyBytes)) return false;
			if (obj.GetType().IsSubclassOf(typeof(ReadonlyBytes))) return false;
			
			var bytes2 = obj as ReadonlyBytes;
			//if (bytes.Length != bytes2.Length) return false;
			if (hash != bytes2.hash) return false;
			for (var i = 0; i < bytes.Length; i++)
				if (bytes[i] != bytes2[i]) return false;

			return true;
		}

		public override int GetHashCode()
		{
			return hash;
		}

		public override string ToString()
		{
			var str = new StringBuilder();
			str.Append("[");
			for (var i = 0; i < bytes.Length; i++)
			{
				str.Append(bytes[i]);
				if (i != bytes.Length - 1) str.Append(", ");
			}
			str.Append("]");

			return str.ToString();
		}

		public IEnumerator<byte> GetEnumerator()
		{
			for(var i = 0; i < bytes.Length; i++)
				yield return bytes[i];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

	}
}