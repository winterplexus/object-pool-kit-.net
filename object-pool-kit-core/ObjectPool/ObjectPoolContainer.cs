//
//  ObjectPoolContainer.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2020
//  
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
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
                                ManagerLog.WritePoolMessage(string.Format(CultureInfo.InvariantCulture, "object ({0}) abandoned (lifetime > lifetime limit)", ObjectPoolMember.Identifier), LogLevel.Info);
                                continue;
                            }
                        }
                        if (ObjectPoolMember != null)
                        {
                            ManagerLog.WritePoolMessage(string.Format(CultureInfo.InvariantCulture, "object ({0}) taken", ObjectPoolMember.Identifier), LogLevel.Trace);
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

                    var ObjectPoolMember = value as ObjectPoolMember;
                    if (ObjectPoolMember != null)
                    {
                        ManagerLog.WritePoolMessage(string.Format(CultureInfo.InvariantCulture, "object ({0}) returned", ObjectPoolMember.Identifier), LogLevel.Trace);
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

        public List<ObjectPoolMember> ObjectsList => objects.OfType<ObjectPoolMember>().ToList();

        private void LoadObjects()
        {
            for (var i = 0; i < objectPoolSize; i++)
            {
                objects.Add(objectGenerator());
            }

            ManagerLog.WritePoolMessage(string.Format(CultureInfo.InvariantCulture, "object pool: {0} objects added", objectPoolSize), LogLevel.Info);
        }

        private T GenerateObject()
        {
            var generatedObject = objectGenerator();
            if (generatedObject != null)
            {
                var ObjectPoolMember = generatedObject as ObjectPoolMember;
                if (ObjectPoolMember != null)
                {
                    ManagerLog.WritePoolMessage(string.Format(CultureInfo.InvariantCulture, "object ({0}) generated", ObjectPoolMember.Identifier), LogLevel.Trace);
                    ManagerLog.WritePoolMessage(string.Format(CultureInfo.InvariantCulture, "object ({0}) taken", ObjectPoolMember.Identifier), LogLevel.Trace);
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