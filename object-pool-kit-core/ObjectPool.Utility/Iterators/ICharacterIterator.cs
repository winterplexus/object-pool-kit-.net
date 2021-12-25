//
//  ICharacterIterator.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2021
//
namespace ObjectPool.Utility
{
    public interface ICharacterIterator
    {
        public char First();
        public char Last();
        public char Current();
        public char Next();
        public char Previous();
        public char AtEnd();
        public int GetBeginIndex();
        public int GetEndIndex();
        public int GetIndex();
    }
}
