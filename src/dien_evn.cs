using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ConsoleCore
{
    class dien_evn
    {
        static void MyMain(string[] args)
        {
            //Console.WriteLine(new CT { DIEN_TTHU = 50, DON_GIA = 1235, SO_TIEN = 2345567 });
            //return;
            using (var tw = new StreamWriter("out.txt"))
            using (var handler = new HttpClientHandler { UseCookies = false })
            using (var httpClient = new HttpClient(handler))
            {
                Console.SetOut(tw);
                var list_bill = new List<Electric>();
                var url = "https://cskh.evnhcmc.vn/tracuu/ajax_ds_hoadon";
                var url_tai = "https://cskh.evnhcmc.vn/tracuu/ajax_tai_hoadon";

                // get cookie from browser, after loging in
                httpClient.DefaultRequestHeaders.Add("Cookie", "SS=n2f4dqm55skvvg5e4ms3lnrctg; _ga=GA1.2.1016517780.1583501620; _gid=GA1.2.605371135.1583501620; _gat_gtag_UA_153388238_1=1; login=f8f441314134d3905c50c9e378c635aa57122f8f6c7a017847f3120a7e90418aa7930ccddf573e38d29ca7c643429f783b4e9e6665bf4ba74158929e06b08d7fxrp1BjACg4gGyndSo3bZOOwgiHNoCpXd%2F72urKGXslxlgwvee%2BXPwDUh9u6XWo3iswtmFgZ8qghkWs9VE3UPYVhURGqJuZRnewRAJvsIbKVXhAvM%2BDvSSsrOXDvoWPjPSxdV%2FgEzp3vmwHNVDX4tR18Vp3evT2mxYE6MT0KSaieIRx0%2FWmYkoHWmVZaslrm%2BnWyEmbvLvrKJcWKtQ4kqn7aTJxaJ5EE%2FEIB05QjG6Q%2F4CvXO0xoRmM%2Bbq7AHVeMds1X47L7GCjvmGCar8%2B38je2GY3kQjoj1EMCB3w%2FqV7C6J%2BCydYV6APJ%2BMY%2BUcNUm; TS01128427=018b6f63b4b3f28f75113094ca8938a313316bcb81615bb816489d9fde4d16aeea10758afcada1e97a400b6c15773ffa08425a054c449199caa9b2bce7825d600ec33f4d40");

                int mm = 3, yy = 2016;
                while ($"{yy}{mm:0#}".CompareTo("202003") < 0)
                {
                    HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        {"input_thang", $"{mm}" },
                        {"input_nam", $"{yy}" },
                        {"input_makh", "PE13000104655" },
                    }.AsEnumerable());

                    var response = httpClient.PostAsync(url, content).Result;
                    var html_content = response.Content.ReadAsStringAsync().Result;
                    var m = Regex.Match(html_content, @"xem_thongtin_hddt\(""(\d+)""");
                    var ID_HDON = m.Groups[1].Value;

                    //Konsole.WriteLine($"Get {yy}/{mm:0#} - {ID_HDON}");

                    // =============== load detail ===============
                    HttpContent content_tai = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        {"input_id_hd", $"{ID_HDON}" },
                    }.AsEnumerable());
                    response = httpClient.PostAsync(url_tai, content_tai).Result;
                    html_content = response.Content.ReadAsStringAsync().Result;

                    if (string.IsNullOrEmpty(html_content))
                    {
                        ++mm;
                        if (mm == 13) { mm = 1; ++yy; }
                        continue;
                    }

                    XElement xDoc = XDocument.Parse(html_content).Root;
                    var hd = xDoc.Element("HOADON");
                    var el = new Electric
                    {
                        ID_HDON = ID_HDON,
                        NAM = int.Parse(hd.Attribute("NAM").Value),
                        THANG = int.Parse(hd.Attribute("THANG").Value),
                        DIEN_TTHU = int.Parse(hd.Attribute("DIEN_TTHU").Value),
                        TONG_TIEN = int.Parse(hd.Attribute("TONG_TIEN").Value),
                    };
                    Debug.Assert(el.THANG == mm && el.NAM == yy);

                    var cts = xDoc.Descendants("CHITIET").Select(c => new CT
                    {
                        ID_HDONCTIET = c.Attribute("ID_HDONCTIET").Value,
                        DON_GIA = int.Parse(c.Attribute("DON_GIA").Value),
                        DIEN_TTHU = int.Parse(c.Attribute("DIEN_TTHU").Value),
                        SO_TIEN = int.Parse(c.Attribute("SO_TIEN").Value),
                    }).OrderBy(c => c.ID_HDONCTIET);
                    Debug.Assert(!cts.Any(c => c.DON_GIA * c.DIEN_TTHU != c.SO_TIEN));

                    el.CT.AddRange(cts);

                    list_bill.Add(el);

                    ++mm;
                    if (mm == 13) { mm = 1; ++yy; }

                    //Konsole.indent(2);
                    Konsole.WriteLine($"{el}");
                    Konsole.back();
                }
            }
            Console.ReadKey(true);
        }
    }

    class Electric
    {
        public Electric() => CT = new List<CT>();

        public string ID_HDON { get; set; }
        public int NAM { get; set; }
        public int THANG { get; set; }
        public int DIEN_TTHU { get; set; }
        public int TONG_TIEN { get; set; }

        public List<CT> CT { get; }

        //public override string ToString() => $"{NAM}/{THANG:0#} - {DIEN_TTHU} - {TONG_TIEN:N0}\n  " + 
        //    string.Join("\n  ", CT);
        public override string ToString() => $"{NAM}/{THANG:0#}\t{DIEN_TTHU}\t{TONG_TIEN:N0}\t" +
            string.Join("\t", CT);
    }

    class CT
    {
        public string ID_HDONCTIET { get; set; }
        public int DON_GIA { get; set; }
        public int DIEN_TTHU { get; set; }
        public int SO_TIEN { get; set; }

        //public override string ToString() => $"{DIEN_TTHU}\t{DON_GIA:N0}\t{SO_TIEN:N0}";
        public override string ToString() => $"{DIEN_TTHU}\t{DON_GIA:N0}";
    }
}
