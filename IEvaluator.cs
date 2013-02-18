using System;
using System.Collections.Generic;
using System.Text;

namespace Strategist
{
    public interface IEvaluator
    {
        double Evaluate(State state);

        double Cost(State x, State y); 

        bool IsEndGame(State state);
    }
}
