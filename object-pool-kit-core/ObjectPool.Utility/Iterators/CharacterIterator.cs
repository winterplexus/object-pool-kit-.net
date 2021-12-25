//
//  CharacterIterator.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2021
//
using System;

namespace ObjectPool.Utility
{
    public class CharacterIterator : ICharacterIterator
    {
        private const char eol = '\uFFFF';
        private readonly string text;
        private readonly int beginIndex;
        private readonly int endIndex;
        private int index;

        public CharacterIterator(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            text = value;
            index = 0;
            beginIndex = 0;
            endIndex = text.Length;
        }

        public char First()
        {
            index = beginIndex;
            return Current();
        }

        public char Last()
        {
            if (endIndex != beginIndex)
            {
                index = endIndex - 1;
            }
            else
            {
                index = endIndex;
            }

            return Current();
        }

        public char Current()
        {
            if (index >= beginIndex && (index < endIndex))
            {
                return text[index];
            }

            return eol;
        }

        public char Next()
        {
            if (index < endIndex - 1)
            {
                index++;
                return text[index];
            }

            index = endIndex;
            return eol;
        }

        public char Previous()
        {
            if (index > beginIndex)
            {
                index--;
                return text[index];
            }

            return eol;
        }

        public char AtEnd()
        {
            return eol;
        }

        public int GetBeginIndex()
        {
            return beginIndex;
        }

        public int GetEndIndex()
        {
            return endIndex;
        }

        public int GetIndex()
        {
            return index;
        }
    }
}