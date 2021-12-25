//
//  ObjectPoolMember.cs
//
//  Copyright (c) Wiregrass Code Technology 2018
//       
using System;
using System.Globalization;
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
            DisposablePoolMember = new DisposablePoolMember();

            ManagerLog.WritePoolMessage(string.Format(CultureInfo.InvariantCulture, "object pool member ({0}) created", Identifier), LogLevel.Info);
        }

        ~ObjectPoolMember()
        {
            DisposablePoolMember?.Dispose();

            ManagerLog.WritePoolMessage(string.Format(CultureInfo.InvariantCulture, "object pool member ({0}) destroyed", Identifier), LogLevel.Info);
        }

        public Guid Identifier { get; }

        public int UsageCount { get; set; }

        public int RecordsCount { get; set; }

        public DateTime WhenCreated { get; }

        public DateTime WhenUpdated { get; set; }

        public DisposablePoolMember DisposablePoolMember { get; }
    }
}