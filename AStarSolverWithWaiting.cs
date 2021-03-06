﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Strategist
{
    public class AStarSolverWithWaiting : IPathFinder
    {
        IStateGenerator generator;
        IEvaluator evaluator;

        BinaryHeap<State> open    = new BinaryHeap<State>(1024 * 4);
        HashTable<State> openHash = new HashTable<State> (1024 * 4);
        HashTable<State> closed   = new HashTable<State> (1024 * 4);

        public AStarSolverWithWaiting(IStateGenerator generator, IEvaluator evaluator)
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

                double H = evaluator.Evaluate(state);
                double minH = 0;

                List<State> idleStates = new List<State>();

                var nextStates = generator.Succ(state);

                if (nextStates.Count > 0)
                    minH = evaluator.Evaluate(nextStates[0]);

                bool pathFound = false;

                foreach (var nextState in nextStates)
                {
                    if (!closed.Contains(nextState))
                    {
                        double g = state.Score + evaluator.Cost(state, nextState);
                        double h = evaluator.Evaluate(nextState);

                        if (nextState.IsIdle)
                        {
                            idleStates.Add(nextState);
                        }
                        else
                        {
                            minH = Math.Min(minH, h);

                            if (openHash.Contains(nextState))
                            {
                                int findex = open.ContainsValue(nextState);

                                if (g < open[findex].Item2.Score)
                                {
                                    open.RemoveValueAt(findex);
                                    open.Insert(g + h, nextState);
                                    nextState.Score = g;
                                    state.AddChild(nextState);

                                    pathFound = true;
                                }
                            }
                            else
                            {
                                open.Insert(g + h, nextState);
                                openHash.Add(nextState);
                                nextState.Score = g;
                                state.AddChild(nextState);

                                pathFound = true;
                            }
                        }
                    }
                }

                if (minH > H || !pathFound)
                {
                    for (int i = 0; i < idleStates.Count; i++)
                    {
                        var nextState = idleStates[i];

                        double g = state.Score + evaluator.Cost(state, nextState);

                        open.Insert(g + H, nextState);
                        openHash.Add(nextState);
                        nextState.Score = g;
                        state.AddChild(nextState);
                    }
                }

                time++;
            }

            return null;
        }
    }
}
