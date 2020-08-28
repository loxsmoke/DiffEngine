using LoxSmoke.DiffEngine.Sequences.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace LoxSmoke.DiffEngine.Sequences.Xml
{
    /// <summary>
    /// Sequence of XML items.
    /// </summary>
    public class XmlItemSequence : ItemList<XmlItemSequence, XmlItem>
    {
        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as XmlItemSequence);
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(XmlItemSequence other)
        {
            if (other == null) return false;
            if (Data == null && other.Data == null) return true;
            return (Data != null && other.Data != null && Data.SequenceEqual(other.Data));
        }

        /// <summary>
        /// Load the XML file as a sequence of XML items.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="trimTextWhitespace"></param>
        /// <returns></returns>
        public static XmlItemSequence Load(string fileName, bool trimTextWhitespace)
        {
            using (var reader = File.OpenText(fileName))
            {
                return Load(reader, trimTextWhitespace);
            }
        }

        /// <summary>
        /// Read the XML string as a sequence of XML items.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="trimTextWhitespace"></param>
        /// <returns></returns>
        public static XmlItemSequence LoadFromString(string text, bool trimTextWhitespace)
        {
            var reader = new StringReader(text);
            return Load(reader, trimTextWhitespace);
        }

        /// <summary>
        /// Load the XML file as a sequence of XML items.
        /// </summary>
        /// <param name="textReader"></param>
        /// <param name="trimTextWhitespace"></param>
        /// <returns></returns>
        public static XmlItemSequence Load(TextReader textReader, bool trimTextWhitespace)
        {
            var xml = new XmlItemSequence();
            var xmlReader = XmlReader.Create(textReader, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse, IgnoreComments = false });
            while (xmlReader.MoveToNextAttribute() || xmlReader.Read())
            {
                var newNode = new XmlItem(xmlReader.NodeType, xmlReader.Depth, xmlReader.IsEmptyElement,
                    xmlReader.HasAttributes, xmlReader.Name, xmlReader.Value);
                xml.Data.Add(newNode);
                if (trimTextWhitespace &&
                    newNode.NodeType == XmlNodeType.Text &&
                    !string.IsNullOrEmpty(newNode.Value))
                {
                    newNode.Value = newNode.Value.Trim();
                }
            }
            return xml;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
