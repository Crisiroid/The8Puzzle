using The8Puzzle;

class Program
{
    static void Main(string[] args)
    {
        //This input and IDA* don't work together. it takes more than 2 hours for IDA* to solve this input. 
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
        var AStarsolver = new AStarSolver(startState, goalState);

        var AStarSolution = AStarsolver.Solve();

        Console.WriteLine("__________________________A*__________________________");
        if (AStarSolution != null)
        {
            Console.WriteLine("Solution found!");
            foreach (var state in AStarSolution)
            {
                PrintState(state);
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("No solution found.");
        }

        Console.WriteLine("_________________________IDA*_________________________");
        var IDAsolver = new IDAStarSolver(startState, goalState);
        var IDAsolution = IDAsolver.Solve();

        if (IDAsolution != null)
        {
            Console.WriteLine("IDA* Solution found!");
            foreach (var state in IDAsolution)
            {
                PrintState(state);
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("No solution found.");
        }

        Console.WriteLine("_________________________BFS______________________");
        var BFSSolver = new BFSSolver(startState, goalState);
        var BFSSolution = BFSSolver.Solve();
        if (BFSSolution != null)
        {
            Console.WriteLine("BFS Solution found!");
            foreach (var state in BFSSolution)
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
