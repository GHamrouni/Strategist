using System;
using System.Collections.Generic;
using System.Text;

namespace Strategist
{
    public class BinaryHeap<T>
    {
        uint size;
        uint filled_elements;
        Tuple<double, T>[] buffer;

        public BinaryHeap(uint size)
        {
            this.size = size;

            buffer = new Tuple<double, T>[size];

            filled_elements = 0;
        }

        public Tuple<double, T> this[int i]
        {
            get
            {
                return buffer[i];
            }
            set
            {
                buffer[i] = value;
            }
        }

        void Grow()
        {
            uint newSize = 2 * size;
            var newBuffer = new Tuple<double, T>[newSize];

            for (int i = 0; i < filled_elements; i++)
                newBuffer[i] = buffer[i];

            buffer = newBuffer;
            size = newSize;
        }

        int
        GetParentIndex(uint i)
        {
            return (int) (i - 1) / 2;
        }

        uint
        GetRChildIndex(uint i)
        {
            return 2 * i + 2;
        }

        uint
        GetLChildIndex(uint i)
        {
            return 2 * i + 1;
        }

        void
        BalanceHeap(uint currentSize)
        {
            uint i = currentSize - 1;

            while (GetParentIndex(i) >= 0
                   &&
                   buffer[i].Item1 < buffer[GetParentIndex(i)].Item1)
            {
                var value = buffer[i];
                buffer[i] = buffer[GetParentIndex(i)];
                buffer[GetParentIndex(i)] = value;

                i = (uint) GetParentIndex(i);
            }
        }

        void
        BalanceChildren(uint i)
        {
            uint mc;

            uint rc = GetRChildIndex(i);
            uint lc = GetLChildIndex(i);

            if (lc >= filled_elements)
                return;

            if (rc < filled_elements)
            {
                if (buffer[rc].Item1 < buffer[lc].Item1)
                    mc = rc;
                else
                    mc = lc;
            }
            else
                mc = lc;

            if (buffer[mc].Item1 < buffer[i].Item1)
            {
                var value = buffer[mc];
                buffer[mc] = buffer[i];
                buffer[i] = value;

                BalanceChildren(mc);
            }
        }

        public int ContainsValue(T value)
        {
            for (int i = 0; i < filled_elements; i++)
            {
                if (buffer[i].Item2.Equals(value))
                {
                    return i;
                }
            }

            return -1;
        }

        public void RemoveValueAt(int index)
        {
            for (int i = index; i < filled_elements - 1; i++)
            {
                buffer[i] = buffer[i + 1];
                BalanceHeap((uint) i + 1);
            }

            buffer[filled_elements - 1] = null;

            filled_elements--;

            //balance_children((uint) index);
        }

        public bool Empty()
        {
            return filled_elements == 0;
        }

        public
        T
        Pop()
        {
            if (filled_elements <= 0)
                return default(T);

            var value = buffer[0].Item2;

            buffer[0] = buffer[filled_elements - 1];
            buffer[filled_elements - 1] = null;
            filled_elements--;

            BalanceChildren(0);

            return value;
        }

        public
        void
        Insert(double priority, T value)
        {
            if (filled_elements == 0)
            {
                filled_elements = 1;
                buffer[0] = new Tuple<double,T>(priority, value);

                return;
            }

            if (filled_elements >= size)
                Grow();

            if (filled_elements < size)
            {
                buffer[filled_elements] = new Tuple<double, T>(priority, value);
                filled_elements++;

                BalanceHeap(filled_elements);

                return;
            }

            buffer[0] = new Tuple<double, T>(priority, value);
            BalanceChildren(0);
        }
    }
}
