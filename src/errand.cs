using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Digests;
using ServiceStack.Redis;
using SHA3.Net;
using SHA3Core.Enums;

namespace ConsoleCore
{
    static class errand
    {
        internal static void crawl_sachvui()
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            Parallel.For(1, 36, i =>
            {
                parse_site($"https://sachvui.com/doc-sach/bay-tren-to-chim-cuc-cu-ken-kesey/chuong-{i}.html", i);
            });
            //Enumerable.Range(1, 35)
        }

        static string template_nav = @"
<input type='button' value='chuong-{0}' onclick='location.href = ""chuong-{0}.html"";' />
<select onChange='location.href = this.value'>
{1}
</select>
<input type='button' value='chuong-{2}' onclick='location.href = ""chuong-{2}.html"";' />"
;
        static string template_head = @"
<head>
  <meta charset='utf-8' />
  <meta name='viewport' content='width=device-width, initial-scale=1'>
  <title>One flew over the Cuckoo's nest</title>
  <link rel='icon' href='notes.png'>

  <link rel='stylesheet' href='style.css' />
</head>"
;
        static void parse_site(string url, int i)
        {
            StringBuilder sb = new StringBuilder();
            for (int index = 1; index < 36; ++index)
                sb.AppendLine($"  <option value='chuong-{index}.html'{(index == i ? " selected" : null)}>{index}</option>");
            
            var content = DownloadSite(url);
            var m = Regex.Match(content, "\\<div class=\"chapter-c\"\\>.+?\\</div\\>");
            if (m.Success)
            {
                var nav = string.Format(template_nav, i - 1, sb.ToString(), i + 1);
                var new_content = template_head + "\n" + nav + "\n<br/><br/>\n" + m.Value + "\n<br/>\n" + nav;
                File.WriteAllText(Path.GetFileName(url), new_content);
                Console.WriteLine($"{Path.GetFileName(url)} - SUCCESS!!!");
            }
            else
                Console.WriteLine($"{Path.GetFileName(url)} - FAILED!!!");
        }
        static string DownloadSite(string url)
        {
            using (var client = new WebClient())
            {
                //client.Proxy = new WebProxy("cacheproxy.hcnet.vn", 8080);
                client.Proxy = WebRequest.DefaultWebProxy;
                client.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;

                //client.Proxy.Credentials = client.Credentials = new NetworkCredential("khanh.buid", "listeN2mysoul");
                //client.Encoding = System.Text.Encoding.UTF8;
                return client.DownloadString(url);
            }
        }
        
        internal static void test()
        {
            while (true)
            {
                Console.Write("Input number of door: ");
                int n = int.Parse(Console.ReadLine());
                hundredDoor(n);
            }
        }

        static async Task<string> taskAsync()
        {
            await Task.CompletedTask;
            if (2 > 1)
                throw new Exception("aaaa");
            return "aabbc";
        }
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
              elements.SelectMany((e, i) =>
                elements
                    .Skip(i + 1)
                    .Combinations(k - 1)
                    .Select(c => (new[] { e }).Concat(c)));
        }
        static void GetCombination(List<int> list)
        {
            double count = Math.Pow(2, list.Count);
            for (int i = 1; i <= count - 1; i++)
            {
                string str = Convert.ToString(i, 2).PadLeft(list.Count, '0');
                for (int j = 0; j < str.Length; j++)
                {
                    if (str[j] == '1')
                    {
                        Console.Write(list[j]);
                    }
                }
                Console.WriteLine();
            }
        }

        public static void ReplaceLineEnding()
        {
            string ext = ".cs,.cshtml,.aspx,.css,.js,.html,.htm,.xml,.txt,.ascx,.asax,.skin,.resx,.sitemap,.Master";
            string path = @"E:\Downloads\dms-dotnet\src\DMS";
            var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(f => ext.Contains(Path.GetExtension(f)));

            foreach(string f in files)
            {
                File.WriteAllText(f,
                    string.Join('\n', File.ReadAllLines(f))
                );
            }
        }

        public static void hundredDoor(int n)
        {
            bool[] doors = new bool[n];
            for (int i = 1; i <= n; ++i)
            {
                for (int j = i - 1; j < n; j += i)
                {
                    doors[j] = !doors[j];
                }
                Console.Write($"{i:0#} -- ");
                for (int k = 1; k <= n; ++k)
                {
                    Console.Write(doors[k - 1] ? 1 : 0);
                    if ((k & (k - 1)) == 0) Console.Write(' ');
                }
                Console.WriteLine();
            }

            Console.WriteLine($"open: {doors.Count(d => d)}");
        }
        
        public static void HashSha3Digest()
        {
            var input = "The string to hash";
            var inputBytes = Encoding.UTF8.GetBytes(input);

            var sha3 = new Sha3Digest(256);
            sha3.BlockUpdate(inputBytes, 0, inputBytes.Length);
            var hash = new byte[sha3.GetDigestSize()];
            sha3.DoFinal(hash, 0);

            string hashString = BitConverter.ToString(hash).Replace("-", "");
            Console.WriteLine(hashString);
        }
        
        public static void HashSha3()
        {
            var input = "The string to hash";
            var inputBytes = Encoding.UTF8.GetBytes(input);

            using var shaAlg = Sha3.Sha3256();
            var hash = shaAlg.ComputeHash(inputBytes);
            var hashString = BitConverter.ToString(hash).Replace("-", "");
            Console.WriteLine(hashString);
        }
        
        public static void SHA3Core()
        {
            var input = "The string to hash";
            var inputBytes = Encoding.UTF8.GetBytes(input);

            var sha3 = new SHA3Core.SHA3.SHA3(SHA3BitType.S256);
            var hashString = sha3.Hash(inputBytes);
            Console.WriteLine(hashString);
        }
    }
    public class CA
    {
        public string prop1 { get; set; } = "defffault";
        public string prop2 { get; set; }
    }
}
