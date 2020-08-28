using LoxSmoke.DiffEngine.Sequences.Generic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LoxSmoke.DiffEngine.Sequences.Json
{
    /// <summary>
    /// The list of JSON item objects.
    /// </summary>
    public class JsonItemSequence :
        ItemList<JsonItemSequence, JsonItem>
    {
        /// <summary>
        /// Load data from specified JSON input file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static JsonItemSequence Load(string fileName)
        {
            var diffJson = new JsonItemSequence();
            using (var streamReader = new StreamReader(File.Open(fileName, FileMode.Open)))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                while (jsonReader.Read())
                {
                    diffJson.Data.Add(new JsonItem()
                    {
                        TokenType = jsonReader.TokenType,
                        Value = jsonReader.Value,
                        Depth = jsonReader.Depth
                    });
                }
            }
            return diffJson;
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj as JsonItemSequence);
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public new bool Equals(JsonItemSequence obj)
        {
            return (obj != null &&
                (obj.Data == null && Data == null ||
                obj.Data != null && Data != null && Data.SequenceEqual(obj.Data)));
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
