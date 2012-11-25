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
using System.Windows.Controls;
using System.Windows.Media;

namespace FileCmp
{
    /// <summary>
    /// Interaction logic for FileControl.xaml
    /// </summary>
    public partial class FileControl : UserControl
    {
        private IHashItem _hashItem;
        public IHashItem HashItem
        {
            get
            {
                return _hashItem;
            }
            set
            {
                _hashItem = value;
                this.LblFileName.Content = value.Content;
                this.LblHash.Content = value.Hash;
                this.ImgFileIcon.Source = value.Icon;
            }
        }
        public Action<string> MouseOver { get; set; }
        public Action<FileControl> RemoveItem { get; set; }

        public FileControl()
        {
            InitializeComponent();
        }

        public void Highlight(string hash)
        {
            if (hash == LblHash.Content.ToString())
                this.Background = new SolidColorBrush(Colors.LightGreen);
            else
                this.Background = new SolidColorBrush(Colors.Transparent);
        }

        public void Refresh()
        {
            this.LblHash.Content = HashItem.Hash;
        }

        private void FileControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MouseOver(this.LblHash.Content.ToString());
        }

        private void FileControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MouseOver("");
        }

        private void BtnCopy_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText(this.LblHash.Content.ToString(), System.Windows.TextDataFormat.Text);
        }

        private void BtnRemove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            RemoveItem(this);
        }
    }
}
