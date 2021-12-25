//
//  Program.cs
//
//  Copyright (c) Wiregrass Code Technology 2018
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
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            Console.WriteLine("simulation: " + ++iteration);

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

                Parallel.For((long)0, parameters.NumberParallelLoops, (i, loopState) =>
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
                Console.WriteLine("number of pool objects: {0}{1}", poolObjectsList.Count, Environment.NewLine);
            }

            foreach (var poolObject in poolObjectsList)
            {
                Console.WriteLine("pool object identifier: {0}", poolObject.Identifier);
                Console.WriteLine("pool object when created: {0}", poolObject.WhenCreated);
                Console.WriteLine("pool object when updated: {0}", poolObject.WhenUpdated);
                Console.WriteLine("pool object member identifier: {0}{1}", poolObject.DisposablePoolMember.Identifier, Environment.NewLine);
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
                Console.WriteLine("{0} v{1}", ((AssemblyDescriptionAttribute)descriptionAttributes[0]).Description, assembly.GetName().Version);
            }
#if _DISPLAY_COPYRIGHT
            var copyrightAttributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if ((copyrightAttributes.Length > 0))
            {
                Console.WriteLine("{0}", ((AssemblyCopyrightAttribute)copyrightAttributes[0]).Copyright);
            }
#endif
            Console.Write(Environment.NewLine);
        }

        public static void DisplayParameters(CommandParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            Console.WriteLine("number of simulations: {0}", parameters.NumberSimulations);
            Console.WriteLine("number of parallel loops: {0}", parameters.NumberParallelLoops);
            Console.WriteLine("wait time between simulations (in milliseconds): {0}", parameters.WaitTimeBetweenSimulations);
            Console.WriteLine("object lifetime: {0}", parameters.ObjectLifetime);
            Console.WriteLine("object pool size: {0}", parameters.ObjectPoolSize);
            Console.WriteLine("object usage limit: {0}{1}", parameters.ObjectUsageLimit, Environment.NewLine);
        }
    }
}