//
//  StringCharacterIterator.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2020
//
using System;

namespace ObjectPool.Utility
{
    public class StringCharacterIterator
    {
        public const char Done = '\uFFFF';

        private string text;

        public StringCharacterIterator(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }
            SetParameters(text, 0, text.Length, 0);
        }

        public StringCharacterIterator(string text, int position)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }
            SetParameters(text, 0, text.Length, position);
        }

        public StringCharacterIterator(string text, int begin, int end, int position)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (begin < 0 || (begin > end) || (end > text.Length))
            {
                throw new ArgumentNullException(nameof(begin));
            }
            if (position < begin || (position > end))
            {
                throw new ArgumentNullException(nameof(position));
            }
            SetParameters(text, begin, end, position);
        }

        public void SetText(string textParameter)
        {
            if (string.IsNullOrEmpty(textParameter))
            {
                throw new ArgumentNullException(nameof(textParameter));
            }
            text = textParameter;
            BeginIndex = 0;
            EndIndex = text.Length;
            Index = 0;
        }

        public char First()
        {
            Index = BeginIndex;
            return Current();
        }

        public char Last()
        {
            if (EndIndex != BeginIndex)
            {
                Index = EndIndex - 1;
            }
            else
            {
                Index = EndIndex;
            }
            return Current();
        }

        public char SetIndex(int positionParameter)
        {
            if (Index < BeginIndex || (Index > EndIndex))
            {
                throw new ArgumentNullException(nameof(positionParameter));
            }
            Index = positionParameter;
            return Current();
        }

        public char Current()
        {
            if (Index >= BeginIndex && (Index < EndIndex))
            {
                return text[Index];
            }
            return Done;
        }

        public char Next()
        {
            if (Index < EndIndex - 1)
            {
                Index++;
                return text[Index];
            }
            Index = EndIndex;
            return Done;
        }

        public char Previous()
        {
            if (Index > BeginIndex)
            {
                Index--;
                return text[Index];
            }
            return Done;
        }

        public int BeginIndex { get; private set; }

        public int EndIndex { get; private set; }

        public int Index { get; private set; }

        private void SetParameters(string textParameter, int beginParameter, int endParameter, int positionParameter)
        {
            text = textParameter;
            BeginIndex = beginParameter;
            EndIndex = endParameter;
            Index = positionParameter;
        }
    }
}