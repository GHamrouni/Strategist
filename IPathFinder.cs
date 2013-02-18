using System;
using System.Collections.Generic;
using System.Text;

namespace Strategist
{
    public interface IPathFinder
    {
        State FindPath(State root, int iterationsNb);
    }
}
