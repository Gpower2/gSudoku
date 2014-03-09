using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gSudokuEngine
{
    /// <summary>
    ///   Provides a class that solves the Exact Cover problem with
    ///   "Algorithm-X", augmented with Knuth's "Dancing Links" shortcut.
    ///   Can be used as the basis for a Sudoku Solver, but can also
    ///   be used to solve any exact cover problem.
    /// </summary>
    public class ExactCover
    {
        internal class LinkedNode
        {
            public LinkedNode Above { get; set; }
            public LinkedNode Below { get; set; }
            public LinkedNode Left { get; set; }
            public LinkedNode Right { get; set; }

            public LinkedNode Head { get; set; }
            public String Id { get; set; }
            public int ColumnCount;     // used only for head
            public int r { get; set; } // for tracking the solution

            // safety check
            public bool Removed;

            public override String ToString()
            {
                return Id;
            }
        }

        private LinkedNode _root;
        private LinkedNode[] _rightMost; // for each row
        private int _RowCount;                  // row count
        private int _ColCount;                  // col count
        private Stack<int> _MatrixState;      // state of matrix - which rows have been eliminated
        private System.Random _rnd;
        private int _seed;

        public ExactCover()
        {
            _rnd = new System.Random();
            _seed = _rnd.Next();
            _rnd = new System.Random(_seed);
        }

        public ExactCover(bool[,] matrix)
            : this()
        {
            Data = matrix;
            _RowCount = Data.GetLength(0);
            _ColCount = Data.GetLength(1);
        }

        /// <summary>
        ///   Build the doubly-linked toroidal lists.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     Given the matrix, this builds the linked list that models it.  It
        ///     constructs all the column headers, then a node for each
        ///     element in the column that is filled. It links each
        ///     element to its North, South, East, and West neighbors.
        ///   </para>
        ///   <para>
        ///     There are only N + H + 1 nodes in the mesh, where N is
        ///     the number of 1's in the matrix, and H is the number of
        ///     distinct columns in which those 1's appear. It is not
        ///     necessary to create a LinkedNode for empty places in the
        ///     matrix.  This is especially important for sparse
        ///     matrices, such as those modelling Sudoku or other
        ///     problems.
        ///   </para>
        /// </remarks>
        private LinkedNode BuildLinks()
        {
            _root = new LinkedNode();
            _root.Id = "root";
            LinkedNode leftHead = _root;
            LinkedNode northernNeighbor, head = null, c;
            _rightMost = new LinkedNode[_RowCount];             // in each row
            LinkedNode[] leftMost = new LinkedNode[_RowCount]; // in each row

            for (int j = 0; j < _ColCount; j++)     // columns
            {
                head = new LinkedNode();
                head.Id = ":" + j + ":";  // diagnostic purposes
                head.Below = head;
                head.Above = head;
                head.Left = leftHead;

                leftHead.Right = head;
                northernNeighbor = head;

                for (int i = 0; i < _RowCount; i++) // rows
                {
                    if (Data[i, j])
                    {
                        c = new LinkedNode();
                        c.Id = String.Format("[{0},{1}]", i, j);
                        c.Head = head;
                        c.r = i;
                        c.Above = northernNeighbor;
                        northernNeighbor.Below = c;
                        head.ColumnCount++;
                        if (leftMost[i] == null)
                        {
                            leftMost[i] = c;
                        }
                        if (_rightMost[i] != null)
                        {
                            _rightMost[i].Right = c;
                            c.Left = _rightMost[i];
                        }

                        // for the next cycle
                        _rightMost[i] = c;
                        northernNeighbor = c;
                    }
                }
                head.Above = northernNeighbor;
                northernNeighbor.Below = head;
                leftHead = head;
            }

            // close the loop on each row
            for (int i = 0; i < _RowCount; i++)
            {
                if (leftMost[i] != null)
                    leftMost[i].Left = _rightMost[i];

                if (_rightMost[i] != null)
                    _rightMost[i].Right = leftMost[i];
            }

            // close the loop on the column heads
            head.Right = _root;
            _root.Left = head;

            return _root;
        }

        private void RemoveColumn(LinkedNode colNode)
        {
            // Guard against removing the same column twice.
            //
            // If we try to remove the same column twice, the pointers
            // get corrupted. Must not do this!
            //
            // Normally, there's no need to check to see if a column has
            // already been removed. If the algorithm is correctly
            // implemented, it will never try to remove a column twice. BUT !
            // Supposing an incorrect Sudoku puzzle, which is not solvable
            // (for example, there are two 9's in a single row), then
            // it can result in the same column being removed twice, for
            // the "givens".
            //
            // So this safety flag protects against bad input.
            //
            if (colNode.Removed)
                return;

            // remove the column head
            colNode.Right.Left = colNode.Left;
            colNode.Left.Right = colNode.Right;

            // fixup all nodes in the matrix from this column
            for (LinkedNode r1 = colNode.Below; r1 != colNode; r1 = r1.Below)
            {
                for (LinkedNode n = r1.Right; n != r1; n = n.Right)
                {
                    n.Above.Below = n.Below;
                    n.Below.Above = n.Above;
                }
            }
            colNode.Removed = true;
        }

        private void ReinsertColumn(LinkedNode colNode)
        {
            // reinsert in reverse order from removal
            for (LinkedNode r1 = colNode.Above; r1 != colNode; r1 = r1.Above)
            {
                for (LinkedNode n = r1.Left; n != r1; n = n.Left)
                {
                    n.Above.Below = n;
                    n.Below.Above = n;
                }
            }

            colNode.Right.Left = colNode;
            colNode.Left.Right = colNode;
            colNode.Removed = false;
        }

        private List<LinkedNode> ActiveRowsInColumn(LinkedNode colHead)
        {
            List<LinkedNode> rows = new List<LinkedNode>();
            for (LinkedNode n1 = colHead.Below; n1 != colHead; n1 = n1.Below)
            {
                rows.Add(n1);
            }
            return rows;
        }

        private int GetCellCountForColumn(LinkedNode colHead)
        {
            return ActiveRowsInColumn(colHead).Count;
        }

        /// <summary>
        ///   Choose one column to eliminate from the matrix
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     The heuristic of using a column with the fewest 1's
        ///     tends to make the search run more quickly.
        ///   </para>
        /// </remarks>
        private LinkedNode ChooseColumn()
        {
            int lowestCount = Data.GetLength(0) * Data.GetLength(1);
            var lowList = new List<LinkedNode>();

            // pass 1: find the lowest count
            for (LinkedNode n = _root.Right; n != _root; n = n.Right)
            {
                // Cannot rely on the ColumnCount - some nodes may have
                // been removed from this column already.
                int cc = GetCellCountForColumn(n);
                if (cc > 0 && cc < lowestCount)
                {
                    lowList.Clear();
                    lowestCount = cc;
                    lowList.Add(n);
                }
                else if (cc == lowestCount)
                    lowList.Add(n);
            }

            // Console.WriteLine("Found {0} low items, c({1})",
            //                   lowList.Count, lowestCount);

            // // randomly select one of those :
            // // This NEVER converges! ????  why?
            // int ix = rnd.Next(lowList.Count);
            // return lowList[ix];

            // // this converges:
            // return lowList[0];

            // this converges:
            if (lowList.Count > 2)
                return lowList[_rnd.Next(3)];
            if (lowList.Count > 1)
                return lowList[_rnd.Next(2)];
            if (lowList.Count == 1)
                return lowList[0];
            return null;
        }



        /// <summary>
        ///   Actually perform the search for the exact cover.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     It does its work recursively, choosing a column and covering it, then
        ///     randomly choosing a row to "try" when searching for the next candidate.
        ///   </para>
        ///   <para>
        ///     The solution state is maintained in local var mstate, a
        ///     stack. When the matrix is solved, the mstate lists the
        ///     rows that provide exact cover.
        ///   </para>
        /// </remarks>
        private bool Search(int depth)
        {
            if (_root.Left == _root && _root.Right == _root)
            {
                // the matrix is empty - problem solved
                return true;
            }

            // choose column deterministically (heuristically)
            LinkedNode candidate = ChooseColumn();
            if (candidate == null) return false;
            RemoveColumn(candidate);

            var rows = ActiveRowsInColumn(candidate);
            // choose rows non-deterministically
            foreach (LinkedNode n in rows.OrderRandomly())
            {
                // try removing the chosen row.
                _MatrixState.Push(n.r);
                for (LinkedNode r1 = n.Right; r1 != n; r1 = r1.Right)
                    RemoveColumn(r1.Head);

                if (Search(depth + 1))
                    return true;

                // put the pointers back, in reverse order
                for (LinkedNode r1 = n.Left; r1 != n; r1 = r1.Left)
                    ReinsertColumn(r1.Head);

                _MatrixState.Pop();
            }

            ReinsertColumn(candidate);
            return false;
        }



        /// <summary>
        ///   Solve with no initial state.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     This is useful when the matrix provided in Data is the
        ///     actual matrix.
        ///   </para>
        /// </remarks>
        public bool Solve()
        {
            return Solve(null);
        }


        /// <summary>
        ///   Solve the matrix.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     You must have first set the Data property.
        ///   </para>
        ///   <para>
        ///     The startingState lists rows in the provided constraint
        ///     matrix that should be removed - they are "given" as
        ///     part of the solution.
        ///   </para>
        /// </remarks>
        public bool Solve(Stack<int> startingState)
        {
            if (Data == null)
                throw new ArgumentException("Data");

            BuildLinks();

            if (startingState != null)
            {
                _MatrixState = startingState;
                // remove (cover) the columns which have 1's in the row
                // corresponding to the starting state of the matrix.
                foreach (int v in startingState)
                {
                    var n = _rightMost[v];
                    LinkedNode r1 = n;
                    do
                    {
                        r1 = r1.Right;
                        RemoveColumn(r1.Head);
                    } while (r1 != n);
                }
            }
            else _MatrixState = new Stack<int>();

            return Search(0);
        }

        /// <summary>
        ///   Set this to the matrix to be solved for Exact Cover
        /// </summary>
        public bool[,] Data
        {
            get;
            set;
        }

        /// <summary>
        ///   gets or sets the seed for the random number generator.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     Algorithm-X employed by this class uses random selection
        ///     to search for solutions. This seed governs the randomness.
        ///   </para>
        /// </remarks>
        public int Seed
        {
            set
            {
                _seed = value;
                _rnd = new System.Random(_seed);
            }
            get
            {
                return _seed;
            }
        }

        public Stack<int> MatrixState
        {
            get
            {
                return _MatrixState;
            }
        }
    }

    public static class Extensions
    {
        public static IEnumerable<T> OrderRandomly<T>(this IList<T> list)
        {
            Random random = new Random();
            while (list.Count > 0)
            {
                int index = random.Next(list.Count);
                yield return list[index];
                list.RemoveAt(index);
            }
        }
    }

}
