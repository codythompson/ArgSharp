using System;
using System.Collections;
using System.Collections.Generic;

namespace ArgSharp
{
    public class VirtualArray<T> : IEnumerable
    {
        public int length
        {
            get { return end - start; }
        }
        public int startIndexInclusive
        {
            get { return start; }
        }
        public int endIndexExclusive
        {
            get { return end; }
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
            if (startIndexInclusive > endIndexExclusive)
            {
                throw new IndexOutOfRangeException(String.Format("The new start index ('{0}' was given) cannot be greater than the end index ({1}).", startIndexInclusive, endIndexExclusive));
            }
            else if (startIndexInclusive < 0)
            {
                throw new IndexOutOfRangeException(String.Format("The new start index ('{0}' was given) cannot be less than 0.", startIndexInclusive));
            }
            else if (endIndexExclusive > baseArray.Length)
            {
                throw new IndexOutOfRangeException(String.Format("The new start index ('{0}' was given) cannot be greater than the length of the base array ({1}).", startIndexInclusive, baseArray.Length));
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

        public void moveStartBy(int amount)
        {
            moveStart(start + amount);
        }

        public void moveEndBy(int amount)
        {
            moveEnd(end + amount);
        }

        public bool isInBounds(int index)
        {
            return index >= 0 && index < length;
        }

        public T get(int index)
        {
            if (!isInBounds(index))
            {
                throw new IndexOutOfRangeException(string.Format("The index '{0}' is not in the range [0 - {1})", index, length));
            }
            int absIx = getAbsoluteIndex(index);
            return baseArray[absIx];
        }

        public void set(T element, int index)
        {
            if (!isInBounds(index))
            {
                throw new IndexOutOfRangeException(string.Format("The index '{0}' is not in the range [0 - {1})", index, length));
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

        public override string ToString()
        {
            string str = "VirtualArray [";
            foreach (T t in this)
            {
                str += string.Format("{0}, ", t);
            }
            str += "]";

            return str;
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
