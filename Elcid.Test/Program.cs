using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elcid.Utilities;

namespace Elcid.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var ftp = new FtpHelper("192.168.12.128", null, "ftpuser", "GGyy12#$");
            ftp.Download("C:\\123\\", "pacsimg");
        }
    }
}
