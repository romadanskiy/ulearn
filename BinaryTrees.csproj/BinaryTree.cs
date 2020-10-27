using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrees
{
	public class TreeNode<T> where T : IComparable
	{
		public T Value;
		public TreeNode<T> Left, Right;
		public int LeftElemnts;

		public TreeNode(T value)
		{
			Value = value;
		}
	}

	public class BinaryTree<T> : IEnumerable<T> where T : IComparable
	{
		private TreeNode<T> root;
		private int count;

		public void Add(T key)
		{
			if (root == null)
			{ 
				root = new TreeNode<T>(key);
				count++;
			}
			else
			{
				var curNode = root;
				while (true)
				{
					if (key.CompareTo(curNode.Value) < 0)
					{
						curNode.LeftElemnts++;
						if (curNode.Left == null)
						{
							curNode.Left = new TreeNode<T>(key);
							count++;
							break;
						}
						curNode = curNode.Left;
					}
					else
					{
						if (curNode.Right == null)
						{
							curNode.Right = new TreeNode<T>(key);
							count++;
							break;
						}
						curNode = curNode.Right;
					}
				}
			}
		}

		public bool Contains(T key)
		{
			if (root == null)
				return false;

			var curNode = root;
			while(curNode != null)
			{
				if (key.CompareTo(curNode.Value) == 0)
					return true;
				curNode = key.CompareTo(curNode.Value) < 0 ? curNode.Left : curNode.Right;
			}

			return false;
		}

		public IEnumerator<T> GetEnumerator()
		{
			if (root == null) throw new InvalidOperationException();
			return GetValues(root).GetEnumerator();
		}

		private IEnumerable<T> GetValues(TreeNode<T> node)
		{
			if (node.Left != null)
				foreach (var value in GetValues(node.Left))
					yield return value;

			yield return node.Value;

			if (node.Right != null)
				foreach (var value in GetValues(node.Right))
					yield return value;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public T this[int index]
		{
			get
			{
				if (index < 0 || root == null || index >= count) throw new IndexOutOfRangeException();
				
				//var curNode = root;
				return FindElement(root, index);
				//while (true)
				//{
				//	var leftElements = curNode.LeftElemnts;
				//	if (leftElements == index) return curNode.Value;
				//	if (leftElements > index)
				//		curNode = curNode.Left;
				//	else
				//	{
				//		curNode = curNode.Right;
				//		index -= 1 + leftElements;
				//	}
				//}
			}
		}

		private T FindElement(TreeNode<T> node, int index)
		{
			var leftElements = node.LeftElemnts;
			if (leftElements == index)
				return node.Value;
			if (leftElements > index)
				return FindElement(node.Left, index);
			return FindElement(node.Right, index - (leftElements + 1));
		}
	}
}
