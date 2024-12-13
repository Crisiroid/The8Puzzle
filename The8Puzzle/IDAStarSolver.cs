using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The8Puzzle
{
    public class IDAStarSolver
    {
        private readonly int[,] startState;
        private readonly int[,] goalState;
        private readonly int rows = 3;
        private readonly int cols = 3;

        public IDAStarSolver(int[,] startState, int[,] goalState)
        {
            this.startState = startState;
            this.goalState = goalState;
        }

        public List<int[,]> Solve()
        {
            int threshold = CalculateHeuristic(startState);

            while (true)
            {
                var result = Search(startState, 0, threshold, null);
                if (result.Item1)
                    return ReconstructPath(result.Item2);

                if (result.Item3 == int.MaxValue)
                    return null; 

                threshold = result.Item3;
            }
        }

        private (bool, Node, int) Search(int[,] state, int g, int threshold, Node parent)
        {
            int f = g + CalculateHeuristic(state);
            if (f > threshold)
                return (false, null, f);

            if (IsGoal(state))
                return (true, new Node(state, parent, g, 0), threshold);

            int min = int.MaxValue;
            int zeroRow = 0, zeroCol = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (state[i, j] == 0)
                    {
                        zeroRow = i;
                        zeroCol = j;
                    }
                }
            }

            var directions = new[] { (0, -1), (0, 1), (-1, 0), (1, 0) };

            foreach (var (dx, dy) in directions)
            {
                int newRow = zeroRow + dx;
                int newCol = zeroCol + dy;

                if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols)
                {
                    var newState = (int[,])state.Clone();
                    newState[zeroRow, zeroCol] = newState[newRow, newCol];
                    newState[newRow, newCol] = 0;

                    if (parent != null && AreStatesEqual(newState, parent.State))
                        continue;

                    var result = Search(newState, g + 1, threshold, new Node(state, parent, g, 0));
                    if (result.Item1)
                        return result;

                    min = Math.Min(min, result.Item3);
                }
            }

            return (false, null, min);
        }

        private int CalculateHeuristic(int[,] state)
        {
            int heuristic = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int value = state[i, j];
                    if (value == 0) continue;

                    int targetX = (value - 1) / cols;
                    int targetY = (value - 1) % cols;
                    heuristic += Math.Abs(i - targetX) + Math.Abs(j - targetY);
                }
            }
            return heuristic;
        }

        private bool IsGoal(int[,] state)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (state[i, j] != goalState[i, j])
                        return false;
                }
            }
            return true;
        }

        private List<int[,]> ReconstructPath(Node node)
        {
            var path = new List<int[,]>();
            while (node != null)
            {
                path.Add(node.State);
                node = node.Parent;
            }
            path.Reverse();
            return path;
        }

        private bool AreStatesEqual(int[,] state1, int[,] state2)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (state1[i, j] != state2[i, j])
                        return false;
                }
            }
            return true;
        }

        private class Node
        {
            public int[,] State { get; }
            public Node Parent { get; }
            public int Cost { get; }
            public int Heuristic { get; }

            public Node(int[,] state, Node parent, int cost, int heuristic)
            {
                State = state;
                Parent = parent;
                Cost = cost;
                Heuristic = heuristic;
            }
        }
    }

}
