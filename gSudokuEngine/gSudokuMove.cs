using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gSudokuEngine
{
    public class SudokuMoveNullCellException : Exception
    {
        public SudokuMoveNullCellException() { }
        public SudokuMoveNullCellException(String message) : base(message) { }
        public SudokuMoveNullCellException(String message, Exception inner) : base(message, inner) { }
    }

    public enum gSudokuMoveType
    {
        Add,
        Remove,
        Clear,
        Inverse,
        FillNote,
        FillNoteClear,
        ClearNote,
        Solve,
        ProtectCell
    }

    public class gSudokuMove
    {
        public gSudokuCell Cell { get { return _Cell; } }
        public gSudokuMoveType MoveType { get { return _MoveType; } }
        public Int32 Value { get { return _Value; } }
        public gSudokuCell CellCopy { get { return _CellCopy; } }
        public Int32 Id { get { return _Id; } }

        private gSudokuCell _Cell = null;
        private gSudokuMoveType _MoveType;
        private Int32 _Value;
        private gSudokuCell _CellCopy = null;
        private Int32 _Id = -1;

        public gSudokuMove(gSudokuCell myCell, gSudokuMoveType myType, Int32 myValue, Int32 myId)
        {
            if (myType != gSudokuMoveType.Solve)
            {
                if (myCell == null)
                {
                    throw new SudokuMoveNullCellException();
                }
                //Check if cell is protected
                if (myCell.IsProtected)
                {
                    throw new SudokuCellValueIsProtectedException();
                }
            }

            if (myType == gSudokuMoveType.Add)
            {
                //Check for valid values
                if (myValue < 1 || myValue > 9)
                {
                    throw new SudokuCellInvalidValueException();
                }
                //Check if value already exists
                if (myCell.ValueExists(myValue))
                {
                    throw new SudokuCellValueAlreadyExistsException();
                }
            }
            else if (myType == gSudokuMoveType.Remove)
            {
                //Check if value exists        
                if (!myCell.ValueExists(myValue))
                {
                    throw new SudokuCellValueDoesNotExistException();
                }
            }

            _Cell = myCell;
            _MoveType = myType;
            _Value = myValue;
            _Id = myId;
            if (myCell != null)
            {
                _CellCopy = myCell.Clone();
            }
        }
    }
}
