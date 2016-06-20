using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            wc.Headers.Add("VIA", "8.8.8.8");
            wc.Headers.Add("X_FORWARDED_FOR", "9.9.9.9");
            Console.WriteLine(wc.DownloadString("http://localhost/Jinyinmao.Tirisfal.Api/SendVeriCode"));
            Console.ReadLine();
        }
    }
}
