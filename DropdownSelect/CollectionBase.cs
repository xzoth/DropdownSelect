using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropdownSelect
{
    /// <summary>
    /// CollectionBase 泛型集合基类
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    public abstract class CollectionBase<T> : IList<T>, IList where T : class
    {
        private List<T> items = new List<T>();

        #region IList<T> 成员

        public int IndexOf(T item)
        {
            return this.items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.items.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return this.items[index];
            }
            set
            {
                this.items[index] = value;
            }
        }

        #endregion

        #region ICollection<T> 成员

        public void Add(T item)
        {
            this.items.Add(item);
        }

        public void Clear()
        {
            this.items.Clear();
        }

        public bool Contains(T item)
        {
            return this.items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return this.items.Remove(item);
        }

        #endregion

        #region IEnumerable<T> 成员

        public IEnumerator<T> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.items).GetEnumerator();
        }

        #endregion

        #region IList 成员

        int IList.Add(object value)
        {
            this.items.Add(value as T);
            return this.items.Count - 1;
        }

        void IList.Clear()
        {
            this.items.Clear();
        }

        bool IList.Contains(object value)
        {
            return this.items.Contains(value as T);
        }

        int IList.IndexOf(object value)
        {
            return this.items.IndexOf(value as T);
        }

        void IList.Insert(int index, object value)
        {
            this.items.Insert(index, value as T);
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return false; }
        }

        void IList.Remove(object value)
        {
            this.items.Remove(value as T);
        }

        void IList.RemoveAt(int index)
        {
            this.items.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return this.items[index];
            }
            set
            {
                this.items[index] = value as T;
            }
        }

        #endregion

        #region ICollection 成员

        void ICollection.CopyTo(Array array, int index)
        {

        }

        int ICollection.Count
        {
            get
            {
                return this.items.Count;
            }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return null; }
        }

        #endregion
    }
}
