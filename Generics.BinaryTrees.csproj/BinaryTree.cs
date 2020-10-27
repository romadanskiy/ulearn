using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Generics.BinaryTrees
{
    public class Node<T> where T : IComparable<T>
    {
        public readonly T Value;
        public BinaryTree<T> Left { get; set; }
        public BinaryTree<T> Right { get; set; }

        public Node(T value)
        {
            Value = value;
        }
    }
    
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        private Node<T> root;
        public T Value => root.Value;
        public BinaryTree<T> Left => root.Left;
        public BinaryTree<T> Right => root.Right;

        public void Add(T value)
        {
            if (root == null)
            {
                root = new Node<T>(value);
            }
            else
            {
                if (Value.CompareTo(value) < 0)
                {
                    if (Right == null)
                        root.Right = new BinaryTree<T>();
                    root.Right.Add(value);
                }
                else
                {
                    if (Left == null)
                        root.Left = new BinaryTree<T>();
                    root.Left.Add(value);
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (root == null) yield break;
            
            if (Left != null)
            {
                foreach (var v in Left)
                {
                    yield return v;
                }
            }

            yield return Value;

            if (Right != null)
            {
                foreach (var v in Right)
                {
                    yield return v;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class BinaryTree
    {
        public static BinaryTree<T> Create<T>(params T[] values) where T : IComparable<T>
        {
            var tree = new BinaryTree<T>();
            foreach (var value in values)
            {
                tree.Add(value);
            }

            return tree;
        }
    }
}