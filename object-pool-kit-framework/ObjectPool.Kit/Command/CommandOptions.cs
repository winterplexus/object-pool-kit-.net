//
//  CommandOptions.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2022
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ObjectPool.Utility;

namespace ObjectPool.Kit
{
    public static class CommandOptions
    {
        private const char numberSimulationFlag = 's';
        private const char numberParallelLoopsFlag = 't';
        private const char waitTimeBetweenSimulationsFlag = 'w';
        private const char objectLifetimeFlag = 'l';
        private const char objectPoolSizeFlag = 'p';
        private const char objectUsageLimitFlag = 'u';

        public static bool Parse(string[] arguments, CommandParameters parameters)
        {
            if (arguments == null || arguments.Length < 1)
            {
                DisplayUsage();
                return false;
            }

            if (parameters == null)
            {
                throw new ArgumentException("parameters argument is null");
            }

            parameters.NumberSimulations = 2;
            parameters.NumberParallelLoops = 10;
            parameters.WaitTimeBetweenSimulations = 10000;
            parameters.ObjectLifetime = 100;
            parameters.ObjectPoolSize = 5;
            parameters.ObjectUsageLimit = 50;

            for (var index = 0; index < arguments.Length; index++)
            {
                if (arguments[index][0] != '-')
                {
                    Console.WriteLine($"error-> option or option value is missing (argument index {index}): {arguments[index]}");
                    return false;
                }
                if (arguments[index].Length <= 1)
                {
                    continue;
                }

                var option = arguments[index][1];

                index++;

                bool invalidValue;

                switch (option)
                {
                    case numberSimulationFlag:
                         invalidValue = ParseNumberSimulations(arguments, index, parameters);
                         break;
                    case numberParallelLoopsFlag:
                         invalidValue = ParseNumberParallelLoops(arguments, index, parameters);
                         break;
                    case waitTimeBetweenSimulationsFlag:
                         invalidValue = ParsenWaitTimeBetweenSimulations(arguments, index, parameters);
                         break;
                    case objectLifetimeFlag:
                         invalidValue = ParseObjectLifetime(arguments, index, parameters);
                         break;
                    case objectPoolSizeFlag:
                         invalidValue = ParseObjectPoolSize(arguments, index, parameters);
                         break;
                    case objectUsageLimitFlag:
                         invalidValue = ParseObjectUsageLimit(arguments, index, parameters);
                         break;
                    default:
                         Console.WriteLine($"error-> unknown option: {option}");
                         return false;
                }

                if (!invalidValue)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ParseNumberSimulations(IReadOnlyList<string> arguments, int i, CommandParameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> number of simulations value is missing");
                return false;
            }
            parameters.NumberSimulations = Assistant.GetIntegerValue(arguments[i]);
            return true;
        }

        private static bool ParseNumberParallelLoops(IReadOnlyList<string> arguments, int i, CommandParameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> number of parallel loops value is missing");
                return false;
            }
            parameters.NumberParallelLoops = Assistant.GetIntegerValue(arguments[i]);
            return true;
        }

        private static bool ParsenWaitTimeBetweenSimulations(IReadOnlyList<string> arguments, int i, CommandParameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> wait time between simulations value is missing");
                return false;
            }
            parameters.WaitTimeBetweenSimulations = Assistant.GetIntegerValue(arguments[i]);
            return true;
        }

        private static bool ParseObjectLifetime(IReadOnlyList<string> arguments, int i, CommandParameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> object lifetime value is missing");
                return false;
            }
            parameters.ObjectLifetime = Assistant.GetIntegerValue(arguments[i]);
            return true;
        }

        private static bool ParseObjectPoolSize(IReadOnlyList<string> arguments, int i, CommandParameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> object pool size value is missing");
                return false;
            }
            parameters.ObjectPoolSize = Assistant.GetIntegerValue(arguments[i]);
            return true;
        }

        private static bool ParseObjectUsageLimit(IReadOnlyList<string> arguments, int i, CommandParameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> object usage limit value is missing");
                return false;
            }
            parameters.ObjectUsageLimit = Assistant.GetIntegerValue(arguments[i]);
            return true;
        }

        private static void DisplayUsage()
        {
            Console.WriteLine($"usage: {Process.GetCurrentProcess().ProcessName}.exe (options){Environment.NewLine}");
            Console.WriteLine($"options: -{numberSimulationFlag} <number of simulations>");
            Console.WriteLine($"\t -{numberParallelLoopsFlag} <number of parallel loops>");
            Console.WriteLine($"\t -{waitTimeBetweenSimulationsFlag} <wait time between simulations>");
            Console.WriteLine($"\t -{objectLifetimeFlag} <object lifetime>");
            Console.WriteLine($"\t -{objectPoolSizeFlag} <object pool size>");
            Console.WriteLine($"\t -{objectUsageLimitFlag} <object usage limit>");
        }
    }
}