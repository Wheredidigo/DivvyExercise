using System;
using System.Diagnostics;
using Amazon.Lambda.SQSEvents;
using DivvyExercise.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace DivvyExercise.ConsoleApp
{
    public class Program
    {
        static Program()
        {
            ServiceProvider = Startup.GetServiceProvider(true);
        }

        private static ServiceProvider ServiceProvider { get; }

        public static void Main(string[] args)
        {
            var timer = new Stopwatch();
            timer.Start();

            ServiceProvider.GetService<App>().Run(GetTestEvent()).GetAwaiter().GetResult();

            timer.Stop();
            Console.WriteLine(timer.Elapsed.TotalMilliseconds);
        }

        private static SQSEvent GetTestEvent()
        {
            throw new NotImplementedException();
        }

        ~Program()
        {
            ServiceProvider.Dispose();
        }
    }
}