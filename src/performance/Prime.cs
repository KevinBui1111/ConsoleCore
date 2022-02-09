using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCore.performance
{
    internal class Prime
    {
        public static long nth_prime(int n)
        {
            var prime_list = new List<int>() { 2 };
            int number = 1;

            while (prime_list.Count < n)
            {
                number += 2;
                bool success = true; // to check if found prime
                int sqrt = (int)Math.Sqrt(number);

                foreach (int b in prime_list)
                {
                    if (b > sqrt)
                        break;
                    else if (number % b == 0)
                    {
                        success = false;
                        break;
                    }
                }

                if (success) prime_list.Add(number);
            }

            return prime_list[^1];
        }

    }
}
