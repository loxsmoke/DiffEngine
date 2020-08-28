using LoxSmoke.DiffEngine.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#pragma warning disable CS1591

namespace LoxSmoke.DiffEngine.Sequences.Xml
{
    /// <summary>
    /// XML item.
    /// </summary>
    public class XmlItem
    {
        public XmlNodeType NodeType { get; set; }
        public int Depth { get; set; }
        public bool Empty { get; set; }
        public bool HasAttributes { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public XmlItem(XmlNodeType type, int depth, bool empty, bool hasAttributes, string name, string value)
        {
            this.NodeType = type;
            this.Depth = depth;
            this.Empty = empty;
            this.HasAttributes = hasAttributes;
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public XmlItem()
        { }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as XmlItem);
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="token">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(XmlItem token)
        {
            if (object.ReferenceEquals(token, null)) return false;
            return NodeType == token.NodeType &&
                Depth == token.Depth &&
                Empty == token.Empty &&
                HasAttributes == token.HasAttributes &&
                Name == token.Name &&
                Value == token.Value;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            switch (NodeType)
            {
                // An element (for example, &lt;item&gt; ).
                case XmlNodeType.Element: // name
                    if (HasAttributes) return $"<{Name}";
                    return Empty ? $"<{Name}/>" : $"<{Name}>";
                // An attribute (for example, id=&#39;123&#39; ).
                case XmlNodeType.Attribute: // name value
                    return $" {Name}=\"{Value}\"";
                // The text content of a node.
                case XmlNodeType.Text: // value
                    return Value;
                // A CDATA section (for example, &lt;![CDATA[my escaped text]]&gt; ).
                case XmlNodeType.CDATA:
                    return $"<![CDATA[{Value}]]>";
                // A reference to an entity (for example, &amp;num; ).
                case XmlNodeType.EntityReference:
                    return $"&{Value};";
                // An entity declaration (for example, &lt;!ENTITY...&gt; ).
                case XmlNodeType.Entity:
                    break;
                // A processing instruction (for example, &lt;?pi test?&gt; ).
                case XmlNodeType.ProcessingInstruction:
                    break;
                // A comment (for example, &lt;!-- my comment --&gt; ).
                case XmlNodeType.Comment:
                    return $"<!--{Value}-->";
                // A document object that, as the root of the document tree, provides access to
                // the entire XML document.
                case XmlNodeType.Document:
                    break;
                // The document type declaration, indicated by the following tag (for example, &lt;!DOCTYPE...&gt;).
                case XmlNodeType.DocumentType:
                    return $"<!DOCTYPE {Name}" + (string.IsNullOrEmpty(Value) ? ">" : $"[{Value}]>");
                // A document fragment.
                case XmlNodeType.DocumentFragment:
                    break;
                // A notation in the document type declaration (for example, &lt;!NOTATION...&gt;).
                case XmlNodeType.Notation:
                    break;
                // White space between markup.
                case XmlNodeType.Whitespace: // value
                    return Value;
                // White space between markup in a mixed content model or white space within the
                // xml:space=&quot;preserve&quot; scope.
                case XmlNodeType.SignificantWhitespace:
                    return Value;
                // An end element tag (for example, &lt;/item&gt; ).
                case XmlNodeType.EndElement: // name
                    return $"</{Name}>";
                // Returned when XmlReader gets to the end of the entity replacement as a result
                // of a call to System.Xml.XmlReader.ResolveEntity.
                case XmlNodeType.EndEntity:
                    break;
                // The XML declaration (for example, &lt;?xml version=&#39;1.0&#39;?&gt; ).
                case XmlNodeType.XmlDeclaration: // name
                    return HasAttributes ? $"<?xml" : $"<?xml?>";
            }
            return "";
        }

        /// <summary>
        /// Auto-generated hash code function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = -1593454109;
            hashCode = hashCode * -1521134295 + NodeType.GetHashCode();
            hashCode = hashCode * -1521134295 + Depth.GetHashCode();
            hashCode = hashCode * -1521134295 + Empty.GetHashCode();
            hashCode = hashCode * -1521134295 + HasAttributes.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Value);
            return hashCode;
        }
    }
}
