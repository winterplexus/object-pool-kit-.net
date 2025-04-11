//
//  DisposablePoolMember.cs
//
//  Copyright (c) Code Construct System 2018-2025
//  
using NLog;
using ObjectPool.Log;
using System;

namespace ObjectPool
{
    public class DisposablePoolMember : IDisposable
    {
        private bool disposedValue;

        public DisposablePoolMember()
        {
            Identifier = Guid.NewGuid();
            disposedValue = false;

            ManagerLog.WritePoolMessage($"disposable pool member created: {Identifier}", LogLevel.Trace);
        }

        public Guid Identifier { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ManagerLog.WritePoolMessage($"disposable pool member disposed: {Identifier}", LogLevel.Trace);
                }
                disposedValue = true;
            }
        }
    }
}