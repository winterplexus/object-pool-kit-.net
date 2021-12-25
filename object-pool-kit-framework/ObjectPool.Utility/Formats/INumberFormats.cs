//
//  INumberFormats.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2021
//
namespace ObjectPool.Utility
{
    public interface INumberFormats
    {
        string GetOrdinal(int number);
        string GetLiteralAmount(double amount);
    }
}