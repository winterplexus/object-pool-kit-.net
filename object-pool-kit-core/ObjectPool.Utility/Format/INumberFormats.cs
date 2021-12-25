//
//  INumberFormats.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2021
//
namespace ObjectPool.Utility
{
    public interface INumberFormats
    {
        public string GetOrdinal(int number);
        public string GetLiteralAmount(double amount);
    }
}