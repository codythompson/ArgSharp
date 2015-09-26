using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpParse
{
    public class VirtualArray<T> : IEnumerable
    {
        public int length
        {
            get { return end - start; }
        }

        private T[] baseArray;
        private int start, end;

        public VirtualArray(T[] baseArray, int startIndexInclusive, int endIndexExclusive)
        {
            this.baseArray = baseArray;
            start = startIndexInclusive;
            end = endIndexExclusive;
        }

        public VirtualArray(T[] baseArray) : this(baseArray, 0, baseArray.Length) { }

        public void resize(int startIndexInclusive, int endIndexExclusive)
        {
            if (startIndexInclusive < 0 || endIndexExclusive > baseArray.Length)
            {
                throw new IndexOutOfRangeException(); // TODO use a custom exception here
            }

            start = startIndexInclusive;
            end = endIndexExclusive;
        }

        public void moveStart(int startIndexInclusive)
        {
            resize(startIndexInclusive, end);
        }

        public void moveEnd(int endIndexExclusive)
        {
            resize(start, endIndexExclusive);
        }


        public T get(int index)
        {
            if (index < 0 || index >= length)
            {
                throw new IndexOutOfRangeException(); // TODO use a custom exception here
            }
            int absIx = getAbsoluteIndex(index);
            return baseArray[absIx];
        }

        public void set(T element, int index)
        {
            if (index < 0 || index >= length)
            {
                throw new IndexOutOfRangeException(); // TODO use a custom exception here
            }
            int absIx = getAbsoluteIndex(index);
            baseArray[absIx] = element;
        }

        public T this[int index]
        {
            get { return get(index); }
            set { set(value, index); }
        }

        public T[] toArray()
        {
            T[] newArray = new T[length];
            for (int i = 0; i < newArray.Length; i++)
            {
                newArray[i] = get(i);
            }
            return newArray;
        }

        public List<T> toList()
        {
            List<T> newList = new List<T>(toArray());
            return newList;
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < length; i++)
            {
                yield return get(i);
            }
        }

        /*
         * helpers
         */
        private int getAbsoluteIndex(int virtualIndex)
        {
            return start + virtualIndex;
        }
    }
}
