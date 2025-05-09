﻿//
//  CommandParameters.cs
//
//  Copyright (c) Code Construct System 2018-2025
//                        
namespace ObjectPool.Kit
{
    internal sealed class CommandParameters
    {
        public int NumberSimulations { get; set; }
        public int NumberParallelLoops { get; set; }
        public int WaitTimeBetweenSimulations { get; set; }
        public int ObjectLifetime { get; set; }
        public int ObjectPoolSize { get; set; }
        public int ObjectUsageLimit { get; set; }
    }
}