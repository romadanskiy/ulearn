using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace rocket_bot
{
    public class Channel<T> where T : class
    {
        private readonly List<T> channel;

        public Channel()
        {
            channel = new List<T>();
        }

        /// <summary>
        /// Возвращает элемент по индексу или null, если такого элемента нет.
        /// При присвоении удаляет все элементы после.
        /// Если индекс в точности равен размеру коллекции, работает как Append.
        /// </summary>
        public T this[int index]
        {
            get
            {
                lock (channel)
                {
                    if (index < 0 || index >= channel.Count)
                        return null;
                    return channel[index];
                }
            }
            set
            {
                lock (channel)
                {
                    if (index == channel.Count)
                        channel.Add(value);
                    else
                    {
                        channel[index] = value;
                        for (var i = channel.Count - 1; i > index; i--)
                            channel.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает последний элемент или null, если такого элемента нет
        /// </summary>
        public T LastItem()
        {
            lock (channel)
            {
                if (channel.Count == 0) return null;
                return channel[channel.Count - 1];
            }
        }

        /// <summary>
        /// Добавляет item в конец только если lastItem является последним элементом
        /// </summary>
        public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
        {
            lock (channel)
            {
                if (channel.Count == 0)
                    channel.Add(item);
                else if (channel[channel.Count - 1].Equals(knownLastItem))
                    channel.Add(item);
            }
        }

        /// <summary>
        /// Возвращает количество элементов в коллекции
        /// </summary>
        public int Count
        {
            get
            {
                lock (channel)
                {
                    return channel.Count;
                }
            }
        }
    }
}