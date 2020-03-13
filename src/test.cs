using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCore
{
    class test
    {
        public int X { get; set; }
        public string Y { get; set; }

        static void Main(string[] args)
        {
            int x = 1;
            string a = "3463";
            var t = (x, a);
            (_, a) = new test { X = 10, Y = "abc" };
            var (m, n) = (1, "abc");
            Console.WriteLine((x, a));
            Console.ReadKey();
        }

        static (int, string) tuplefunc()
        {
            return (33, "etwetwet");
        }

        public void Deconstruct(out int firstName, out string lastName) => (firstName, lastName) = (X, Y);
    }
}
