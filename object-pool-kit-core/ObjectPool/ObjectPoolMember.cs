﻿//
//  ObjectPoolMember.cs
//
//  Copyright (c) Code Construct System 2018-2024
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

            // object pool member payload
            DisposablePoolMember = new DisposablePoolMember();

            ManagerLog.WritePoolMessage($"object pool member created: {Identifier}", LogLevel.Info);
        }

        ~ObjectPoolMember()
        {
            DisposablePoolMember?.Dispose();

            ManagerLog.WritePoolMessage($"object pool member destroyed: {Identifier}", LogLevel.Info);
        }

        public Guid Identifier { get; }

        public int UsageCount { get; set; }

        public int RecordsCount { get; set; }

        public DateTime WhenCreated { get; }

        public DateTime WhenUpdated { get; set; }

        public DisposablePoolMember DisposablePoolMember { get; }
    }
}