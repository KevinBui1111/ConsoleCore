using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ConsoleApp
{
    class dien_evn
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(new CT { DIEN_TTHU = 50, DON_GIA = 1235, SO_TIEN = 2345567 });
            //return;
            using (var handler = new HttpClientHandler { UseCookies = false })
            using (var httpClient = new HttpClient(handler))
            {
                var list_bill = new List<Electric>();
                var url = "https://cskh.evnhcmc.vn/tracuu/ajax_ds_hoadon";
                var url_tai = "https://cskh.evnhcmc.vn/tracuu/ajax_tai_hoadon";

                httpClient.DefaultRequestHeaders.Add("Cookie", "_ga=GA1.2.1391265988.1583115567; SS=4mpoese22bt007ec9oenkdm7p0; _gid=GA1.2.882096003.1583464210; _gat_gtag_UA_153388238_1=1; login=a7a01d56ec3dd3e6d82e2bba7f57f38ace49da2627f7de70040e156d20607747a9e279ec70b76b0cb490096009772b8f6456122bb9a6679f617f9e9f5a8eeceaFHOzOC3Q%2B8zQAG1YMe83QNOpjWo%2Fa9YxsS8FyNa80mvZJmCqkwdcbk3GxSUEnVQBgj7lz1Wv%2BkeYPkzjrekrNqZZm7qOxYTNpv4q1n39WWASRS%2BIGwVEl4cbyt3A4B38El%2F7CP%2FQbUxJrA93XtibqdeX8OTfqEDx%2BFmNK6ibh0qL7JetIIbPBB86y4Q%2FktpR4K6%2FHKeW5ZTxyVk3a0Lo7Ovu8U684hGwuM6ylkC9k15LvdiS5UgIUAq2%2B5Yew9yifTv3DbtyKnWRMGUd7mmTPIOISPKusIalIPLrfuTJubZbW9aSHaoL0ykFBWsCdTkt; TS01128427=018b6f63b47dfb3e609ad18db1b39254e5ccb98c964494ec7ba08f133b721824a3beb52a3bc997ff73d8db4b96e4d8c7b0da3d3047928868d3503959dabc0509187b8e5642");

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

                    Console.WriteLine($"Get {yy}/{mm:0#} - {ID_HDON}");

                    // =============== load detail ===============
                    HttpContent content_tai = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        {"input_id_hd", $"{ID_HDON}" },
                        //{"input_loai", "" },
                        //{"input_thang", $"{mm}" },
                        //{"input_nam", $"{yy}" },
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
                        DON_GIA = int.Parse(c. Attribute("DON_GIA").Value),
                        DIEN_TTHU = int.Parse(c.Attribute("DIEN_TTHU").Value),
                        SO_TIEN = int.Parse(c.Attribute("SO_TIEN").Value),
                    });
                    Debug.Assert(!cts.Any(c => c.DON_GIA * c.DIEN_TTHU != c.SO_TIEN));

                    el.CT.AddRange(cts);

                    list_bill.Add(el);

                    ++mm;
                    if (mm == 13) { mm = 1; ++yy; }

                    Console.WriteLine(el);
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

        public override string ToString() => $"{NAM}/{THANG:0#} - {DIEN_TTHU} - {TONG_TIEN:N0}\n  " + 
            string.Join("\n  ", CT);
    }

    class CT
    {
        public string ID_HDONCTIET { get; set; }
        public int DON_GIA { get; set; }
        public int DIEN_TTHU { get; set; }
        public int SO_TIEN { get; set; }

        public override string ToString() => $"{DIEN_TTHU}\t{DON_GIA:N0}\t{SO_TIEN:N0}";
    }
}
