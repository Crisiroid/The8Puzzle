using The8Puzzle;

class Program
{
    static void Main(string[] args)
    {
        int[,] startState =
        {
            { 1, 2, 3 },
            { 4, 0, 5 },
            { 6, 7, 8 }
        };

        int[,] goalState =
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 0 }
        };

        var solver = new AStarSolver(startState, goalState);
        var solution = solver.Solve();

        if (solution != null)
        {
            Console.WriteLine("Solution found!");
            foreach (var state in solution)
            {
                PrintState(state);
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("No solution found.");
        }
    }

    static void PrintState(int[,] state)
    {
        for (int i = 0; i < state.GetLength(0); i++)
        {
            for (int j = 0; j < state.GetLength(1); j++)
            {
                Console.Write(state[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}
