using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace ConsoleCore.magic_treasure
{
    public class Card
    {
        public int number { get; set; } //one, two, or three
        public int color { get; set; } //red, yellow, or blue
        public int shape { get; set; } //triangle, square, or circle
        public int fill { get; set; } // open, solid, or cross
        //2123 1232 3212
        public Card(int number, int color, int shape, int fill)
        {
            this.number = number;
            this.color = color;
            this.shape = shape;
            this.fill = fill;
        }
        public override string ToString() => $"{number} - {color} - {shape} - {fill}";
    }

    public class magic_operate
    {
        static IList<Card> list_card = new List<Card>();
        static public void generate_card()
        {
            for(int n = 1; n <=3; ++n)
            {
                for (int c = 1; c <= 3; ++c)
                {
                    for (int s = 1; s <= 3; ++s)
                    {
                        for (int f = 1; f <= 3; ++f)
                        {
                            list_card.Add(new Card(n, c, s, f));
                        }
                    }
                }
            }

            Random rnd = new Random((int)DateTime.Now.Ticks);
            list_card = list_card.OrderBy(x => rnd.Next()).ToList();
        }

        static public void test()
        {
            generate_card();
            var set8 = list_card.Take(8);

            Console.WriteLine("---- set 8 -----");
            foreach (var c in set8)
                Console.WriteLine($"{c}");

            Console.WriteLine("---- find match -----");
            find_match(set8);
        }
        static public IList<(Card, Card, Card)> find_match(IEnumerable<Card> set_card)
        {
            var list_set = errand.Combinations(set_card, 3).ToList();
            foreach(var set in list_set)
            {
                var res = check_match(set);
                if (res.match)
                {
                    Console.WriteLine(res.case_match);
                    Console.WriteLine(
                        string.Join('\n', set)
                    );
                    Console.WriteLine();
                }
            }
            return null;
        }
        static public (bool match, int case_match) check_match(IEnumerable<Card> set_card)
        {
            // case:
            // 1: 4 diff
            // 2: 3 diff
            // 3: 2 diff
            // 4: 1 diff
            int c_n = set_card.Select(c => c.number).Distinct().Count();
            int c_c = set_card.Select(c => c.color).Distinct().Count();
            int c_s = set_card.Select(c => c.shape).Distinct().Count();
            int c_f = set_card.Select(c => c.fill).Distinct().Count();
            //0 1 2 3 4
            //3,3,3,3
            //1,3,3,3  3,1,3,3  3,3,3,1
            //1,1,3,3  3,1,3,1  1,3,3,1
            //3,1,1,1  1,3,1,1  1,1,3,1
            int[] res_prop = new int[] { c_n, c_c, c_s, c_f };
            int diff = res_prop.Where(r => r == 3).Count();
            int same = res_prop.Where(r => r == 1).Count();

            if (diff == 4)
                return (true, same);
            if (diff == 3 && same == 1)
                return (true, same);
            if (diff == 2 && same == 2)
                return (true, same);
            if (diff == 1 && same == 3)
                return (true, same);

            return (false, 0);
        }
    }
}
