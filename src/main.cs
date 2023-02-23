using ConsoleCore.magic_treasure;
using ConsoleCore.performance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Threading.Thread;

namespace ConsoleCore
{
    class main
    {
        static async Task Main(string[] args)
        {
            //Ago.RandomPercent();
            // errand.test();
            //decor2.test();
            //errand.ReplaceLineEnding();
            //DI.setup_di();
            //magic_operate.test();
            // DynamicTest.Test();
            await Asynchronous.Test();
            Console.WriteLine("======== OK =========");
            Console.ReadKey(false);
        }
    }
}
