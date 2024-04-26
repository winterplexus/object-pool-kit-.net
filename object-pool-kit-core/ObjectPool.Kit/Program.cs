//
//  Program.cs
//
//  Copyright (c) Code Construct System 2018-2024
//   
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

[assembly: CLSCompliant(true)]
namespace ObjectPool.Kit
{
    public static class Program
    {
        private static int iteration;

        public static void Main(string[] arguments)
        {
            DisplayVersion();

            var parameters = new CommandParameters();
            if (CommandOptions.Parse(arguments, parameters))
            {
                DisplayParameters(parameters);

                for (var i = 0; i < parameters.NumberSimulations; i++)
                {
                    Simulation(parameters);
                    Thread.Sleep(parameters.WaitTimeBetweenSimulations);
                }
            }
        }

        public static void Simulation(CommandParameters parameters)
        {
            ArgumentNullException.ThrowIfNull(parameters);

            Console.WriteLine($"simulation: {++iteration}");

            ObjectPoolManager.Instance.SetParameters(parameters.ObjectLifetime, parameters.ObjectPoolSize, parameters.ObjectUsageLimit);

            using (var cts = new CancellationTokenSource())
            {
                Task.Run(() =>
                {
                    if (Console.ReadKey().KeyChar == 'c' || Console.ReadKey().KeyChar == 'C')
                    {
                        cts?.Cancel();
                    }
                }, cts.Token);

                Parallel.For((long)0, parameters.NumberParallelLoops, (_, loopState) =>
                {
                    var poolObject = ObjectPoolManager.Instance.PoolObject;
                    ObjectPoolManager.Instance.ReturnPoolObject(poolObject, 1);

                    if (cts.Token.IsCancellationRequested)
                    {
                        loopState.Stop();
                    }
                });

                Console.WriteLine("press the enter key to exit.");
                Console.ReadLine();
            }

            var poolObjectsList = ObjectPoolManager.Instance.PoolObjectsList;
            if (poolObjectsList.Count > 0)
            {
                Console.WriteLine($"number of pool objects: {poolObjectsList.Count}{Environment.NewLine}");
            }

            foreach (var poolObject in poolObjectsList)
            {
                Console.WriteLine($"pool object identifier: {poolObject.Identifier}");
                Console.WriteLine($"pool object when created: {poolObject.WhenCreated}");
                Console.WriteLine($"pool object when updated: {poolObject.WhenUpdated}");
                Console.WriteLine($"pool object member identifier: {poolObject.DisposablePoolMember.Identifier}{Environment.NewLine}");
            }

            ObjectPoolManager.Instance.ReleasePoolObjects();
        }

        public static void DisplayVersion()
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
            {
                Console.WriteLine("error-> unable to get version information from executable");
                return;
            }
            var descriptionAttributes = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if ((descriptionAttributes.Length > 0))
            {
                Console.WriteLine($"{((AssemblyDescriptionAttribute)descriptionAttributes[0]).Description} v{assembly.GetName().Version}");
            }
            var copyrightAttributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if ((copyrightAttributes.Length > 0))
            {
                Console.WriteLine($"{((AssemblyCopyrightAttribute)copyrightAttributes[0]).Copyright}");
            }

            Console.Write(Environment.NewLine);
        }

        public static void DisplayParameters(CommandParameters parameters)
        {
            ArgumentNullException.ThrowIfNull(parameters);

            Console.WriteLine($"number of simulations: {parameters.NumberSimulations}");
            Console.WriteLine($"number of parallel loops: {parameters.NumberParallelLoops}");
            Console.WriteLine($"wait time between simulations (in milliseconds): {parameters.WaitTimeBetweenSimulations}");
            Console.WriteLine($"object lifetime: {parameters.ObjectLifetime}");
            Console.WriteLine($"object pool size: {parameters.ObjectPoolSize}");
            Console.WriteLine($"object usage limit: {parameters.ObjectUsageLimit}{Environment.NewLine}");
        }
    }
}