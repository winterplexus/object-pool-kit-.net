//
//  CommandOptionsParameters.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2021
//                        
namespace ObjectPool.Kit
{
    public class CommandOptionsParameters
    {
        public int NumberSimulations { get; set; }
        public int NumberParallelLoops { get; set; }
        public int WaitTimeBetweenSimulations { get; set; }
        public int ObjectLifetime { get; set; }
        public int ObjectPoolSize { get; set; }
        public int ObjectUsageLimit { get; set; }
    }
}