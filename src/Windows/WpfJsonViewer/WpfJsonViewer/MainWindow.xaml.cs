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

using Json.Data.IO;
using System.Collections.Generic;
using System.Windows;
using WpfJsonViewer.ViewModel;

namespace WpfJsonViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TreeView_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            var viewModels = new List<JsonItemViewModel>();
            foreach (var filePath in filePaths)
            {
                foreach (var jsonItem in JsonFile.Parse(filePath))
                {
                    var viewModel = new JsonItemViewModel(new JsonViewItem(jsonItem));
                    viewModels.Add(viewModel);
                }
            }

            if (0 < viewModels.Count)
            {
                DataContext = viewModels[0];
            }
        }
    }
}
