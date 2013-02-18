using System;
using System.Collections.Generic;
using System.Text;

namespace Strategist
{
    public class AStarSolver : IPathFinder
    {
        IStateGenerator generator;
        IEvaluator evaluator;

        BinaryHeap<State> open     = new BinaryHeap<State>(1024 * 4);
        HashTable<State>  openHash = new HashTable<State> (1024 * 4);
        HashTable<State>  closed   = new HashTable<State> (1024 * 4);

        public AStarSolver(IStateGenerator generator, IEvaluator evaluator)
        {
            this.generator = generator;
            this.evaluator = evaluator;
        }

        public State FindPath(State root, int maxIteration)
        {
            root.Score = 0;
            open.Insert(evaluator.Evaluate(root), root);
            openHash.Add(root);

            int time = 0;

            while (!open.Empty())
            {
                if (maxIteration > 0)
                    if (time > maxIteration)
                        break;

                var state = open.Pop();
                openHash.Delete(state);

                if (evaluator.IsEndGame(state))
                    return state;

                closed.Add(state);

                var nextStates = generator.Succ(state);

                foreach (var nextState in nextStates)
                {
                    if (!closed.Contains(nextState))
                    {
                        double g = state.Score + evaluator.Cost(state, nextState);
                        double h = evaluator.Evaluate(nextState);

                        if (openHash.Contains(nextState))
                        {
                            int findex = open.ContainsValue(nextState);

                            if (g < open[findex].Item2.Score)
                            {
                                open.RemoveValueAt(findex);
                                open.Insert(g + h, nextState);
                                nextState.Score = g;
                                state.AddChild(nextState);
                            }
                        }
                        else
                        {
                            open.Insert(g + h, nextState);
                            openHash.Add(nextState);
                            nextState.Score = g;
                            state.AddChild(nextState);
                        }
                    }
                }

                time++;
            }

            return null;
        }
    }
}
