/*
 * Copyright 2019 Jan Tschada
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;

namespace Json.Data.Model
{
    /// <summary>
    /// Represents a JSON object instance.
    /// </summary>
    public class JsonElement : JsonItem
    {
        /// <summary>
        /// Creates a new JSON object instance.
        /// </summary>
        public JsonElement()
        {
            Properties = new Dictionary<string, JsonProperty>();
        }

        /// <summary>
        /// The properties of this element.
        /// </summary>
        public IDictionary<string, JsonProperty> Properties { get; }

        /// <summary>
        /// Creates a new property using an unique name.
        /// </summary>
        /// <param name="name">the name of the property</param>
        /// <returns><see cref="JsonItem"/></returns>
        public JsonProperty CreateProperty(string name)
        {
            var newProperty = new JsonProperty();
            newProperty.Name = name;
            Properties.Add(name, newProperty);
            return newProperty;
        }

        public override void AddValue(object value)
        {
            throw new InvalidOperationException("You cannot add a value to a JSON object instance!");
        }
    }
}
