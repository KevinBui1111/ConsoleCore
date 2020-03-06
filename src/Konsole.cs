using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCore
{
    class Konsole
    {
        static Queue<int> q_indent = new Queue<int>();
        static int current = 0;
        public static void indent(int i)
        {
            q_indent.Enqueue(i);
            current += i;
        }
        public static void back()
        {
            if (q_indent.Count > 0)
                current -= q_indent.Dequeue();
        }
        public static void reset()
        {
            q_indent.Clear();
            current = 0;
        }

        public static void WriteLine(string o)
        {
            o = string.Join(
                '\n',
                o.Split('\n').Select(l => "".PadLeft(current) + l)
                );
            Console.WriteLine(o);
        }
    }
}
