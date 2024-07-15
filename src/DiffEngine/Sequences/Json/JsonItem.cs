using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable CS1591

namespace LoxSmoke.DiffEngine.Sequences.Json
{
    /// <summary>
    /// JSON item. 
    /// </summary>
    public class JsonItem
    {
        /// <summary>
        /// Token type.
        /// </summary>
        public JsonToken TokenType;
        /// <summary>
        /// Token depth. Useful for diff operations. 
        /// </summary>
        public int Depth;
        /// <summary>
        /// The token value.
        /// </summary>
        public object Value;

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as JsonItem);
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="token">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(JsonItem token)
        {
            if (token == null) return false;
            return TokenType == token.TokenType &&
                Depth == token.Depth &&
                (Value == null && token.Value == null ||
                Value != null && token.Value != null && Value.Equals(token.Value));
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region String conversions
        /// <summary>
        /// Debug-style string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Depth}: {TokenType} {Value}";
        }
        public string ToPrettyText(bool doIndent = true)
        {
            var indent = (Depth == 0 || !doIndent) ? "" : new string(' ', Depth);
            switch (TokenType)
            {
                case JsonToken.None:
                    return "";
                case JsonToken.StartObject:
                    return indent + "{";
                case JsonToken.StartArray:
                    return indent + "[";
                case JsonToken.StartConstructor:
                    return indent + "?";
                case JsonToken.PropertyName:
                    return indent + $"\"{Value}\": ";
                case JsonToken.Comment:
                    return indent + $"\\*{Value}*\\";
                case JsonToken.Raw:
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.Boolean:
                case JsonToken.Date:
                case JsonToken.Bytes:
                    return indent + $"{Value}";
                case JsonToken.String:
                    return indent + $"\"{Value}\"";
                case JsonToken.Null:
                    return indent + "null";
                case JsonToken.Undefined:
                    return indent + $"?{Value}?";
                case JsonToken.EndObject:
                    return indent + "}";
                case JsonToken.EndArray:
                    return indent + $"]";
                case JsonToken.EndConstructor:
                    return indent + $"Coonstructor {Value}";
            }
            return $"{Depth}: {TokenType} {Value}";
        }

        public bool IsStartToken
        {
            get
            {
                switch (TokenType)
                {
                    case JsonToken.StartObject:
                    case JsonToken.StartArray:
                        return true;
                }
                return false;
            }
        }
        public bool IsEndToken
        {
            get
            {
                switch (TokenType)
                {
                    case JsonToken.EndObject:
                    case JsonToken.EndArray:
                        return true;
                }
                return false;
            }
        }

        public bool IsValueToken
        {
            get
            {
                switch (TokenType)
                {
                    case JsonToken.Boolean:
                    case JsonToken.Raw:
                    case JsonToken.Integer:
                    case JsonToken.Float:
                    case JsonToken.String:
                    case JsonToken.Null:
                    case JsonToken.Date:
                    case JsonToken.Bytes:
                        return true;
                }
                return false;
            }
        }
        #endregion
    }
}
