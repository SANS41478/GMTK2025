using System.Linq;
namespace Space.Collections
{
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class SortedList<TKey,TValue>  where TKey : System.IComparable<TKey> 
    {
        [SerializeField] private List<(TKey,TValue)> _items = new List<(TKey,TValue)>();
        private bool _isDirty = true;
        private List<TValue> _sortedItems = new List<TValue>();
        public int Count => _items.Count;
    
        /// <summary>
        /// Add会导致列表变脏
        /// 下次获取前需要重排
        /// </summary>
        public void Add(TKey sourtKey,TValue item)
        {
            _items.Add((sourtKey,item));
            _isDirty = true;
        }
        /// <summary>
        /// remove的行为不会导致变脏
        /// </summary>
        public bool Remove(TValue item)
        {
            (TKey,TValue) data = _items.Find(data => data.Item2.Equals( item)); 
            bool sortRemoved=  _sortedItems.Remove(item);
            bool  removed=_items.Remove(data);
            return removed;
        }
        public void Clear()
        {
            _items.Clear();
            _isDirty = true;
        }
        /// <summary>
        /// 如果是脏的就重排
        /// 非脏直接返回排好的序列
        /// </summary>
        public IReadOnlyList<TValue> GetSorted()
        {
            if (_isDirty)
            {
                _items.Sort((a,b)=>a.Item1.CompareTo(b.Item1));
                _sortedItems.Clear();
                _sortedItems.AddRange( (_items.Select(data => data.Item2)));
                _isDirty = false;
            }
            return _sortedItems.AsReadOnly();
        }

        public TValue this[int index] => GetSorted()[index];
    }
}