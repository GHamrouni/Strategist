using System;
using System.Collections.Generic;
using System.Text;

namespace Strategist
{
    /// <summary>
    /// A very simple implementation of a hash table 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HashTable<T>
    {
        int NHash = 128;

        LinkedList<T>[] table;

        public HashTable(int nhash)
        {
            NHash = nhash;

            table = new LinkedList<T>[NHash];

            for (int i = 0; i < NHash; i++)
                table[i] = new LinkedList<T>();
        }

        public void Add(T item)
        {
            int h = item.GetHashCode();

            table[h % NHash].AddLast(item);
        }

        public void Delete(T item)
        {
            int h = item.GetHashCode();
            int n = h % NHash;

            table[n].Remove(item);
        }

        public bool Contains(T item)
        {
            int h = item.GetHashCode();
            int n = h % NHash;

            return table[n].Contains(item);
        }
    }
}
