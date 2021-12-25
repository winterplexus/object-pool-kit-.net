//
//  ObjectPoolMember.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2021
//       
using System;
using NLog;
using ObjectPool.Log;

namespace ObjectPool
{
    public class ObjectPoolMember
    {
        public ObjectPoolMember()
        {
            Identifier = Guid.NewGuid();
            WhenCreated = DateTime.Now;
            WhenUpdated = WhenCreated;

            // Pool object
            DisposablePoolMember = new DisposablePoolMember();

            ManagerLog.WritePoolMessage($"object pool member ({Identifier}) created", LogLevel.Info);
        }

        ~ObjectPoolMember()
        {
            DisposablePoolMember?.Dispose();

            ManagerLog.WritePoolMessage($"object pool member ({Identifier}) destroyed", LogLevel.Info);
        }

        public Guid Identifier { get; }

        public int UsageCount { get; set; }

        public int RecordsCount { get; set; }

        public DateTime WhenCreated { get; }

        public DateTime WhenUpdated { get; set; }

        public DisposablePoolMember DisposablePoolMember { get; }
    }
}