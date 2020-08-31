using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleCore
{
    static class DI
    {
        public static void setup_di()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(l => l.AddConsole())
                .AddSingleton<IFooService, FooService>()
                .AddSingleton<IBarService, BarService>()
                .AddSingleton<IBoxRepo, BoxRepo>()
                .BuildServiceProvider();
                ;
            //configure console logging

            var bar = serviceProvider.GetService<IBoxRepo>();
            bar.find_box();
            bar.save();

            var bar2 = serviceProvider.GetService<IBoxRepo>();
            Console.WriteLine(bar == bar2);
            var abs = (absBaseRepo)bar;
            abs.save();
        }
    }


    public interface IFooService
    {
        void DoThing(string number);
    }
    public interface IBarService
    {
        void DoSomeRealWork();
    }


    public class BarService : IBarService
    {
        private readonly IFooService _fooService;
        public BarService(IFooService fooService) => _fooService = fooService;

        public void DoSomeRealWork() => _fooService.DoThing("DoSomeRealWork");
    }
    public class FooService : IFooService
    {
        private readonly ILogger<FooService> _logger;

        public FooService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FooService>();
        }

        public void DoThing(string number)
        {
            Console.WriteLine("DoThing");
            _logger.LogInformation($"Doing the thing {number}");
            Console.WriteLine("DoThing 2");
        }
    }


    interface IBaseRepo
    {
        void save();
    }

    interface IBoxRepo : IBaseRepo
    {
        void find_box();
    }
    interface IRecRepo : IBaseRepo
    {
        void find_rec();
    }

    abstract class absBaseRepo : IBaseRepo
    {
        private readonly IFooService _fooService;
        public absBaseRepo(IFooService fooService) => _fooService = fooService;

        public virtual void save() => _fooService.DoThing("123");
    }

    class BoxRepo : absBaseRepo, IBoxRepo
    {
        public BoxRepo(IFooService _foo) : base(_foo) { }

        //public void save() => Console.WriteLine("absBaseRepo SAVE");
        public override void save() => Console.WriteLine("BoxRepo SAVE");
        public void find_box() => Console.WriteLine("find_box");
    }
}
