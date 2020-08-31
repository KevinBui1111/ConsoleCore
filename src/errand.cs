using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleCore
{
    static class errand
    {
        internal static void selectMany()
        {
            var y = "The,quick;brown;fox jumps over the lazy dog".Split();
            string[] a = new string[] { "123,456", "678,9090" };
            var x = a.SelectMany((e, index) => e.Split(',').Select(val => (index, val)));
            foreach (var e in x)
            {
                Console.WriteLine($"{e.index} - {e.val}");
            }
        }
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
            var b = change_primitive_type("2001/08/31T14:15:34.855Z", typeof(DateTime));
            //var b = DateTime.TryParse("01831 14:15:34.855Z", out DateTime d);
            Console.WriteLine(b);
        }

        static bool test_optional(this int x, int y = 0)
        {
            return x % 2 == 0;
        }

        static void get_type(List<object> list)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                Console.WriteLine(list[i] == null ? null : list[i].GetType().Name);
                list[i] = 1;
            }
        }
        static (object val, bool success) change_primitive_type(string value, Type type)
        {
            var false_val = (Activator.CreateInstance(type), false);
            try
            {
                if (type == typeof(bool))
                    if ("1,0,true,false".Contains(value.ToLower()))
                        return (value.ToLower() == "1" || value.ToLower() == "true", true);
                    else
                        return false_val;

                if (type == typeof(DateTime))
                    return (DateTime.Parse(value), true);

                return (Convert.ChangeType(value, type), true);
            }
            catch
            {
                return false_val;
            }
        }
        static (T, bool) change_primitive_type<T>(string value)
        {
            var res = change_primitive_type(value, typeof(T));
            return ((T)res.val, res.success);
            //try
            //{
            //    if (typeof(T) == typeof(bool))
            //        if ("1,0,true,false".Contains(value.ToLower()))
            //            return ((T)(object)(value.ToLower() == "1" || value.ToLower() == "true"), true);
            //        else
            //            return (default(T), false);

            //    if (typeof(T) == typeof(DateTime))
            //        return ((T)(object)DateTime.Parse(value), true);

            //    return ((T)Convert.ChangeType(value, typeof(T)), true);
            //}
            //catch
            //{
            //    return (default(T), false);
            //}
        }
    }
    public class CA
    {
        public string prop1 { get; set; } = "defffault";
        public string prop2 { get; set; }
    }
    static class RES
    {
        static Dictionary<int, string> dicRES;
        static RES()
        {
            Console.WriteLine($"111-{RES.PartnerCodeIsNotExist}");
            dicRES = new Dictionary<int, string>();
            dicRES[PartnerCodeIsNotExist] = "Partner Code Is Not Exist";
            Console.WriteLine("Constructor RES");
        }
        public static readonly int PartnerCodeIsNotExist = 1;
    }
}
