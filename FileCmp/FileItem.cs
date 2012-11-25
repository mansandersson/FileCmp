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
    public class FileItem
    {
        public BitmapImage Icon
        {
            get
            {
                if (FilePath == null)
                    return null;

                Bitmap iconBitmap = System.Drawing.Icon.ExtractAssociatedIcon(FilePath).ToBitmap();
                MemoryStream iconStream = new MemoryStream();
                iconBitmap.Save(iconStream, ImageFormat.Png);
                BitmapImage iconImage = new BitmapImage();
                iconImage.BeginInit();
                iconImage.StreamSource = iconStream;
                iconImage.EndInit();
                return iconImage;
            }
        }
        public string FilePath { get; private set; }
        public string FileName
        {
            get
            {
                return Path.GetFileName(FilePath);
            }
        }
        public string Hash { get; private set; }
        private HashAlgorithms _hashAlgorithm = HashAlgorithms.UNKNOWN;
        public HashAlgorithms HashAlgorithm
        {
            get { return _hashAlgorithm; }
            set
            {
                if (_hashAlgorithm != value)
                {
                    Hash = CalcHash(FilePath, value);
                    _hashAlgorithm = value;
                }
            }
        }

        public FileItem(string path)
        {
            FilePath = path;
            HashAlgorithm = HashAlgorithms.MD5;
        }
        public FileItem(string path, HashAlgorithms algorithm)
        {
            FilePath = path;
            HashAlgorithm = algorithm;
        }

        private string CalcHash(string path, HashAlgorithms algorithm)
        {
            System.Security.Cryptography.HashAlgorithm cryptoProvider = null;
            switch (algorithm)
            {
                default:
                    return String.Empty;
                case HashAlgorithms.MD5:
                    cryptoProvider = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    break;
                case HashAlgorithms.SHA1:
                    cryptoProvider = new System.Security.Cryptography.SHA1CryptoServiceProvider();
                    break;
            }

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] result = cryptoProvider.ComputeHash(stream);
                return BitConverter.ToString(result).Replace("-", "").ToLower();
            }
        }
    }
}
