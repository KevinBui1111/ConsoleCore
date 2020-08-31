using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ConsoleCore
{
    class expression
    {
        internal static void test()
        {
            //var exp = exp_str_Contains<CA>("prop1", "12");
            var exp = exp_Equal<CA>("prop1", 123);
            IEnumerable<CA> list = new List<CA>
            {
                new CA{prop1 = "asfa", prop2 = "fa"},
                new CA{prop1 = "123", prop2 = "222"},
                new CA{prop1 = "0a1234w", prop2 = "a12"},
                new CA{prop1 = "5555", prop2 = "a12"},
            };
            var f = exp.Compile();
            var x = list.Where(f);
            foreach(var i in x)
            {
                Console.WriteLine(i.prop1);
            }
            Console.WriteLine("=================");
            var l = new List<int> { 2, 4, 6, 7 };
            Func<List<int>, int, bool> func = (List<int> list, int e) => list.Contains(e);
            var b = func(l, 16);
            Console.WriteLine(b);

            var exp2 = FuncContains<int>();
            var func2 = exp2.Compile();
            Console.WriteLine(func2(l, 7));
            //var res = BuildIt<int>().Compile()(2, 3);
            //Console.WriteLine((a?.prop1 != "abc"));
        }
        private static Expression<Func<T, T, T>> BuildIt<T>()
        {
            var paramExprA = Expression.Parameter(typeof(T), "a");
            var paramExprB = Expression.Parameter(typeof(T), "b");

            var body = Expression.Add(paramExprA, paramExprB);

            var lambda = Expression.Lambda<Func<T,
                T, T>>(body, paramExprA, paramExprB);

            return lambda;
        }

        private static Expression<Func<T, bool>> exp_str_Contains<T>(string propertyName, string propertyValue)
        {
            var param_entity = Expression.Parameter(typeof(T));
            var exp_prop = Expression.Property(param_entity, propertyName);
            var method = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
            var exp_val = Expression.Constant(propertyValue, typeof(string));
            var exp_method_contains = Expression.Call(exp_prop, method, exp_val);

            return Expression.Lambda<Func<T, bool>>(exp_method_contains, param_entity);
        }
        private static Expression<Func<T, bool>> exp_Equal<T>(string propertyName, object propertyValue)
        {
            var param_entity = Expression.Parameter(typeof(T));
            var exp_prop = Expression.Property(param_entity, propertyName);
            var exp_val = Expression.Constant(propertyValue);
            var exp_equal = Expression.Equal(exp_prop, exp_val);

            return Expression.Lambda<Func<T, bool>>(exp_equal, param_entity);
        }
        static Expression<Func<List<T>, T, bool>> FuncContains<T>()
        {
            var param_list = Expression.Parameter(typeof(List<T>));
            MethodInfo method = typeof(List<T>).GetMethod("Contains", new[] { typeof(T) });
            //var someValue = Expression.Constant(propertyValue, typeof(string));
            var param_e = Expression.Parameter(typeof(T));
            var containsMethodExp = Expression.Call(param_list, method, param_e);

            return Expression.Lambda<Func<List<T>, T, bool>>(containsMethodExp, param_list, param_e);
        }
    }
}
