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

using Json.Data.Model;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using JsonElement = Json.Data.Model.JsonElement;

namespace Json.Data.IO
{
    /// <summary>
    /// Represents a Json file on disk.
    /// </summary>
    public static class JsonFile
    {
        /// <summary>
        /// Reads the content of the Json file.
        /// </summary>
        /// <param name="filePath">path to the file</param>
        /// <returns><see cref="IEnumerable{JsonItem}"/> for all contained items.</returns>
        /// <remarks>An <see cref="Utf8JsonReader"/> instance cannot be created when using yield return iterator block.</remarks>
        public static IEnumerable<JsonItem> Parse(string filePath)
        {
            var rootItems = new List<JsonItem>();

            var buffer = File.ReadAllBytes(filePath);
            var itemStack = new Stack<JsonItem>();
            JsonItem currentProperty = null;

            var byteSequence = new ReadOnlySequence<byte>(buffer, 0, buffer.Length);
            var reader = new Utf8JsonReader(byteSequence);
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartObject:
                        var newElement = new JsonElement();
                        if (null != currentProperty)
                        {
                            currentProperty.AddValue(newElement);
                        }
                        else if (0 < itemStack.Count)
                        {
                            var objectParent = (JsonItemArray)itemStack.Peek();
                            objectParent.AddValue(newElement);
                        }
                        itemStack.Push(newElement);
                        currentProperty = null;
                        break;
                    case JsonTokenType.EndObject:
                        var poppedObject = itemStack.Pop();
                        if (0 == itemStack.Count)
                        {
                            rootItems.Add(poppedObject);
                        }
                        break;
                    case JsonTokenType.StartArray:
                        var newArray = new JsonItemArray();
                        if (null != currentProperty)
                        {
                            currentProperty.AddValue(newArray);
                        }
                        else if (0 < itemStack.Count)
                        {
                            var arrayParent = (JsonItemArray)itemStack.Peek();
                            arrayParent.AddValue(newArray);
                        }
                        itemStack.Push(newArray);
                        currentProperty = null;
                        break;
                    case JsonTokenType.EndArray:
                        var poppedArray = itemStack.Pop();
                        if (0 == itemStack.Count)
                        {
                            rootItems.Add(poppedArray);
                        }
                        break;
                    case JsonTokenType.PropertyName:
                        var propertyName = reader.GetString();
                        var propertyParent = (JsonElement)itemStack.Peek();
                        currentProperty = propertyParent.CreateProperty(propertyName);
                        break;

                    case JsonTokenType.String:
                        var stringValue = reader.GetString();
                        if (null != currentProperty)
                        {
                            currentProperty.AddValue(stringValue);
                        }
                        else
                        {
                            var stringParent = itemStack.Peek();
                            stringParent.AddValue(stringValue);
                        }
                        break;

                    case JsonTokenType.Number:
                        var doubleValue = reader.GetDouble();
                        if (null != currentProperty)
                        {
                            currentProperty.AddValue(doubleValue);
                        }
                        else
                        {
                            var doubleParent = itemStack.Peek();
                            doubleParent.AddValue(doubleValue);
                        }
                        break;
                }
            }
            return rootItems;
        }
    }
}
