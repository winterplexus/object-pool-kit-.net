//
//  ObjectPoolManager.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2021
//  
using System;
using System.Collections.Generic;
using NLog;
using ObjectPool.Log;

namespace ObjectPool
{
    public sealed class ObjectPoolManager
    {
        private static volatile ObjectPoolManager objectPoolContainerManager;
        private static readonly object objectPoolContainerManagerLock = new();
        private readonly ObjectPoolContainer<ObjectPoolMember> objectPoolContainer;
        private int objectLifetime;
        private int objectPoolSize;
        private int objectUsageLimit;

        private ObjectPoolManager()
        {
            objectPoolContainer = new ObjectPoolContainer<ObjectPoolMember>(() => new ObjectPoolMember(), objectLifetime, objectPoolSize);
        }

        public static ObjectPoolManager Instance
        {
            get
            {
                if (objectPoolContainerManager == null)
                {
                    lock (objectPoolContainerManagerLock)
                    {
                        if (objectPoolContainerManager == null)
                        {
                            objectPoolContainerManager = new ObjectPoolManager();

                            ManagerLog.WriteManagerMessage("object pool container manager created", LogLevel.Info);
                        }
                    }
                }
                return objectPoolContainerManager;
            }
        }

        public ObjectPoolMember PoolObject => objectPoolContainer.PoolObject;

        public void SetParameters(int lifetime, int poolSize, int usageLimit)
        {
            objectLifetime = lifetime;
            objectPoolSize = poolSize;
            objectUsageLimit = usageLimit;
        }

        public void ReturnPoolObject(ObjectPoolMember poolObject, int recordCount)
        {
            if (poolObject != null)
            {
                if (poolObject.UsageCount > objectUsageLimit)
                {
                    ManagerLog.WriteManagerMessage($"pool object ({poolObject.Identifier}) abandoned (usage > usage limit)", LogLevel.Info);
                    return;
                }

                poolObject.UsageCount++;
                poolObject.RecordsCount += recordCount;
                poolObject.WhenUpdated = DateTime.Now;

                objectPoolContainer.PoolObject = poolObject;
            }
            else
            {
                ManagerLog.WriteManagerMessage("pool object is null", LogLevel.Error);
            }
        }

        public List<ObjectPoolMember> PoolObjectsList => objectPoolContainer.ObjectsList;

        public void ReleasePoolObjects()
        {
            var poolObjectsList = objectPoolContainer.ObjectsList;

            foreach (var poolObject in poolObjectsList)
            {
                poolObject.DisposablePoolMember.Dispose();
            }

            ManagerLog.WriteManagerMessage("pool objects released", LogLevel.Info);
        }
    }
}