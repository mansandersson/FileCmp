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
        private HashAlgorithms _currentAlgorithm = HashAlgorithms.MD5;

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
                FileItem fileItem = new FileItem(file, _currentAlgorithm);
                Console.WriteLine(fileItem.Hash);
                FileControl newFileControl = new FileControl();
                newFileControl.FileItem = fileItem;
                newFileControl.MouseOver = MainWindow_MouseOver;
                newFileControl.RemoveItem = RemoveItem;

                this.RootGrid.Children.Add(newFileControl);
                if (_numberOfItems > 0)
                {
                    this.RootGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100) });
                }
                Grid.SetRow(newFileControl, _numberOfItems++);
                boxDropHere.Visibility = Visibility.Hidden;
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

        private void RemoveItem(FileControl item)
        {
            Int32 rowIndex = Grid.GetRow(item);
            this.RootGrid.Children.Remove(item);
            foreach (UIElement elm in this.RootGrid.Children)
            {
                Int32 elmRowIndex = Grid.GetRow(elm);
                if (elmRowIndex > rowIndex)
                {
                    Grid.SetRow(elm, elmRowIndex - 1);
                }
            }
            if (this.RootGrid.RowDefinitions.Count > 1)
                this.RootGrid.RowDefinitions.RemoveAt(this.RootGrid.RowDefinitions.Count - 1);
            _numberOfItems--;

            if (_numberOfItems == 0)
                boxDropHere.Visibility = Visibility.Visible;
        }

        private void MainWindow_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ContextMenu menu = new ContextMenu();

            // md5sum menu item
            MenuItem md5sum = new MenuItem();
            md5sum.Header = "md5sum";
            if (_currentAlgorithm == HashAlgorithms.MD5)
                md5sum.FontWeight = FontWeights.Bold;
            md5sum.Click += md5sum_Click;
            menu.Items.Add(md5sum);

            // sha1sum menu item
            MenuItem sha1sum = new MenuItem();
            sha1sum.Header = "sha1sum";
            if (_currentAlgorithm == HashAlgorithms.SHA1)
                sha1sum.FontWeight = FontWeights.Bold;
            sha1sum.Click += sha1sum_Click;
            menu.Items.Add(sha1sum);

            menu.IsOpen = true;
        }

        private void sha1sum_Click(object sender, RoutedEventArgs e)
        {
            _currentAlgorithm = HashAlgorithms.SHA1;
            RecalculateHash();
        }

        private void md5sum_Click(object sender, RoutedEventArgs e)
        {
            _currentAlgorithm = HashAlgorithms.MD5;
            RecalculateHash();
        }

        private void RecalculateHash()
        {
            foreach (UIElement elm in this.RootGrid.Children)
            {
                if (elm is FileControl)
                {
                    FileControl fc = (FileControl)elm;
                    fc.FileItem.HashAlgorithm = _currentAlgorithm;
                    fc.Refresh();
                }
            }
        }
    }
}
