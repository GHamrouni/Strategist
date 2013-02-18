using System;
using System.Collections.Generic;
using System.Text;

namespace Strategist
{
    public interface IStateGenerator
    {
        List<State> Succ(State state);

        bool IsValid(State state);
    }
}
