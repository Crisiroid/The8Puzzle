namespace The8Puzzle
{
    public class AStarSolver
    {
        private readonly int[,] startState;
        private readonly int[,] goalState;
        private readonly int rows = 3;
        private readonly int cols = 3;

        public AStarSolver(int[,] startState, int[,] goalState)
        {
            this.startState = startState;
            this.goalState = goalState;
        }

        public List<int[,]> Solve()
        {
            var openSet = new PriorityQueue<Node>();
            var closedSet = new HashSet<string>();

            var startNode = new Node(startState, null, 0, CalculateHeuristic(startState));
            openSet.Enqueue(startNode);

            while (openSet.Count > 0)
            {
                var currentNode = openSet.Dequeue();

                if (IsGoal(currentNode.State))
                {
                    return ReconstructPath(currentNode);
                }

                closedSet.Add(GetStateKey(currentNode.State));

                foreach (var neighbor in GetNeighbors(currentNode))
                {
                    if (closedSet.Contains(GetStateKey(neighbor.State)))
                        continue;

                    openSet.Enqueue(neighbor);
                }
            }

            return null; 
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

        private IEnumerable<Node> GetNeighbors(Node node)
        {
            var neighbors = new List<Node>();
            int zeroRow = 0, zeroCol = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (node.State[i, j] == 0)
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
                    var newState = (int[,])node.State.Clone();
                    newState[zeroRow, zeroCol] = newState[newRow, newCol];
                    newState[newRow, newCol] = 0;

                    neighbors.Add(new Node(newState, node, node.Cost + 1, CalculateHeuristic(newState)));
                }
            }

            return neighbors;
        }

        private string GetStateKey(int[,] state)
        {
            return string.Join(",", state.Cast<int>());
        }

        private class Node : IComparable<Node>
        {
            public int[,] State { get; }
            public Node Parent { get; }
            public int Cost { get; }
            public int Heuristic { get; }
            public int TotalCost => Cost + Heuristic;

            public Node(int[,] state, Node parent, int cost, int heuristic)
            {
                State = state;
                Parent = parent;
                Cost = cost;
                Heuristic = heuristic;
            }

            public int CompareTo(Node other)
            {
                return TotalCost.CompareTo(other.TotalCost);
            }
        }

        private class PriorityQueue<T> where T : IComparable<T>
        {
            private readonly List<T> elements = new List<T>();

            public int Count => elements.Count;

            public void Enqueue(T item)
            {
                elements.Add(item);
                elements.Sort();
            }

            public T Dequeue()
            {
                var item = elements[0];
                elements.RemoveAt(0);
                return item;
            }
        }
    }

}
