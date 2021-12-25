//
//  ObjectPoolContainer.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2022
//  
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NLog;
using ObjectPool.Log;

namespace ObjectPool
{
    public class ObjectPoolContainer<T>
    {
        private readonly ConcurrentBag<T> objects;
        private readonly Func<T> objectGenerator;
        private readonly int objectLifetime;
        private readonly int objectPoolSize;

        public ObjectPoolContainer(Func<T> generator, int lifetime, int poolSize)
        {
            objects = new ConcurrentBag<T>();
            objectGenerator = generator ?? throw new ArgumentNullException(nameof(generator));
            objectLifetime = lifetime;
            objectPoolSize = poolSize;

            ManagerLog.WritePoolMessage("object pool container created", LogLevel.Info);

            LoadObjects();
        }

        public T PoolObject
        {
            get
            {
                while (true)
                {
                    if (objects.TryTake(out T value))
                    {
                        var ObjectPoolMember = value as ObjectPoolMember;
                        if (ObjectPoolMember != null)
                        {
                            if (CheckObjectLifetime(ObjectPoolMember))
                            {
                                ManagerLog.WritePoolMessage($"object ({ObjectPoolMember.Identifier}) abandoned (lifetime > lifetime limit)", LogLevel.Info);
                                continue;
                            }
                        }
                        if (ObjectPoolMember != null)
                        {
                            ManagerLog.WritePoolMessage($"object ({ObjectPoolMember.Identifier}) taken", LogLevel.Trace);
                        }
                        else
                        {
                            ManagerLog.WritePoolMessage("object (null) taken", LogLevel.Error);
                        }
                    }
                    else
                    {
                        value = GenerateObject();
                    }
                    return value;
                }
            }
            set
            {
                if (value != null)
                {
                    objects.Add(value);

                    if (value is ObjectPoolMember ObjectPoolMember)
                    {
                        ManagerLog.WritePoolMessage($"object ({ObjectPoolMember.Identifier}) returned", LogLevel.Trace);
                    }
                    else
                    {
                        ManagerLog.WritePoolMessage("object (null) returned", LogLevel.Error);
                    }
                }
                else
                {
                    ManagerLog.WritePoolMessage("object returned is null", LogLevel.Error);
                }
            }
        }

        public IReadOnlyCollection<ObjectPoolMember> ObjectsList => objects.OfType<ObjectPoolMember>().ToList();

        private void LoadObjects()
        {
            for (var i = 0; i < objectPoolSize; i++)
            {
                objects.Add(objectGenerator());
            }

            ManagerLog.WritePoolMessage($"object pool: {objectPoolSize} objects added", LogLevel.Info);
        }

        private T GenerateObject()
        {
            var generatedObject = objectGenerator();
            if (generatedObject != null)
            {
                if (generatedObject is ObjectPoolMember ObjectPoolMember)
                {
                    ManagerLog.WritePoolMessage($"object ({ObjectPoolMember.Identifier}) generated", LogLevel.Trace);
                    ManagerLog.WritePoolMessage($"object ({ObjectPoolMember.Identifier}) taken", LogLevel.Trace);
                }
            }
            return generatedObject;
        }

        private bool CheckObjectLifetime(ObjectPoolMember poolObject)
        {
            var lifetimeSpan = DateTime.Now.Subtract(poolObject.WhenUpdated);

            if (objectLifetime < 1)
            {
                return false;
            }
            return lifetimeSpan.Seconds > objectLifetime;
        }
    }
}