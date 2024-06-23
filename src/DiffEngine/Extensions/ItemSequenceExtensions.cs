using LoxSmoke.DiffEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.DiffEngine.Extensions
{
    /// <summary>
    /// Extension methods for the generic item sequences.
    /// </summary>
    public static class ItemSequenceExtensions
    {
        /// <summary>
        /// Check if item sequence is empty. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence">The sequence of items</param>
        /// <returns>True if sequence is empty, i.e. length is 0. False if length is non-zero</returns>
        public static bool IsEmpty<T>(this T sequence)
            where T : class, IItemSequence<T>
        {
            return sequence.Length == 0;
        }

        /// <summary>
        /// Get the sequence of elements on the left. 
        /// </summary>
        /// <param name="sequence">The sequence of items</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static T Left<T>(this T sequence, int count)
            where T : class, IItemSequence<T>
        {
            return sequence.Mid(0, count);
        }
        /// <summary>
        /// Get the copy of the seqnece with the specified number of items removed.
        /// </summary>
        /// <param name="sequence">The sequence of items</param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T LeftExcept<T>(this T sequence, int length)
            where T : class, IItemSequence<T>
        {
            return sequence.Mid(0, sequence.Length - length);
        }
        /// <summary>
        /// Get the part of the sequence starting from the specified 0-based index.
        /// </summary>
        /// <param name="sequence">The sequence of items</param>
        /// <param name="from"></param>
        /// <returns></returns>
        public static T RightFrom<T>(this T sequence, int from)
            where T : class, IItemSequence<T>
        {
            return sequence.Mid(from, sequence.Length - from);
        }

        /// <summary>
        /// Create "Insert" DiffOperation object from the specified item sequence
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence">The sequence of items</param>
        /// <returns>DiffOperation of the type Insert</returns>
        public static DiffOperation<T> AsInsert<T>(this T sequence)
            where T : class, IItemSequence<T>
        {
            return new DiffOperation<T>(DiffOperationType.Insert, sequence);
        }

        /// <summary>
        /// Create "Equal" DiffOperation object from the specified item sequence
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence">The sequence of items</param>
        /// <returns>DiffOperation of the type Equal</returns>
        public static DiffOperation<T> AsEqual<T>(this T sequence)
            where T : class, IItemSequence<T>
        {
            return new DiffOperation<T>(DiffOperationType.Equal, sequence);
        }

        /// <summary>
        /// Create "Delete" DiffOperation object from the specified item sequence
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence">The sequence of items</param>
        /// <returns>DiffOperation of the type Delete</returns>
        public static DiffOperation<T> AsDelete<T>(this T sequence)
            where T : class, IItemSequence<T>
        {
            return new DiffOperation<T>(DiffOperationType.Delete, sequence);
        }

        /// <summary>
        /// Create DiffOperation object from the specified item sequence
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence">The sequence of items</param>
        /// <param name="operation">The difference operation type</param>
        /// <returns>DiffOperation of the specified operation type</returns>
        public static DiffOperation<T> As<T>(this T sequence, DiffOperationType operation)
            where T : class, IItemSequence<T>
        {
            return new DiffOperation<T>(operation, sequence);
        }
    }
}
