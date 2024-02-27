namespace SudokuSolver.Models
{
    public class SudokuCell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Solved { get; set; }

        public int Number { get; set; }

        public override string ToString()
            => Solved ? Number.ToString() : $"x";
    }
}
