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
using System.Text.Json.Serialization;

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
        public static IEnumerable<JsonItem> Parse(string filePath)
        {
            const int bytes = 2048;
            using (var fileStream = File.OpenRead(filePath))
            {
                var buffer = new byte[bytes];
                int bytesRead;
                while (0 < (bytesRead = fileStream.Read(buffer, 0, buffer.Length)))
                {
                    var byteSequence = new ReadOnlySequence<byte>(buffer, 0, bytesRead);
                    var reader = new Utf8JsonReader(byteSequence);
                    while (reader.Read())
                    {
                        switch (reader.TokenType)
                        {
                            case JsonTokenType.StartObject:
                                yield return new JsonItem();
                                break;
                        }
                    }
                }
            }
        }
    }
}
