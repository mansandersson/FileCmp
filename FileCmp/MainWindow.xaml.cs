/*
    Copyright (c) 2012, Måns Andersson <mail@mansandersson.se>

    Permission to use, copy, modify, and/or distribute this software for any
    purpose with or without fee is hereby granted, provided that the above
    copyright notice and this permission notice appear in all copies.

    THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
    WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
    MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
    ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
    WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
    ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
    OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/
using System;
using System.Windows;
using System.Windows.Controls;

namespace FileCmp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Int32 _numberOfItems = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Drop(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            // Do something with the data...
            foreach (string file in fileList)
            {
                FileItem fileItem = new FileItem(file);
                Console.WriteLine(fileItem.Hash);
                FileControl newFileControl = new FileControl();
                newFileControl.FileItem = fileItem;
                newFileControl.MouseOver = MainWindow_MouseOver;

                this.RootGrid.Children.Add(newFileControl);
                if (_numberOfItems > 0)
                {
                    this.RootGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100) });
                }
                Grid.SetRow(newFileControl, _numberOfItems++);
            }
        }

        private void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
        }

        private void MainWindow_MouseOver(string hash)
        {
            foreach (UIElement elm in this.RootGrid.Children)
            {
                if (elm is FileControl)
                {
                    ((FileControl)elm).Highlight(hash);
                }
            }
        }
    }
}
