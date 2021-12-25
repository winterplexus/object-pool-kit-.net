//
//  IDateTimeFormats.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2021
//
using System;

namespace ObjectPool.Utility
{
    public interface IDateTimeFormats
    {
        string Format(string dateTimeSpecification);
        string Format(string dateTimeSpecification, DateTime dateTime);
    }
}