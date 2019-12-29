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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WpfJsonViewer.ViewModel
{
    /// <summary>
    /// Represents a view on a JSON item which can be used in a view model.
    /// </summary>
    public class JsonViewItem
    {
        private readonly JsonItem _item;

        /// <summary>
        /// Creates a new view item using a JSON item.
        /// </summary>
        /// <param name="item">the item containing the data</param>
        internal JsonViewItem(JsonItem item)
        {
            _item = item;
        }

        /// <summary>
        /// The label for this item.
        /// </summary>
        public string Label
        {
            get
            {
                if (_item is JsonElement)
                {
                    return @"JSON";
                }
                if (_item is JsonItemArray)
                {
                    return @"ARRAY";
                }
                if (_item is JsonProperty jsonProperty)
                {
                    return jsonProperty.Name;
                }

                return (null != _item.Value) ? _item.Value.ToString() : @"NULL";
            }
        }

        public ReadOnlyCollection<JsonViewItem> Children
        {
            get
            {
                if (_item is JsonElement jsonElement)
                {
                    return new ReadOnlyCollection<JsonViewItem>((from property in jsonElement.Properties.Values
                                                                 select new JsonViewItem(property)).ToList());
                }
                if (_item is JsonItemArray jsonItemArray)
                {
                    var containerItems = new List<JsonViewItem>();
                    foreach (var item in jsonItemArray.Items)
                    {
                        if (item is JsonItem jsonItem)
                        {
                            containerItems.Add(new JsonViewItem(jsonItem));
                        }
                    }
                    return new ReadOnlyCollection<JsonViewItem>(containerItems);
                }
                if (_item is JsonProperty jsonProperty)
                {
                    var propertyValue = jsonProperty.Value;
                    if (propertyValue is JsonElement jsonElementValue)
                    {
                        return new ReadOnlyCollection<JsonViewItem>(new List<JsonViewItem> { new JsonViewItem(jsonElementValue) });
                    }
                    if (propertyValue is JsonItemArray jsonItemArrayValue)
                    {
                        return new ReadOnlyCollection<JsonViewItem>(new List<JsonViewItem> { new JsonViewItem(jsonItemArrayValue) });
                    }
                }

                return new ReadOnlyCollection<JsonViewItem>(new List<JsonViewItem>());
            }
        }
    }
}
