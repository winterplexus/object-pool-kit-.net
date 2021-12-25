//
//  DisposablePoolMember.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2020
//  
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
        }

        public Guid Identifier { get; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose resources (if applicable)
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}