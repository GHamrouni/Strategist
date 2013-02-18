using System;
using System.Collections.Generic;
using System.Text;

namespace Strategist
{
    [Serializable]
    public class Tuple<T1, T2>
    {
        private readonly T1 m_Item1;
        private readonly T2 m_Item2;

        public T1 Item1
        {
            get
            {
                return this.m_Item1;
            }
        }

        public T2 Item2
        {
            get
            {
                return this.m_Item2;
            }
        }

        public Tuple(T1 item1, T2 item2)
        {
            this.m_Item1 = item1;
            this.m_Item2 = item2;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Tuple<T1, T2>))
            {
                var t = (Tuple<T1, T2>)obj;

                return t.Item1.Equals(Item1) && t.Item2.Equals(Item2);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;

            hashCode = hashCode * 31 + Item1.GetHashCode();
            hashCode = hashCode * 31 + Item2.GetHashCode();

            return hashCode;
        }
    }
}
