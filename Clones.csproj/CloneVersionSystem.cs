using System;
using System.Collections.Generic;

namespace Clones
{
    public class Node<T>
    {
        public int Value;
        public Node<T> Prev;
    }
    public class Stack<T>
    {
        private Node<T> tail;
        private Node<T> head;
        private int count;

        public Stack<T> Clone()
        {
            return new Stack<T> { tail = this.tail, head = this.head, count = this.count };
        }

        public void Push(int item)
        {
            if (tail == null)
                head = tail = new Node<T> { Value = item, Prev = null };
            else
            {
                var node = new Node<T> { Value = item, Prev = head };
                head = node;
            }
            count++;
        }

        public int Pop()
        {
            if (tail == null) throw new InvalidOperationException();
            var result = head.Value;
            head = head.Prev;
            if (head == null)
                tail = null;
            count--;

            return result;
        }

        public int Count { get { return count; } }

        public int Peek()
        {
            if (tail == null) throw new InvalidOperationException();

            return head.Value;
        }

        public Stack<T> Clear()
        {
            return new Stack<T> { tail = null, head = null, count = 0 };
        }

        public bool Contains(int item)
        {
            var currentItem = head;
            while (currentItem != null)
            {
                if (currentItem.Value.Equals(item))
                    return true;
                currentItem = currentItem.Prev;
            }

            return false;
        } 
    }

    public class Clone
    {
        public Stack<int> RolledBackPrograms;
        public Stack<int> LearnedPrograms;
    }

    public class CloneVersionSystem : ICloneVersionSystem
    {
        readonly List<Clone> clonesList = new List<Clone>()
        {
            new Clone { RolledBackPrograms = new Stack<int>(), LearnedPrograms = new Stack<int>() }
        };

        public string Execute(string query)
        {

            string command = query.Split(' ')[0];
            int cloneNum = int.Parse(query.Split(' ')[1]) - 1;
            
            if (command == "learn")
            {
                int programToLearn = int.Parse(query.Split(' ')[2]);
                clonesList[cloneNum].RolledBackPrograms.Clear();
                if (!clonesList[cloneNum].LearnedPrograms.Contains(programToLearn))
                    clonesList[cloneNum].LearnedPrograms.Push(programToLearn);
                
                return null;
            }

            if (command == "rollback")
            {
                clonesList[cloneNum].RolledBackPrograms.Push(clonesList[cloneNum].LearnedPrograms.Pop());
                
                return null;
            }

            if (command == "relearn")
            {
                clonesList[cloneNum].LearnedPrograms.Push(clonesList[cloneNum].RolledBackPrograms.Pop());
                
                return null;
            }

            if (command == "clone")
            {
                clonesList.Add(new Clone
                {
                    LearnedPrograms = clonesList[cloneNum].LearnedPrograms.Clone(),
                    RolledBackPrograms = clonesList[cloneNum].RolledBackPrograms.Clone()
                });
            }

            if (command == "check")
            {
                if (clonesList[cloneNum].LearnedPrograms.Count == 0)
                    return "basic";
                
                return clonesList[cloneNum].LearnedPrograms.Peek().ToString();
            }

            return null;
        }
    }
}