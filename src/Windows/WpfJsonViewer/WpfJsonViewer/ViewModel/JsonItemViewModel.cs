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

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WpfJsonViewer.ViewModel
{
    /// <summary>
    /// Represents the view model for JSON items.
    /// </summary>
    public class JsonItemViewModel : INotifyPropertyChanged
    {
        private readonly JsonViewItem _rootItem;
        private bool _expanded;
        private bool _selected;

        internal JsonItemViewModel(JsonViewItem rootItem)
        {
            _rootItem = rootItem;
            Children = new ReadOnlyCollection<JsonItemViewModel>(
                (from item in _rootItem.Children 
                 select new JsonItemViewModel(item)).ToList());
        }

        public string Label { get => _rootItem.Label; }

        public bool IsExpanded
        {
            get => _expanded;
            set
            {
                if (value != _expanded)
                {
                    _expanded = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsSelected
        {
            get => _selected;
            set
            {
                if (value != _selected)
                {
                    _selected = value;
                    OnPropertyChanged();
                }
            }
        }

        public ReadOnlyCollection<JsonItemViewModel> Children { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
