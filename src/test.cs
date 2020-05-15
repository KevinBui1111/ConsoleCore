using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ConsoleCore
{
    class testTupple
    {
        public int X { get; set; }
        public string Y { get; set; }

        static void Main_(string[] args)
        {
            // quick declaration, tuple style.
            var (iii, sss) = (123, "abc");
            // write as tuple, and write normal
            Console.WriteLine($"{(iii, sss)} | {iii} - {sss}");

            Console.WriteLine("-----");

            Console.WriteLine(tripleValue((iii, sss)));
            Console.WriteLine($"{(iii, sss)}");

            Console.WriteLine("-----");

            var tup = (iii * 2, sss + sss);
            Console.WriteLine(tripleValue(tup));
            Console.WriteLine(tup);

            Console.WriteLine("---- new value when assign structure ---");

            var tup22 = tup;
            tup22.Item1 *= 2;
            Console.WriteLine(tup);
            Console.WriteLine(tup22);

            (_, sss) = new testTupple { X = 10, Y = "abc" };

            Console.ReadKey();
        }

        static (int, string) tripleValue((int iVal, string sVal) tuple)
        {
            tuple.iVal++;
            tuple.sVal += tuple.sVal;

            return (tuple.iVal * 3, tuple.sVal + tuple.sVal + tuple.sVal);
        }

        public void Deconstruct(out int firstName, out string lastName) => (firstName, lastName) = (X, Y);
    }

    class test
    {
        static void Main(string[] args)
        {
            var y = "The,quick;brown;fox jumps over the lazy dog".Split();
            string[] a = new string[] { "123,456", "678,9090" };
            var x = a.SelectMany((e, index) => e.Split(',').Select(val => (index, val)));
            foreach (var e in x)
            {
                Console.WriteLine($"{e.index} - {e.val}");
            }
        }
    }
}
