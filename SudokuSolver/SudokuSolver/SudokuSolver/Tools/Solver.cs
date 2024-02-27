using SudokuSolver.Models;

namespace SudokuSolver.Tools
{
    internal static class Solver
    {
        public const int Size = 9;
        private static readonly int _boxSize = (int)Math.Sqrt(Size);

        private static readonly List<SudokuCell> _cellsToSolve = new();
        private static string[] _sudoku = new[] { "IMPOSSIBLE" };
        private readonly static SudokuCell[,] _cells = new SudokuCell[Size, Size];

        public static void Input()
        {
            _sudoku = new string[Size];

            for (int i = 0; i < Size; i++)
                _sudoku[i] = Console.ReadLine() ?? "";
        }

        public static string[] Solve()
        {
            InitializeCells();

            return SolveNext(0)
                 ? SolutionToTextFancy()
                 : new[] { "IMPOSSIBLE" };
        }

        private static bool SolveNext(int index)
        {
            if (index == _cellsToSolve.Count)
                return true;

            var cell = _cellsToSolve[index];
            var markers = GetMarkers(cell);

            cell.Solved = true;

            foreach (var marker in markers)
            {
                cell.Number = marker;

                if (SolveNext(index + 1))
                    return true;
            }

            cell.Solved = false;
            return false;
        }

        private static List<int> GetMarkers(SudokuCell cell)
        {
            var row = GetRow(cell.X);
            var column = GetColumn(cell.Y);
            var box = GetBox(cell.X, cell.Y);
            var solvedNumbers = row.Concat(column)
                                   .Concat(box)
                                   .Where(x => x.Solved)
                                   .Select(x => x.Number)
                                   .ToHashSet();

            var markers = new List<int>();

            for (int possibleNumber = 1; possibleNumber <= Size; possibleNumber++)
            {
                if (!solvedNumbers.Contains(possibleNumber))
                    markers.Add(possibleNumber);
            }

            return markers;
        }

        private static IEnumerable<SudokuCell> GetRow(int index)
        {
            for (int i = 0; i < Size; i++)
                yield return _cells[index, i];
        }

        private static IEnumerable<SudokuCell> GetColumn(int index)
        {
            for (int i = 0; i < Size; i++)
                yield return _cells[i, index];
        }

        private static IEnumerable<SudokuCell> GetBox(int iIndex, int jIndex)
        {
            var iBox = iIndex / _boxSize;
            var jBox = jIndex / _boxSize;

            for (int i = iBox * _boxSize; i < (iBox + 1) * _boxSize; i++)
                for (int j = jBox * _boxSize; j < (jBox + 1) * _boxSize; j++)
                    yield return _cells[i, j];
        }

        private static void InitializeCells()
        {
            _cellsToSolve.Clear();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _cells[i, j] = new SudokuCell
                    {
                        X = i,
                        Y = j,
                        Solved = _sudoku[i][j] != 'x',
                        Number = _sudoku[i][j] - '0'
                    };

                    if (!_cells[i, j].Solved)
                        _cellsToSolve.Add(_cells[i, j]);
                }
            }

        }

        private static string[] SolutionToTextFancy()
        {
            var textSolution = new string[13];
            var display = new char[13][];
            display[0]  = "┌───┬───┬───┐".ToCharArray();
            display[1]  = "│   │   │   │".ToCharArray();
            display[2]  = "│   │   │   │".ToCharArray();
            display[3]  = "│   │   │   │".ToCharArray();
            display[4]  = "├───┼───┼───┤".ToCharArray();
            display[5]  = "│   │   │   │".ToCharArray();
            display[6]  = "│   │   │   │".ToCharArray();
            display[7]  = "│   │   │   │".ToCharArray();
            display[8]  = "├───┼───┼───┤".ToCharArray();
            display[9]  = "│   │   │   │".ToCharArray();
            display[10] = "│   │   │   │".ToCharArray();
            display[11] = "│   │   │   │".ToCharArray();
            display[12] = "└───┴───┴───┘".ToCharArray();

            var customPositionsX = (new[] { 1, 2, 3, 5, 6, 7, 9, 10, 11 } as IEnumerable<int>).GetEnumerator();
            var customPositionsY = (new[] { 1, 2, 3, 5, 6, 7, 9, 10, 11 } as IEnumerable<int>).GetEnumerator();

            for (int i = 0; customPositionsX.MoveNext(); i++)
            {
                foreach (var number in GetRow(i).Select(x => x.Number))
                {
                    customPositionsY.MoveNext();
                    display[customPositionsX.Current][customPositionsY.Current] = (char)('0' + number);
                }

                customPositionsY.Reset();
            }

            for (int i = 0; i < 13; i++)
                textSolution[i] = new string(display[i]);

            return textSolution;
        }
    }
}

