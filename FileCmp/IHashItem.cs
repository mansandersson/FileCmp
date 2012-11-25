using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FileCmp
{
    public interface IHashItem
    {
        BitmapImage Icon { get; }
        String Content { get; }
        string Hash { get; }
        HashAlgorithms HashAlgorithm { get; set; }
    }
}
