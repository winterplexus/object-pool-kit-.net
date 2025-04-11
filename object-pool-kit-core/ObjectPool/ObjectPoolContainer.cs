//
//  ObjectPoolContainer.cs
//
//  Copyright (c) Code Construct System 2018-2025
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
            objects = [];
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
                        var objectPoolMember = value as ObjectPoolMember;
                        if (objectPoolMember != null)
                        {
                            if (CheckObjectLifetime(objectPoolMember))
                            {
                                ManagerLog.WritePoolMessage($"object pool member life expired: {objectPoolMember.Identifier} (object life time exceeds life time limit)", LogLevel.Info);
                                continue;
                            }
                        }
                        if (objectPoolMember != null)
                        {
                            ManagerLog.WritePoolMessage($"object taken: {objectPoolMember.Identifier})", LogLevel.Info);
                        }
                        else
                        {
                            ManagerLog.WritePoolMessage("object is null", LogLevel.Error);
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

                    if (value is ObjectPoolMember objectPoolMember)
                    {
                        ManagerLog.WritePoolMessage($"object returned: ({objectPoolMember.Identifier})", LogLevel.Trace);
                    }
                    else
                    {
                        ManagerLog.WritePoolMessage("object is not a pool member", LogLevel.Error);
                    }
                }
                else
                {
                    ManagerLog.WritePoolMessage("object is null", LogLevel.Error);
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

            ManagerLog.WritePoolMessage($"object pool loaded with {objectPoolSize} objects", LogLevel.Info);
        }

        private T GenerateObject()
        {
            var generatedObject = objectGenerator();
            if (generatedObject != null)
            {
                if (generatedObject is ObjectPoolMember ObjectPoolMember)
                {
                    ManagerLog.WritePoolMessage($"object generated: {ObjectPoolMember.Identifier}", LogLevel.Trace);
                }
            }
            return generatedObject;
        }

        private bool CheckObjectLifetime(ObjectPoolMember poolObject)
        {
            var lifetimeSpan = DateTime.Now.Subtract(poolObject.WhenUpdated);

            if (objectLifetime < 1)
            {
                ManagerLog.WritePoolMessage($"object lifetime less than zero: {poolObject.Identifier}", LogLevel.Trace);
                return false;
            }
            if (lifetimeSpan.Seconds > objectLifetime)
            {
                ManagerLog.WritePoolMessage($"object lifetime greater than {objectLifetime}: {poolObject.Identifier}", LogLevel.Trace);
                return true;
            }
            return false;
        }
    }
}