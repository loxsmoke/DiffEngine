using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.DiffEngine.Interfaces
{
    /// <summary>
    /// Sequence of elements that can be compared.
    /// </summary>
    /// <typeparam name="T">
    /// Sequence of elements type. T must be a class and implement the IItemSequence interface. 
    /// </typeparam>
    public interface IItemSequence<T>
        where T : class, IItemSequence<T>
    {
        #region Query functions and properties
        /// <summary>
        /// Get length of the sequence. Empty sequence should return 0.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Return true if item at the specified index is equal to the  
        /// item at otherIndex position in other sequence.
        /// </summary>
        /// <param name="index">Item index in this sequence.</param>
        /// <param name="otherSequence">Other item sequence.</param>
        /// <param name="otherIndex">Item index in other sequence.</param>
        /// <returns>True if elements are equal. False if elements are not equal</returns>
        bool ItemEquals(int index, T otherSequence, int otherIndex);
        /// <summary>
        /// True if sequence starts with all elements in the specified sequence.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        bool StartsWith(T sequence);
        /// <summary>
        /// True if sequence ends with all elements from the specified sequence.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        bool EndsWith(T sequence);
       
        /// <summary>
        /// Find the specified sequence in this sequence.
        /// Start search from the specified position.
        /// </summary>
        /// <param name="sequence">Sequence to search for.</param>
        /// <param name="startFrom">Search start index.</param>
        /// <returns>The first occurrence of the specified sequence in this item sequence 
        /// or -1 if this sequence does not contains the specified sequence.</returns>
        int IndexOf(T sequence, int startFrom = 0);
        
        /// <summary>
        /// Get the common sequence length.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns>The number of common elements</returns>
        int CommonOverlapLength(T sequence);

        /// <summary>
        /// Determines whether two object instances are equal.
        /// Must be implemented for diff engine to work.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        bool Equals(T other);
        #endregion

        #region Manipulation methods
        /// <summary>
        /// Get the middle of the sequence.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        T Mid(int from, int length);
        /// <summary>
        /// Return the concatenation of this and the other object.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        T Concat(T sequence);
        /// <summary>
        /// Return the common prefix.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        T CommonPrefix(T sequence);
        /// <summary>
        /// Return the common suffix.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        T CommonSuffix(T sequence);
        #endregion
    }
}
