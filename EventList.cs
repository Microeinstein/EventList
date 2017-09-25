using System;
using System.Collections.Generic;
using System.Linq;

namespace Micro.Utils {
    public class EventList<T> : List<T> {
        public delegate void ItemEventHandler(T item, int index);
        public delegate void CollectionEventHandler(IEnumerable<T> items, IEnumerable<int> indexes);
        public event ItemEventHandler ItemAdd, ItemSet, ItemRemove;
        public event CollectionEventHandler CollectionAdd, CollectionRemove;
        public event Action ListClear, ListReverse, ListSort;

        public new T this[int index] {
            get => base[index];
            set {
                base[index] = value;
                ItemSet?.Invoke(value, index);
            }
        }
        public new void Add(T item) {
            var i = Count;
            base.Add(item);
            ItemAdd?.Invoke(item, i);
        }
        public new void AddRange(IEnumerable<T> collection) {
            var i = Count;
            base.AddRange(collection);
            CollectionAdd?.Invoke(collection, Enumerable.Range(i, collection.Count()));
        }
        public new void Insert(int index, T item) {
            var i = Count;
            base.Insert(index, item);
            ItemAdd?.Invoke(item, i);
        }
        public new void InsertRange(int index, IEnumerable<T> collection) {
            base.InsertRange(index, collection);
            CollectionAdd?.Invoke(collection, Enumerable.Range(index, collection.Count()));
        }
        public new bool Remove(T item) {
            var i = IndexOf(item);
            if (i == -1)
                return false;
            RemoveAt(i);
            return true;
        }
        public new int RemoveAll(Predicate<T> match) {
            var e = FindAll(match);
            if (e.Count > 0) {
                int j = -1;
                var indexes =
                    from a in e
                    select j = IndexOf(a, j + 1);
                base.RemoveAll(match);
                CollectionRemove?.Invoke(e, indexes);
            }
            return e.Count;
        }
        public new void RemoveAt(int index) {
            var e = base[index];
            base.RemoveAt(index);
            ItemRemove?.Invoke(e, index);
        }
        public new void RemoveRange(int index, int count) {
            var c = GetRange(index, count);
            base.RemoveRange(index, count);
            CollectionRemove?.Invoke(c, Enumerable.Range(index, count));
        }
        public new void Clear() {
            base.Clear();
            ListClear?.Invoke();
        }
        public new void Reverse() {
            base.Reverse();
            ListReverse?.Invoke();
        }
        public new void Reverse(int index, int count) {
            base.Reverse(index, count);
            ListReverse?.Invoke();
        }
        public new void Sort() {
            base.Sort();
            ListSort?.Invoke();
        }
        public new void Sort(Comparison<T> comparison) {
            base.Sort(comparison);
            ListSort?.Invoke();
        }
        public new void Sort(IComparer<T> comparer) {
            base.Sort(comparer);
            ListSort?.Invoke();
        }
        public new void Sort(int index, int count, IComparer<T> comparer) {
            base.Sort(index, count, comparer);
            ListSort?.Invoke();
        }
        public void ClearEvents() {
            ItemAdd = null;
            ItemRemove = null;
        }
    }
}
