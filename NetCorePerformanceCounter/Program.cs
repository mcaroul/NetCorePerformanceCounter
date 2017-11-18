using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetCorePerformanceCounter
{
    class Program
    {
        private const string PerfCategoryName = "NetCorePerformanceCounter";
        private const string PerfCounterName = "MyCustomPerfCounter";

        static void Main(string[] args)
        {
            Console.ReadKey();
            Console.WriteLine("Running");

            CreatePerfCategoryIfNotExists();
            SimulatePerfCounterActivity();
            DeletePerfCategoryIfNotExists();

            Console.WriteLine("End!");
            Console.ReadKey();
        }

        private static void SimulatePerfCounterActivity()
        {
            var perfCounter = new PerformanceCounter(PerfCategoryName, PerfCounterName, false);

            while (!Console.KeyAvailable)
            {
                long value = perfCounter.IncrementBy(new Random().Next(3));
                Console.WriteLine(value);
                Task.Delay(500).Wait();
            }
        }

        private static void CreatePerfCategoryIfNotExists()
        {
            if (PerformanceCounterCategory.Exists(PerfCategoryName))
                return;

            var counterData = new CounterCreationDataCollection(new[] {
                new CounterCreationData
                {
                    CounterName = PerfCounterName,
                    CounterHelp = PerfCounterName,
                    CounterType = PerformanceCounterType.NumberOfItems64
                }
            });

            PerformanceCounterCategory.Create(PerfCategoryName, PerfCategoryName, PerformanceCounterCategoryType.Unknown, counterData);
        }

        private static void DeletePerfCategoryIfNotExists()
        {
            if (!PerformanceCounterCategory.Exists(PerfCategoryName))
                return;

            PerformanceCounterCategory.Delete(PerfCategoryName);
        }
    }
}