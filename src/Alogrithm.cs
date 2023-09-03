using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleCore
{
    class Result
    {

        /*
         * Complete the 'climbingLeaderboard' function below.
         *
         * The function is expected to return an INTEGER_ARRAY.
         * The function accepts following parameters:
         *  1. INTEGER_ARRAY ranked
         *  2. INTEGER_ARRAY player
         */

        public static List<int> climbingLeaderboard(List<int> ranked, List<int> player)
        {
            //100 100 50 40 40 20 10
            //5 25 50 120
            int[] res = new int[player.Count];

            int rank = 0;
            int prev_score = int.MaxValue;
            int i = 0, j = player.Count - 1;
            while (i < ranked.Count && j >= 0)
            {
                if (prev_score > ranked[i])
                {
                    ++rank;
                    prev_score = ranked[i];
                }

                if (player[j] >= ranked[i])
                {
                    res[j--] = rank;
                    continue;
                }
                else
                {
                    ++i;
                }
            }

            ++rank;
            while (j >= 0)
            {
                res[j--] = rank;
            }
            return res.ToList();
        }

        /*
         * Complete the 'stockmax' function below.
         *
         * The function is expected to return a LONG_INTEGER.
         * The function accepts INTEGER_ARRAY prices as parameter.
         */

        public static long stockmax_(List<int> prices)
        {
            if (prices.Count == 0) return 0;
            // find tip
            int iTip = prices.IndexOf(prices.Max());
            long profit = iTip * (long)prices[iTip] - prices.Take(iTip).Sum(p => (long)p);
            profit += stockmax(prices.Skip(iTip + 1).ToList());
            return profit;
        }
        public static long stockmax(List<int> prices)
        {
            // find tip, partition
            int last_tip = 0;
            var tips = new (long p, int i)[prices.Count];
            for (int i = 0; i < prices.Count; ++i)
            {
                while (last_tip >= 0 && tips[last_tip].p <= prices[i])
                {
                    --last_tip;
                }
                tips[++last_tip] = (prices[i], i);
                Console.WriteLine(tips[last_tip]);
            }

            // calc
            long profit = 0;
            int prev_tip = 0;
            for (int i = 0; i <= last_tip; ++i)
            {
                var tip = tips[i];
                profit += (tip.i - prev_tip) * (long)prices[tip.i] - prices.Skip(prev_tip).Take(tip.i - prev_tip).Sum(p => (long)p);
                prev_tip = tip.i + 1;
            }
            return profit;
        }

        /*
         * https://www.geeksforgeeks.org/next-greater-element/
         */
        public static long[] next_greater_element(long[] list)
        {
            long[] res = new long[list.Length];
            var wait_stack = new Stack<int>();
            for(int i = 0; i < list.Length; ++i)
            {
                int i_wait = -1;
                while (wait_stack.Count > 0 && list[i_wait = wait_stack.Peek()] < list[i])
                {
                    wait_stack.Pop();
                    res[i_wait] = list[i];
                }
                res[i] = -1;
                wait_stack.Push(i);
            }
            return res;
        }
        public static long[] next_greater_element2(long[] list)
        {
            long[] res = new long[list.Length];
            for (int i = 0; i < list.Length; ++i)
            {
                for(int j = i - 1; j >= 0; --j)
                {
                    if (res[j] == -1 && list[j] < list[i])
                        res[j] = list[i];
                }
                res[i] = -1;
            }
            return res;
        }
        public static int[] next_greater_element3(int[] list)
        {
            Enumerable.Repeat(-1, list.Length);
            int[] res = new int[list.Length];
            var wait_stack = new Stack<int>();
            for (int i = 0; i < 2 * list.Length; ++i)
            {
                int idx = i % list.Length;
                int i_wait = -1;
                while (wait_stack.Count > 0 && list[i_wait = wait_stack.Peek()] < list[idx])
                {
                    wait_stack.Pop();
                    res[i_wait] = list[idx];
                }

                if (i == idx)
                {
                    res[idx] = -1;
                    wait_stack.Push(idx);
                }
            }
            return res;
        }
        // Complete the riddle function below.
        public static long[] riddle_naive(long[] arr) =>
            Enumerable.Range(1, arr.Length).Select(i =>
                Enumerable.Range(0, arr.Length - i + 1).Max(j =>
                    arr.Skip(j).Take(i).Min()
                    )
            ).ToArray();

        public static long[] riddle_better(long[] arr)
        {
            // complete this function
            long[] res = new long[arr.Length];
            res[0] = arr.Max();
            for (int i = 1; i < arr.Length; ++i)
            {
                long max = -1;
                for (int j = 1; j <= arr.Length - i; ++j)
                {
                    if (arr[j  - 1] > arr[j])
                        arr[j - 1] = arr[j];
                    
                    if (arr[j - 1] > max)
                        max = arr[j - 1];
                }
                res[i] = max;
            }
            return res;
        }
        public static long[] riddle(long[] arr)
        {
            // next smaller;
            int[] next = new int[arr.Length];
            var wait_stack = new Stack<int>();
            for (int i = 0; i < arr.Length; ++i)
            {
                int i_wait;
                while (wait_stack.Count > 0 && arr[i_wait = wait_stack.Peek()] > arr[i])
                {
                    wait_stack.Pop();
                    next[i_wait] = i;
                }
                next[i] = arr.Length;
                wait_stack.Push(i);
            }

            // prev smaller;
            int[] prev = new int[arr.Length];
            wait_stack = new Stack<int>();
            for (int i = 0; i < arr.Length; ++i)
            {
                while (wait_stack.Count > 0 && arr[wait_stack.Peek()] >= arr[i])
                {
                    wait_stack.Pop();
                }

                prev[i] = wait_stack.Count > 0 ? wait_stack.Peek() : -1;
                wait_stack.Push(i);
            }

            long[] res = Enumerable.Repeat(-999L, arr.Length).ToArray();
            for (int i = 0; i < arr.Length; ++i)
            {
                int win_size = next[i] - prev[i] - 2;
                res[win_size] = arr[i] > res[win_size] ? arr[i]: res[win_size];
            }

            for (int i = arr.Length - 2; i > -1; --i)
            {
                res[i] = res[i] > res[i + 1] ? res[i] : res[i + 1];
                //if (res[i] == -999)
                //    res[i] = res[i + 1];
            }
            return res;
        }
    }

    class Solution
    {
        public static void Main2(string[] args)
        {
            using (var sr = new StreamReader("in.txt"))
            {
                Console.SetIn(sr);

                List<int> ranked = Console.ReadLine().TrimEnd().Split(' ').ToList().Select(rankedTemp => Convert.ToInt32(rankedTemp)).ToList();
                List<int> player = Console.ReadLine().TrimEnd().Split(' ').ToList().Select(playerTemp => Convert.ToInt32(playerTemp)).ToList();

                Stopwatch sw = new Stopwatch();
                sw.Start();
                List<int> result = Result.climbingLeaderboard(ranked, player);

                TextWriter textWriter = new StreamWriter("out.txt");

                sw.Stop();
                Console.WriteLine($"complete in {sw.Elapsed}!");

                textWriter.WriteLine(String.Join("\n", result));

                textWriter.Flush();
                textWriter.Close();
            }

            Console.ReadKey();
        }

        // Complete the divisibleSumPairs function below.
        static int divisibleSumPairs(int n, int k, int[] ar)
        {
            int[] remainders = new int[k];
            int sum = 0;
            foreach(int a in ar)
            {
                sum += remainders[(k - a % k) % k];
                ++remainders[a % k];
            }
            return sum;

        }
        static void Main_divisibleSumPairs(string[] args)
        {
            TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"));

            string[] nk = Console.ReadLine().Split(' ');

            int n = Convert.ToInt32(nk[0]);

            int k = Convert.ToInt32(nk[1]);

            int[] ar = Array.ConvertAll(Console.ReadLine().Split(' '), arTemp => Convert.ToInt32(arTemp))
            ;
            int result = divisibleSumPairs(n, k, ar);

            textWriter.WriteLine(result);

            textWriter.Flush();
            textWriter.Close();
        }
        public static void Main_stockmax(string[] args)
        {
            TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

            int t = Convert.ToInt32(Console.ReadLine().Trim());

            for (int tItr = 0; tItr < t; tItr++)
            {
                int n = Convert.ToInt32(Console.ReadLine().Trim());

                List<int> prices = Console.ReadLine().TrimEnd().Split(' ').ToList().Select(pricesTemp => Convert.ToInt32(pricesTemp)).ToList();

                long result = Result.stockmax(prices);

                textWriter.WriteLine(result);
            }

            textWriter.Flush();
            textWriter.Close();
        }
        public static void Main_nge(string[] args)
        {
            TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"));

            int t = Convert.ToInt32(Console.ReadLine().Trim());

            for (int tItr = 0; tItr < t; tItr++)
            {
                Console.ReadLine();

                var prices = Console.ReadLine().TrimEnd().Split(' ').Select(pricesTemp => Convert.ToInt32(pricesTemp)).ToArray();
                var result = Result.next_greater_element3(prices);

                textWriter.WriteLine(String.Join(" ", result));
            }

            textWriter.Flush();
            textWriter.Close();
        }
        static void Main_minMaxRiddle(string[] args)
        {
            TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"));

            int n = Convert.ToInt32(Console.ReadLine());

            long[] arr = Array.ConvertAll(Console.ReadLine().Split(" ,\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries), arrTemp => Convert.ToInt64(arrTemp));
            long[] res = Result.riddle(arr);

            textWriter.WriteLine(string.Join(" ", res));

            textWriter.Flush();
            textWriter.Close();
        }
        public static void Main3()
        {
            Environment.SetEnvironmentVariable("OUTPUT_PATH", "out.txt");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (var sr = new StreamReader("in.txt"))
            {
                Console.SetIn(sr);
                Main_minMaxRiddle(null);
            }
            sw.Stop();
            Console.WriteLine($"complete in {sw.Elapsed}!");

            (int min, int max) minmax = (int.MinValue, int.MaxValue);
            Console.WriteLine(minmax);

            Console.ReadKey();
        }

    }
}
