using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable CS1591

namespace LoxSmoke.DiffEngine
{
    /// <summary>
    /// The type of the diff operation
    /// </summary>
    public enum DiffOperationType
    {
        /// <summary>
        /// No change between two compared sequences
        /// </summary>
        Equal,
        /// <summary>
        /// New data inserted in the new sequence
        /// </summary>
        Insert,
        /// <summary>
        /// Data deleted in the original sequence
        /// </summary>
        Delete
    }
}
