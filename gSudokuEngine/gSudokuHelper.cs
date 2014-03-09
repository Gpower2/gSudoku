using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gSudokuEngine
{
    /// <summary>
    /// The Matrix is a table of booleans.  It could be represented
    /// in a more compact manner, using bits, but for now we'll use
    /// bools.
    ///
    /// Each column corresponds to a constraint, in this way:
    ///
    /// - The first 81 columns represent the constraint that each
    ///   cell must have exactly one digit in it.  Each cell in the
    ///   9x9 sudoku, starting from top left, moving right across the
    ///   row, then down through each row, maps to a single column in
    ///   the matrix.  The middle cell, in the upper right box in the
    ///   Sudoku puzzle (cell [1,7]) (counting from zero),
    ///   corresponds to column 16 (counting from zero) in the
    ///   matrix.
    ///
    ///      Formula: MC1 = R*9 + C
    ///
    /// - The next 81 columns represent the row constraint: that
    ///   each row in the sudoku must contain each digit 1-9.
    ///   Each column in this group means "row A has digit
    ///   N". Therefore, for a sudoku row with 9 required values,
    ///   there will be nine columns in the matrix. The fact that
    ///   the 4th row (counting from zero) must contain a 7 is
    ///   encoded in the 42nd column in this batch, or the 122nd
    ///   column overall. Assume R is the row (starting from zero)
    ///   and V is the value:
    ///
    ///      Formula: MC2 = 81 + R*9 + (V - 1)
    ///
    /// - The next 81 columns model the column constraint in the
    ///   sudoku puzzle, in the same way the row constraint is
    ///   modeled.  Assuming C is the column number, starting from
    ///   zero, and V is the value in the column:
    ///
    ///      Formula: MC3 = (81*2) + C*9 + (V - 1)
    ///
    /// - The next 81 columns model the box constraint in the
    ///   sudoku puzzle, in the same way. B is the box
    ///   number, starting from zero in the upper left, moving across
    ///   so that box 2 is the upper right.
    ///
    ///      Formula: B = ((R/3)*3 + (C/3);
    ///      Formula: MC4 = (81*3) + B*9 + (V - 1)
    ///
    ///  Each row in the matrix corresponds to the presence of a
    ///  particular value in a particular cell.  Since there are
    ///  9 values and 81 cells, there are 720 rows.  The
    ///  placement is done this way: If a 7 is present in cell
    ///  [2,3] (counting from zero), then all true values for
    ///  that vell will be placed in row 21.
    ///
    ///    Formula:  MR = (R*9 + C)*9 + (V-1)
    ///
    /// The appearance of a particular number into a particular
    /// row,col in the Sudoku implies the placement of four 1's
    /// (true values) in the matrix, all in a particular row.
    ///
    /// </summary>
    public class gSudokuHelper
    {
        private Boolean[,] _Matrix;
        private Random _rnd;

        /// <summary>
        /// Constructor that creates the base matrix
        /// which contains the constrains for the exact cover
        /// </summary>
        public gSudokuHelper()
        {
            CreateBaseMatrix();
        }

        /// <summary>
        ///             
        /// apply the basic constraints
        /// 
        /// There are 729 rows in the matrix that models a 9x9 sudoku
        /// - each row corresponds to a distinct tuple (cell,value)
        /// in the Sudoku puzzle. If value 6 is in cell [3,4], that
        /// corresponds to  MR = (r*D + c)*D + (v-1), or row 284 in the
        /// matrix.
        /// 
        /// There are 324 columns in the matrix, divided into 4
        /// sections of 81, each one corresponding to a distinct
        /// class of constraint.
        /// 
        /// Constraint 1: The first slice of 81 columns express the
        /// constraint that each cell must have exactly one value.
        /// Cell [3,4] in the sudoku is represented by column r*9+c =
        /// 31 in the matrix.  From above, for a given cell in the
        /// Sudoku, there are 9 rows in the solution Matrix. Each
        /// cell in column 31 for those 9 rows, must get a 1 to
        /// express this constraint.  To generalize, in column N
        /// corresponding to sudoku cell c, there must be nine cells
        /// containing a 1, one for each the 9 rows corresponding to
        /// the various values cell c can take.  This results in a
        /// vertical line segment of 1's in the matrix in the given
        /// column.
        /// 
        /// Constraint 2: This is expressed in the 2nd slice of 81
        /// columns of the matrix. the constraint is that each row in
        /// the sudoku must contain all values, from 1-9. Each column
        /// in this slice corresponds to a tuple (row,value). Because
        /// there are slices of rows in the matrix representing
        /// cell/value tuples, when modelling this constraint, there
        /// appear diagonal lines of 1's. Imagine it: a slice of 9
        /// columns in which the 0th represents the fact that a 1 is
        /// in the column in the sudoku puzzle; the 1st means a 2 is
        /// present somewhere in the given column in the sudoku; the
        /// 2nd for a 3 in the column; etc.  Horizontally there is a
        /// slice of rows representing cell values.  There are 9 rows
        /// for cell X, each representing a different value that
        /// could be contained in that cell, 1-9.  When the 0th row
        /// is populated, it means a 1 is in the cell in the sudoku,
        /// which means the column corresponding to "1 present in the
        /// row" is a 1.  When the 1st row in the slice is populated,
        /// it means a 2 is in the cell in the sudoku, which means
        /// the column corresponding to "2 present in the row" is a
        /// 1. And so on, which results in a diagnonal of 1s in the
        /// matrix.  This continbues for each respective slice of
        /// rows (one slice per cell in the sudocku puzzle), and each
        /// subslice of columns,q each one corresponding to a set of
        /// values appearing in a particular row in the sudoku
        /// puzzle.
        /// 
        /// Constraints 3 and 4 are modeled in the next two slices of
        /// 81 columns. They work the same way as the constraint #2.
        /// The tuple for 3, the column constraint, is (col,val) and
        /// the tuple for slice #4, the box constraint is (box,val).
        /// It works the same way, but the patterns in the matrix are
        /// slightly different, only because C and B vary differently
        /// with respect to the row number inthe matrix.  The result
        /// is still regularly repeating diagonals.
        /// </summary>
        private void CreateBaseMatrix()
        {
            int nClassesOfConstraint = 4;
            int sliceOffset = 81; //9 * 9
            int d = 3; //Math.Sqrt(9)
            int nCol = 81 * nClassesOfConstraint; // == 324 for a 9x9 sudoku
            int nRow = 729; // == 729  for a 9x9 sudoku

            _Matrix = new bool[nRow, nCol];

            for (int r = 0; r < 9; r++) // row in sudoku
            {
                for (int c = 0; c < 9; c++) // col in sudoku
                {
                    // MCx is Matrix Column for constraint x
                    int MC1 = r * 9 + c; // M.C. for this cell
                    int B = ((r / d) * d) + (c / d);  // box for this cell
                    for (int v = 1; v <= 9; v++)
                    {
                        int MC2 = (sliceOffset * 1) + r * 9 + (v - 1); // M.C. for this row/value
                        int MC3 = (sliceOffset * 2) + c * 9 + (v - 1); // M.C. for col/val
                        int MC4 = (sliceOffset * 3) + B * 9 + (v - 1); // M.C. for box/val
                        int MR = (r * 9 + c) * 9 + (v - 1);

                        _Matrix[MR, MC1] = true;
                        _Matrix[MR, MC2] = true;
                        _Matrix[MR, MC3] = true;
                        _Matrix[MR, MC4] = true;
                    }
                }
            }
        }

        /// <summary>
        ///   Add a number from the sudoku puzzle into the matrix.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     Really this doesn't "add" numbers into the matrix. It
        ///     *could* do so, but need not. All that is necessary is to
        ///     return the row number in the matrix corresponding to
        ///     this (row, column, value) triple from the Sudoku
        ///     puzzle. Before search befins, the ExactCover solver
        ///     needs to remove (cover) all the columns which have 1s in
        ///     this row. This represents the "given" part of the
        ///     solution.
        ///   </para>
        /// // If we wanted to add values into the matrix, we could do so.
        /// // It's unnecessary because these rows will be removed anyway,
        /// // pre-search.
        /// int MC1 = R*9 + C;
        /// int MC2 = 81 + R*9 + (V - 1);
        /// int MC3 = (81*2) + C*9 + (V - 1);
        /// int B =  ((R/3)*3) + (C/3);
        /// int MC4 = (81*3) + B*9 + (V - 1);
        ///
        /// Console.WriteLine(" {0}  {1}  {2}    {3:D2}   {4:D3}   {5:D3}   " +
        ///                   "{6}  {7:D3}  {8:D3}",
        ///                   V, R, C, MC1, MC2, MC3, B, MC4, MR);
        ///
        /// Matrix[MR,MC1] = true;
        /// Matrix[MR,MC2] = true;
        /// Matrix[MR,MC3] = true;
        /// Matrix[MR,MC4] = true;
        ///   
        /// </remarks>
        private int AddNumber(int value, int row, int col)
        {
            if (value == 0) return 0; // no value, nothing to do
            int MR = (row * 9 + col) * 9 + (value - 1);
            return MR;
        }

        private Stack<int> GetStartingState(gSudokuCell[][] board)
        {
            Stack<int> startingState = new Stack<int>();
            for (int i = 0; i < 9; i++) // rows
            {
                for (int j = 0; j < 9; j++) // cols
                {
                    if (board[i][j].ValuesCount == 1)
                    {
                        startingState.Push(AddNumber(board[i][j].SingleValue, i, j));
                    }
                    //if (puzzle[i,j]!=0) // is there a value in this cell?
                    //    startingState.Push(AddNumber(puzzle[i,j], i, j));
                }
            }
            return startingState;
        }

        /// <summary>
        ///   Display a representation of the solution.
        /// </summary>
        /// <remarks>
        ///     The stack of integers represents the rows
        ///     in the Matrix that solve the exact cover problem.
        ///     Use that to extract the information about the
        ///     positions of the values in the sudoku puzzle, applying
        ///     the rules we used in AddNumber, in reverse.
        /// Each row corresponds to a particular value in a
        /// particular position in the puzzle, according to this
        /// formula:
        ///
        /// int MR = (R*9 + C)*9 + (V-1);
        ///
        /// To solve for R, C, and V, we use this line of reasoning:
        ///
        ///   (R*9 + c)*9 =  MR - (V-1)
        ///   (R*9 + c) =  (MR - floor(V-1))/9
        ///
        /// But  V-1 will always be less than 9, so
        ///
        ///   (R*9 + c) =  floor( MR/9 )
        ///
        /// and
        ///   R*9 = MR/9 - C
        ///
        /// But C will also always be less than 9, so
        ///
        ///   R =  floor(MR/81);
        ///
        /// And then, solving:
        ///
        ///   C =  floor(MR/9) - R*9
        ///
        ///   V =  MR -  (R*9 + C)*9 + 1
        ///   
        /// </remarks>
        private void FillSolution(Stack<int> solution, gSudokuCell[][] board)
        {
            //for (int i=0; i < D; i++)
            //    for (int j=0; j < D; j++)
            //        puzzle[i,j]= 0;
            List<gSudokuCell> protectedCells = new List<gSudokuCell>();
            for (Int32 i = 0; i < 9; i++)
            {
                for (Int32 j = 0; j < 9; j++)
                {
                    if (board[i][j].IsProtected)
                    {
                        protectedCells.Add(board[i][j]);
                    }
                    board[i][j].IsProtected = false;
                    board[i][j].ClearValues();
                }
            }

            foreach (int MR in solution)
            {
                int R = MR / (9 * 9);
                int C = (MR / 9) - (R * 9);
                int V = MR - (R * 9 + C) * 9 + 1;

                //puzzle[R,C] = (Int16)V;
                board[R][C].AddValue(V);
                if (protectedCells.Contains(board[R][C]))
                {
                    board[R][C].IsProtected = true;
                }
            }
        }

        public void Solve(gSudokuCell[][] board)
        {
            //CreateBaseMatrix();

            // pstate just lists the rows to remove from the
            // matrix. They represent numbers already present in the
            // Sudoku puzzle - the given numbers. They are presumed part
            // of the solution, and so the column headers corresponding
            // to 1's in these rows need to be removed from the matrix.
            Stack<int> pstate = GetStartingState(board);

            ExactCover ec = new ExactCover(_Matrix);
            if (ec.Solve(pstate))
            {
                FillSolution(pstate, board);
            }
            else
            {
                throw new Exception("Not solvable!");
                //Console.WriteLine("\nNot solvable.");
            }
        }

        private int SelectRandom(List<int> list)
        {
            int ix = _rnd.Next(list.Count);
            int value = list[ix];
            list.RemoveAt(ix);
            return value;
        }

        private List<int> AvailableCells(gSudokuCell[][] board)
        {
            List<int> list = new List<int>();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i][j].ValuesCount == 1)
                    {
                        list.Add(i * 9 + j);
                    }
                    //if (puzzle[i, j] != 0)
                    //    list.Add(i * D + j);
                }
            }
            return list;
        }

        public Boolean UniquelySolvable(gSudokuCell[][] board, Int32 maxTries)
        {
            gSudokuCell[][] solBoard = gSudokuGame.CloneBoard(board);
            ExactCover ec = new ExactCover(_Matrix);
            Stack<int> pstate = GetStartingState(board);           
            if (!ec.Solve(pstate))
                throw new Exception("unsolvable puzzle!");
            FillSolution(pstate, solBoard);           
            String target = gSudokuGame.GetBoardStringIdentity(solBoard);
            
            return UniquelySolvable(ec, target, board, maxTries);
        }

        /// <summary>
        ///   Determine whether a puzzle is uniquely solvable.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     I don't know how to do this formally; the hueristic I
        ///     use is, if the same solution is returned N times from
        ///     the non-deterministic Algorithm X.  Currently N = 11.
        ///   </para>
        ///   <para>
        ///     I have seen puzzles fail this test after 6 cycles!  In
        ///     other words, 5 times it was solved the same way, and on
        ///     the 6th a different solution was found, ergo, not
        ///     unique.
        ///   </para>
        /// </remarks>
        private bool UniquelySolvable(ExactCover ec, String target, gSudokuCell[][] board, Int32 maxTries)
        {
            gSudokuCell[][] soln = gSudokuGame.GetCleanBoard();

            //Int16[,] soln = new Int16[D, D];
            for (int i = 0; i < maxTries; i++)
            {
                Stack<int> pstate = GetStartingState(board);

                if (!ec.Solve(pstate))
                    throw new Exception("unsolvable puzzle, ??"); // never

                FillSolution(pstate, soln);
                string rep = gSudokuGame.GetBoardStringIdentity(soln);
                if (rep != target)
                    return false;
            }
            return true;
        }

        /// <summary>
        ///   Removes values from cells in a semi-solved 9x9 puzzle until
        ///   the puzzle is minimally constrained.
        /// </summary>
        private void Prune(ExactCover ec, gSudokuCell[][] board, Int32 minFilledCells, Int32 maxTries, Boolean useSymmetry)
        {
            String target = gSudokuGame.GetBoardStringIdentity(board);
            //Console.WriteLine("targ: {0}", target);
            List<Int32> remaining = AvailableCells(board);
            int nRemoved = 0;
            Int32 filledCells = 81;
            do
            {
                // randomly select a non-empty cell
                int n = SelectRandom(remaining);
                int c1 = n % 9;
                int r1 = n / 9;

                // in case we want symmetry
                int r2 = 0, c2 = 0;

                Int32 v1 = board[r1][c1].SingleValue;
                //puzzle[r1, c1] = 0;
                board[r1][c1].ClearValues();
                nRemoved++;

                Int32 v2 = 0;
                if (useSymmetry)
                {
                    r2 = 9 - (r1 + 1);
                    c2 = 9 - (c1 + 1);
                    v2 = board[r2][c2].SingleValue;
                    board[r2][c2].ClearValues();
                    nRemoved++;
                    int n2 = r2 * 9 + c2;
                    for (int i = 0; i < remaining.Count; i++)
                    {
                        if (c2 == remaining[i])
                        {
                            remaining.RemoveAt(i);
                            break;
                        }
                    }
                }

                if (nRemoved > 6)
                {
                    if (!UniquelySolvable(ec, target, board, maxTries))
                    {
                        // differing solutions, we've gone too far.
                        // restore the value.
                        //puzzle[r1, c1] = v1;
                        board[r1][c1].ClearValues();
                        if (v1 != 0)
                        {
                            board[r1][c1].AddValue(v1);
                        }
                        if (useSymmetry)
                        {
                            board[r2][c2].ClearValues();
                            if (v2 != 0)
                            {
                                board[r2][c2].AddValue(v2);
                            }
                            //puzzle[r2, c2] = v2;
                        }
                    }
                }

                filledCells = board.Sum(s => s.Count(s2 => s2.ValuesCount == 1));
                if (filledCells <= minFilledCells)
                {
                    break;
                }
            } while (remaining.Count != 0);

        }

        /// <summary>
        /// The approach taken is to start with an empty matrix
        /// (solve the default matrix), then randomly select cells to
        /// erase, insuring at each step that the puzzle is solvable.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="minFilledCells"></param>
        public void Generate(gSudokuCell[][] board, Int32 minFilledCells, Int32 maxTries, Boolean useSymmetry)
        {
            //CreateBaseMatrix();
            ExactCover ec = new ExactCover(_Matrix);
            Stack<int> pstate = new Stack<int>();
            _rnd = new Random();

            if (!ec.Solve(pstate))
                throw new Exception("Unsolvable puzzle from base state. Inconceivable!");

            FillSolution(pstate, board);
            Prune(ec, board, minFilledCells, maxTries, useSymmetry);
        }

    }
}
