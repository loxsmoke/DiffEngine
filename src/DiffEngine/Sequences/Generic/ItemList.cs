using LoxSmoke.DiffEngine.Extensions;
using LoxSmoke.DiffEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#pragma warning disable CS1591

namespace LoxSmoke.DiffEngine.Sequences.Generic
{
    /// <summary>
    /// The generic implementation of the item sequence. Implementing class inherits this interface with itself and the item 
    /// type as generic parameters. For example class MySequence : ItemList&lt;MySequence, ItemType&gt; {...}
    /// </summary>
    /// <typeparam name="T1">The sequence type itself.</typeparam>
    /// <typeparam name="T2">The sequence item type. Can be a value type or class which implements Equals() method.</typeparam>
    public class ItemList<T1, T2> : IItemSequence<T1>
        where T1 : class, IItemSequence<T1>
    {
        /// <summary>
        /// The list of the items.
        /// </summary>
        public List<T2> Data { get; set; } = new List<T2>();

        /// <summary>
        /// The number of items in the list.
        /// </summary>
        public int Length => Data.Count;

        /// <summary>
        /// Clone this object and replace the item list with the new one.
        /// </summary>
        /// <param name="data">The new item list</param>
        /// <returns>The clone</returns>
        protected virtual T1 CloneWith(List<T2> data)
        {
            var clone = MemberwiseClone() as ItemList<T1, T2>;
            clone.Data = data;
            return clone as T1;
        }

        /// <summary>
        /// Returns the common prefix of two lists.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns>The common prefix</returns>
        public virtual T1 CommonPrefix(T1 sequence)
        {
            return CloneWith(Data.CommonPrefix((sequence as ItemList<T1, T2>).Data));
        }

        /// <summary>
        /// Returns the common suffix of two lists.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns>The common suffix</returns>
        public virtual T1 CommonSuffix(T1 sequence)
        {
            return CloneWith(Data.CommonSuffix((sequence as ItemList<T1, T2>).Data));
        }

        /// <summary>
        /// Return the concatenation of two sequences.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns>The combined sequence</returns>
        public virtual T1 Concat(T1 sequence)
        {
            var otherSequence = sequence as ItemList<T1, T2>;
            if (object.ReferenceEquals(otherSequence, null))  return this as T1;
            return CloneWith(Data.Concat(otherSequence.Data));
        }

        /// <summary>
        /// True if list ends with specified sequence.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public virtual bool EndsWith(T1 sequence)
        {
            return Data.EndsWith((sequence as ItemList<T1, T2>).Data);
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public virtual bool Equals(T1 other)
        {
            var sequence = other as ItemList<T1, T2>;
            return Data.SequenceEqual(sequence.Data);
        }

        public virtual int CommonOverlapLength(T1 sequence)
        {
            return Data.CommonOverlapLength((sequence as ItemList<T1, T2>).Data);
        }

        public virtual int IndexOf(T1 sequence, int startFrom = 0)
        {
            return Data.IndexOf((sequence as ItemList<T1, T2>).Data, startFrom);
        }

        public virtual T1 Mid(int from, int length)
        {
            return CloneWith(Data.GetRange(from, length));
        }

        public virtual bool StartsWith(T1 sequence)
        {
            return Data.StartsWith((sequence as ItemList<T1, T2>).Data);
        }

        public bool ItemEquals(int index, T1 otherSequence, int otherIndex)
        {
            return Data[index].Equals((otherSequence as ItemList<T1, T2>).Data[otherIndex]);
        }
    }
}
