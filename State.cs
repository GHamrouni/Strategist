using System;
using System.Collections.Generic;
using System.Text;

namespace Strategist
{
    public class State
    {
        private int[] states;

        // The length of the shortest path from
        // the start to this state g(s)
        public double Score { get; set; }

        public double RHS { get; set; }

        public int nVisits { get; set; }

        public State Parent { get; set; }

        public bool IsIdle { get; set; }

        public State FirstChild { get; set; }

        public State LastChild { get; set; }

        public State NextSibling { get; set; }

        public int Time { get; set; }

        public int Dimension
        {
            get
            {
                return states.Length;
            }
        }

        public State(int dim)
        {
            states = new int[dim];

            Score = 0;

            RHS = 0;

            Time = 0;

            IsIdle = false;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(State))
            {
                var st = (State)obj;

                if (st.Dimension != Dimension)
                    return false;

                for (int i = 0; i < Dimension; i++)
                    if (st[i] != states[i])
                        return false;

                if (st.IsIdle || IsIdle)
                    return st.Time == Time;

                return true;
            }

            return false;
        }

        public override int  GetHashCode()
        {
            int hashCode = 0;

            for (int i = 0; i < Dimension; i++)
                hashCode = hashCode * 31 + states[i];

            if (IsIdle)
                hashCode = hashCode * 31 + Time;

            return hashCode;
        }

        public bool IsLeaf()
        {
            return FirstChild == null;
        }

        public State Clone()
        {
            State clone = new State(Dimension);

            for (int i = 0; i < Dimension; i++)
                clone[i] = states[i];

            clone.Time = Time;

            return clone;
        }

        public int this[int key]
        {
            get
            {
                return states[key];
            }
            set
            {
                states[key] = value;
            }
        }

        public State MostExploredChild()
        {
            var child = FirstChild;
            var selected = child;

            while (child != null)
            {
                if (selected.nVisits < child.nVisits)
                {
                    selected = child;
                }

                child = child.NextSibling;
            }

            return selected;
        }

        public State AddChild(State child)
        {
            child.Parent = this;

            if (FirstChild == null)
                FirstChild = child;

            if (LastChild != null)
                LastChild.NextSibling = child;

            LastChild = child;

            return child;
        }

        public void AddChilds(List<State> childs)
        {
            foreach (var child in childs)
                AddChild(child);
        }

        public override string ToString()
        {
            String str = "( ";

            for (int i = 0; i < Dimension; i++)
                str += states[i].ToString() + " ";

            str = str + ")";

            return str;
        }
    }
}
