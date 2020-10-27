using System;
using System.Collections.Generic;

namespace TodoApplication
{
    enum Operation { Add, Remove }

    public class ListModel<TItem>
    {
        private class HistoryItem
        {
            public TItem Item;
            public int Index;
            public Operation Operation;
        }
        private readonly LimitedSizeStack<HistoryItem> history;
        public List<TItem> Items { get; }
        public int Limit;

        public ListModel(int limit)
        {
            Items = new List<TItem>();
            Limit = limit;
            history = new LimitedSizeStack<HistoryItem>(limit);
        }

        public void AddItem(TItem item)
        {
            Items.Add(item);
            history.Push(new HistoryItem { Item = item, Index = Items.Count - 1, Operation = Operation.Add });
        }

        public void RemoveItem(int index)
        {
            history.Push(new HistoryItem { Item = Items[index], Index = index, Operation = Operation.Remove });
            Items.RemoveAt(index);
        }

        public bool CanUndo()
        {
            return history.Count != 0;
        }

        public void Undo()
        {
            if (CanUndo())
            {
                var historyItem = history.Pop();
                if (historyItem.Operation == Operation.Remove)
                    Items.Insert(historyItem.Index, historyItem.Item);
                else
                    Items.RemoveAt(historyItem.Index);
            }
        }
    }
}
