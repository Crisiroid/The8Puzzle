using System.Diagnostics;

namespace The8Puzzle
{
    public class BFSSolver
    {
        private readonly int[,] startState;
        private readonly int[,] goalState;
        private readonly int rows = 3;
        private readonly int cols = 3;

        public BFSSolver(int[,] startState, int[,] goalState)
        {
            this.startState = startState;
            this.goalState = goalState;
        }

        public List<int[,]> Solve()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start(); 

            var openList = new Queue<Node>();  
            var closedList = new HashSet<string>(); 
            openList.Enqueue(new Node(startState, null));

            while (openList.Count > 0)
            {
                if (stopwatch.ElapsedMilliseconds > 30000) 
                {
                    Console.WriteLine("Algorithm isn't working with the input. Time exceeded 30 seconds.");
                    return null;
                }

                var currentNode = openList.Dequeue(); 

                if (IsGoal(currentNode.State))
                    return ReconstructPath(currentNode);

                closedList.Add(GetStateKey(currentNode.State));

                int zeroRow = 0, zeroCol = 0;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (currentNode.State[i, j] == 0)
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
                        var newState = (int[,])currentNode.State.Clone();
                        newState[zeroRow, zeroCol] = newState[newRow, newCol];
                        newState[newRow, newCol] = 0;

                        if (closedList.Contains(GetStateKey(newState)))
                            continue;

                        openList.Enqueue(new Node(newState, currentNode)); 
                    }
                }
            }

            return null; 
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

        private string GetStateKey(int[,] state)
        {
            return string.Join(",", state.Cast<int>());
        }

        private class Node
        {
            public int[,] State { get; }
            public Node Parent { get; }

            public Node(int[,] state, Node parent)
            {
                State = state;
                Parent = parent;
            }
        }
    }
}
