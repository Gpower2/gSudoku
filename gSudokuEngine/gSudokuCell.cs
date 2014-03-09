using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gSudokuEngine
{
    #region "Exceptions"

    public class SudokuCellInvalidValueException : Exception
    {
        public SudokuCellInvalidValueException() { }
        public SudokuCellInvalidValueException(String message) : base(message) { }
        public SudokuCellInvalidValueException(String message, Exception inner) : base(message, inner) { }
    }

    public class SudokuCellValueAlreadyExistsException : Exception
    {
        public SudokuCellValueAlreadyExistsException() { }
        public SudokuCellValueAlreadyExistsException(String message) : base(message) { }
        public SudokuCellValueAlreadyExistsException(String message, Exception inner) : base(message, inner) { }
    }

    public class SudokuCellValueDoesNotExistException : Exception
    {
        public SudokuCellValueDoesNotExistException() { }
        public SudokuCellValueDoesNotExistException(String message) : base(message) { }
        public SudokuCellValueDoesNotExistException(String message, Exception inner) : base(message, inner) { }
    }

    public class SudokuCellValueIsProtectedException : Exception
    {
        public SudokuCellValueIsProtectedException() { }
        public SudokuCellValueIsProtectedException(String message) : base(message) { }
        public SudokuCellValueIsProtectedException(String message, Exception inner) : base(message, inner) { }
    }

    #endregion

    public class gSudokuCell
    {
        private List<Int32> _Values = new List<Int32>();

        /// <summary>
        /// Get or sets if cell values are protected
        /// </summary>
        public Boolean IsProtected { get; set; }

        /// <summary>
        /// Gets if a cell contains any values
        /// </summary>
        public Boolean IsEmpty { get { return _Values.Count > 0 ? false : true; } }

        /// <summary>
        /// Gets the number of values that the cell contains
        /// </summary>
        public Int32 ValuesCount { get { return _Values.Count; } }

        /// <summary>
        /// Gets a copy of the list of values for the cell
        /// </summary>
        /// <returns></returns>
        public List<Int32> Values { get { return new List<Int32>(_Values); } }

        /// <summary>
        /// Gets the single value of the cell, 0 if cell contains zero or more values
        /// </summary>
        public Int32 SingleValue { get { return _Values.Count == 1 ? _Values[0] : 0; } }

        /// <summary>
        /// Gets if the cell contains a single value
        /// </summary>
        public Boolean IsSingleValue { get { return (_Values.Count == 1); } }

        /// <summary>
        /// Add a new value to the list
        /// </summary>
        /// <param name="newValue"></param>
        public void AddValue(Int32 newValue)
        {
            //Check if cell is protected
            if (IsProtected)
            {
                throw new SudokuCellValueIsProtectedException();
            }
            //Check for valid values
            if (newValue < 1 || newValue > 9)
            {
                throw new SudokuCellInvalidValueException();
            }
            //Check if value already exists
            if (_Values.Contains(newValue))
            {
                throw new SudokuCellValueAlreadyExistsException();
            }
            _Values.Add(newValue);
        }

        /// <summary>
        /// Add a range of values to the list
        /// </summary>
        /// <param name="newValues"></param>
        public void AddValues(List<Int32> newValues)
        {
            //Check if cell is protected
            if (IsProtected)
            {
                throw new SudokuCellValueIsProtectedException();
            }
            foreach (Int32 val in newValues)
            {
                //Check for valid values
                if (val < 1 || val > 9)
                {
                    throw new SudokuCellInvalidValueException();
                }
                //Check if value already exists
                if (_Values.Contains(val))
                {
                    throw new SudokuCellValueAlreadyExistsException();
                }
                _Values.Add(val);
            }
        }

        /// <summary>
        /// Remove a value from the list
        /// </summary>
        /// <param name="cellValue"></param>
        public void RemoveValue(Int32 cellValue)
        {
            //Check if cell is protected
            if (IsProtected)
            {
                throw new SudokuCellValueIsProtectedException();
            }
            //Check if value exists        
            if (!_Values.Contains(cellValue))
            {
                throw new SudokuCellValueDoesNotExistException();
            }
            _Values.Remove(cellValue);
        }

        /// <summary>
        /// Clears all the values
        /// </summary>
        public void ClearValues()
        {
            //Check if cell is protected
            if (IsProtected)
            {
                throw new SudokuCellValueIsProtectedException();
            }
            _Values.Clear();
        }

        /// <summary>
        /// Inverts all the values
        /// </summary>
        public void InverseValues()
        {
            for (Int32 i = 1; i <= 9; i++)
            {
                if (_Values.Contains(i))
                {
                    _Values.Remove(i);
                }
                else
                {
                    _Values.Add(i);
                }
            }
        }

        /// <summary>
        /// Check if a value already exists in the list
        /// </summary>
        /// <param name="cellValue"></param>
        /// <returns></returns>
        public Boolean ValueExists(Int32 cellValue)
        {
            return _Values.Contains(cellValue);
        }

        /// <summary>
        /// Returns a copy of the cell
        /// </summary>
        /// <returns></returns>
        public gSudokuCell Clone()
        {
            gSudokuCell myCell = new gSudokuCell();
            myCell._Values = new List<Int32>(_Values);
            myCell.IsProtected = IsProtected;
            return myCell;
        }
    }
}
