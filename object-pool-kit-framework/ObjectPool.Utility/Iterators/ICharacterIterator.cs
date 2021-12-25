//
//  ICharacterIterator.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2021
//
namespace ObjectPool.Utility
{
    public interface ICharacterIterator
    {
        char First();
        char Last();
        char Current();
        char Next();
        char Previous();
        char AtEnd();
        int GetBeginIndex();
        int GetEndIndex();
        int GetIndex();
    }
}
