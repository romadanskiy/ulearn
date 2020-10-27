using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.Tables
{
    public class Table<TRow, TColumn, TValue>
    {
        public Indexer<TRow, TColumn, TValue> Open
        {
           get
           {
               var indexer = new Indexer<TRow, TColumn, TValue>(rows, columns, dict);
               indexer.GetEvent += OpenRule<TRow, TColumn, TValue>.Get;
               indexer.SetEvent += OpenRule<TRow, TColumn, TValue>.Set;
               return indexer;
           }
        }

        public Indexer<TRow, TColumn, TValue> Existed
        {
           get
           {
               var indexer = new Indexer<TRow, TColumn, TValue>(rows, columns, dict);
               indexer.GetEvent += ExistedRule<TRow, TColumn, TValue>.Get;
               indexer.SetEvent += ExistedRule<TRow, TColumn, TValue>.Set;
               return indexer;
           }
        }

        private readonly List<TRow> rows = new List<TRow>();
        private readonly List<TColumn> columns = new List<TColumn>();
        private readonly Dictionary<TRow, Dictionary<TColumn, TValue>> dict = new Dictionary<TRow, Dictionary<TColumn, TValue>>();

        public ReadOnlyCollection<TRow> Rows => rows.AsReadOnly();
        public ReadOnlyCollection<TColumn> Columns => columns.AsReadOnly();

        public void AddRow(TRow row)
        {
            if (!rows.Contains(row))
                rows.Add(row);
        }

        public void AddColumn(TColumn column)
        {
            if (!columns.Contains(column))
                columns.Add(column);
        }
    }

    public static class OpenRule<TRow, TColumn, TValue>
    {
        public static TValue Get(TRow key1, TColumn key2, List<TRow> rows, List<TColumn> columns, 
            Dictionary<TRow, Dictionary<TColumn, TValue>> dict)
        {
            if (!rows.Contains(key1) || !columns.Contains(key2))
                return default(TValue);
            if (!dict.ContainsKey(key1) || !dict[key1].ContainsKey(key2))
                return default(TValue);
            return dict[key1][key2];
        }
        
        public static void Set(TRow key1, TColumn key2, TValue value, List<TRow> rows, List<TColumn> columns,
            Dictionary<TRow, Dictionary<TColumn, TValue>> dict)
        {
            if (!rows.Contains(key1)) rows.Add(key1);
            if (!columns.Contains(key2)) columns.Add(key2);
            dict.Add(key1, new Dictionary<TColumn, TValue>() {{key2, value}});
        }
    }

    public static class ExistedRule<TRow, TColumn, TValue>
    {
        public static TValue Get(TRow key1, TColumn key2, List<TRow> rows, List<TColumn> columns,
            Dictionary<TRow, Dictionary<TColumn, TValue>> dict)
        {
            if (!rows.Contains(key1) || !columns.Contains(key2))
                throw new ArgumentException();
            if (!dict.ContainsKey(key1) || !dict[key1].ContainsKey(key2))
                return default(TValue);
            return dict[key1][key2];
        }
        
        public static void Set(TRow key1, TColumn key2, TValue value, List<TRow> rows, List<TColumn> columns,
            Dictionary<TRow, Dictionary<TColumn, TValue>> dict)
        {
            if (!rows.Contains(key1) || !columns.Contains(key2))
                throw new ArgumentException();
            dict.Add(key1, new Dictionary<TColumn, TValue>() {{key2, value}});
        }
    }

    public class Indexer<TKey1, TKey2, TValue>
    {
        private List<TKey1> rows;
        private List<TKey2> columns;
        private Dictionary<TKey1, Dictionary<TKey2, TValue>> dict;

        public delegate void SetHandler(TKey1 key1, TKey2 key2, TValue value, List<TKey1> rows, List<TKey2> columns,
            Dictionary<TKey1, Dictionary<TKey2, TValue>> dict);
        public event SetHandler SetEvent;
        
        public delegate TValue GetHandler(TKey1 key1, TKey2 key2, List<TKey1> rows, List<TKey2> columns,
            Dictionary<TKey1, Dictionary<TKey2, TValue>> dict);
        public event GetHandler GetEvent;

        public Indexer(List<TKey1> rows, List<TKey2> columns, Dictionary<TKey1, Dictionary<TKey2, TValue>> dict)
        {
            this.rows = rows;
            this.columns = columns;
            this.dict = dict;
        }

        public TValue this[TKey1 key1, TKey2 key2]
        {
            get => GetEvent.Invoke(key1, key2, rows, columns, dict);
            set => SetEvent.Invoke(key1, key2, value, rows, columns, dict);
        }
    }
    
    /*

    public interface ITableIndexing<in TKey1, in TKey2, TValue>
    {
        TValue this[TKey1 key1, TKey2 key2]
        {
            get;
            set;
        }
    }

    public class Open<TKey1, TKey2, TValue> : ITableIndexing<TKey1, TKey2, TValue>
    {
        private List<TKey1> rows;
        private List<TKey2> columns;
        private Dictionary<TKey1, Dictionary<TKey2, TValue>> dict;
        
        public Open(List<TKey1> rows, List<TKey2> columns, Dictionary<TKey1, Dictionary<TKey2, TValue>> dict)
        {
            this.rows = rows;
            this.columns = columns;
            this.dict = dict;
        }

        public TValue this[TKey1 key1, TKey2 key2]
        {
            get
            {
                if (!rows.Contains(key1) || !columns.Contains(key2))
                    return default(TValue);
                if (!dict.ContainsKey(key1) || !dict[key1].ContainsKey(key2))
                    return default(TValue);
                return dict[key1][key2];
            }
            set
            {
                if (!rows.Contains(key1)) rows.Add(key1);
                if (!columns.Contains(key2)) columns.Add(key2);
                dict.Add(key1, new Dictionary<TKey2, TValue>() {{key2, value}});
            }
        }
    }

    public class Existed<TKey1, TKey2, TValue> : ITableIndexing<TKey1, TKey2, TValue>
    {
        private List<TKey1> rows;
        private List<TKey2> columns;
        private Dictionary<TKey1, Dictionary<TKey2, TValue>> dict;
        
        public Existed(List<TKey1> rows, List<TKey2> columns, Dictionary<TKey1, Dictionary<TKey2, TValue>> dict)
        {
            this.rows = rows;
            this.columns = columns;
            this.dict = dict;
        }

        public TValue this[TKey1 key1, TKey2 key2]
        {
            get
            {
                if (!rows.Contains(key1) || !columns.Contains(key2))
                    throw new ArgumentException();
                if (!dict.ContainsKey(key1) || !dict[key1].ContainsKey(key2))
                    return default(TValue);
                return dict[key1][key2];
            }
            set
            {
                if (!rows.Contains(key1) || !columns.Contains(key2))
                    throw new ArgumentException();
                dict.Add(key1, new Dictionary<TKey2, TValue>() {{key2, value}});
            }
        }
    }
    
    */
}
