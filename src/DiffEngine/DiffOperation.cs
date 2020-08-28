using LoxSmoke.DiffEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxSmoke.DiffEngine
{
    /// <summary>
    /// Class representing one diff operation (equal, insert or delete) in the list of diff operations.
    /// </summary>
    public class DiffOperation<T> where T : class, IItemSequence<T>
    {
        /// <summary>
        /// One of: Insert, Delete or Equal.
        /// </summary>
        public DiffOperationType Operation { get; set; }

        /// <summary>
        /// Operation type as an upper case string.
        /// </summary>
        public string OperationText
        {
            get
            {
                switch (Operation)
                {
                    case DiffOperationType.Delete:
                        return "DELETE";
                    case DiffOperationType.Insert:
                        return "INSERT";
                }
                return "EQUAL";
            }
        }

        /// <summary>
        /// The data associated with this diff operation. It is the sequence of items that were either added, unchanged or deleted 
        /// from the original sequence. When comparing strings this property contains the sequence of characters.
        /// </summary>
        public T Contents { get; set; }

        /// <summary>
        /// True if this is an insert operation.
        /// </summary>
        public bool IsInsert => Operation == DiffOperationType.Insert;
        /// <summary>
        /// True if this is a delete operation.
        /// </summary>
        public bool IsDelete => Operation == DiffOperationType.Delete;
        /// <summary>
        /// True if this is an equal operation.
        /// </summary>
        public bool IsEqual => Operation == DiffOperationType.Equal;

        /// <summary>
        /// Initializes the diff with the provided values.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="contents"></param>
        public DiffOperation(DiffOperationType operation, T contents)
        {
            this.Operation = operation;
            this.Contents = contents;
        }

        /// <summary>
        /// Get the shallow copy of the object.
        /// </summary>
        /// <returns>The object copy</returns>
        public virtual DiffOperation<T> Clone()
        {
            return MemberwiseClone() as DiffOperation<T>;
        }

        /// <summary>
        /// Return a human-readable string of this Diff.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{OperationText}:{Contents.ToString()}";
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(Object obj)
        {
            return Equals(obj as DiffOperation<T>);
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="otherDiff">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(DiffOperation<T> otherDiff)
        {
            return !object.ReferenceEquals(otherDiff, null) &&
                otherDiff.Operation == this.Operation &&
                (object.ReferenceEquals(otherDiff.Contents, null) && 
                object.ReferenceEquals(this.Contents, null) ||
                !object.ReferenceEquals(otherDiff.Contents, null) && 
                !object.ReferenceEquals(this.Contents, null) && 
                Contents.Equals(otherDiff.Contents));
        }

        /// <summary>
        /// Get the hash code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Contents.GetHashCode() ^ Operation.GetHashCode();
        }

        /// <summary>
        /// Create the new list of diff operations containing this object.
        /// </summary>
        /// <returns></returns>
        public DiffOperationList<T> ToList()
        {
            return new DiffOperationList<T>(this);
        }
    }
}
