using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ddd.Infrastructure
{
	/// <summary>
	/// Базовый класс для всех Value типов.
	/// </summary>
	public class ValueType<T>
	{
		private static readonly PropertyInfo[] properties;
		
		static ValueType()
		{
			properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
		}
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
				return true;
			
			return obj is T && properties.All(p => Equals(p.GetValue(this), p.GetValue(obj)));
		}

		public bool Equals(T obj)
		{
			return Equals((object) obj);
		}

		public override int GetHashCode()
		{
			var hash = 0;
			var c = 0;
			foreach (var value in properties.Select(p => p.GetValue(this)).Where(v => v != null))
			{
				if (c % 2 == 0)
					hash ^= value.GetHashCode();
				else
					hash ^= ShiftAndWrap(value.GetHashCode(), 2);
				c++;
			}

			return hash;
		}
		
		private static int ShiftAndWrap(int value, int positions)
		{
			positions = positions & 0x1F;

			// Save the existing bit pattern, but interpret it as an unsigned integer.
			uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
			// Preserve the bits to be discarded.
			uint wrapped = number >> (32 - positions);
			// Shift and wrap the discarded bits.
			return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(typeof(T).Name);
			sb.Append("(");
			var c = 0;
			foreach (var property in properties.OrderBy(p => p.Name))
			{
				sb.Append($"{property.Name}: {property.GetValue(this)}");
				if (c < properties.Length - 1)
					sb.Append("; ");
				c++;
			}
			sb.Append(")");
			
			return sb.ToString();
		}
	}
}