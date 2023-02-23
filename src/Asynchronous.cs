using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Threading.Thread;

namespace ConsoleCore {
    public class Asynchronous {
        public async Task RunUntilCancelAsync(Func<Task> handler, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Console.WriteLine($"Before Executing handler...{CurrentThread.ManagedThreadId}");
                await handler();
                Console.WriteLine($"After Executing handler...{CurrentThread.ManagedThreadId}");
            }
            Console.WriteLine("================ TASK CANCEL ================");
        }
        
        public async Task DriverMethod(Func<Task> handler)
        {
            var cts = new CancellationTokenSource();

            Console.WriteLine($"1. DriverMethod...{CurrentThread.ManagedThreadId}");
            var runTask = RunUntilCancelAsync(handler, cts.Token);
            Console.WriteLine($"2. DriverMethod - after trigger RunUntilCancelAsync...{CurrentThread.ManagedThreadId}");
            await Task.Delay(2000, cts.Token);
            cts.Cancel();
            await runTask;
            
            Console.WriteLine($"3. DriverMethod...{CurrentThread.ManagedThreadId}");
        }

        public void PrintThread() {
            Console.WriteLine($"Thread Id: {CurrentThread.ManagedThreadId}"); // In ra cùng Id ở trên
        }

        public static async Task Test() {
            var asynchronous = new Asynchronous();
            asynchronous.PrintThread();
            
            async Task AsyncHandler() {
                await Task.CompletedTask;
                Console.WriteLine($"-- AsyncHandler...{CurrentThread.ManagedThreadId}");
                Task.Delay(200).Wait();
            }

            await asynchronous.DriverMethod(AsyncHandler);
            
            asynchronous.PrintThread();
        }
    }
}