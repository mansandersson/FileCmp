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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace FileCmp
{
    public class PreComputedHashItem : IHashItem
    {
        public BitmapImage Icon { get { return null; } }
        public string Content { get; private set; }
        public string Hash { get; private set; }
        private HashAlgorithms _hashAlgorithm = HashAlgorithms.UNKNOWN;
        public HashAlgorithms HashAlgorithm { get { return _hashAlgorithm; } set { return; } }

        public PreComputedHashItem(string hashString, HashAlgorithms algorithm)
        {
            switch (algorithm)
            {
                default:
                    Content = String.Empty;
                    break;
                case HashAlgorithms.MD5:
                    Content = "md5sum string";
                    break;
                case HashAlgorithms.SHA1:
                    Content = "sha1sum string";
                    break;
            }
            Hash = hashString;
            _hashAlgorithm = algorithm;
        }
    }
}
