using ConsoleCore.magic_treasure;
using ConsoleCore.performance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ConsoleCore
{
    class main
    {
        static void Main(string[] args)
        {
            var timer = new Stopwatch();
            timer.Start();
            long prime = Prime.nth_prime((int)5e6);
            timer.Stop();
            Console.WriteLine($"Prime: {prime:n0}, elapsed: {timer.Elapsed}");

            //Ago.RandomPercent();
            //errand.test();
            //decor2.test();
            //errand.ReplaceLineEnding();
            //DI.setup_di();
            //magic_operate.test();
            Console.WriteLine("======== OK =========");
            Console.ReadKey(false);
        }
    }
}
