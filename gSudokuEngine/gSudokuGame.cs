using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace gSudokuEngine
{
    #region "Exceptions"
    public class SudokuEngineNoMovesToUndoException : Exception
    {
        public SudokuEngineNoMovesToUndoException() { }
        public SudokuEngineNoMovesToUndoException(String message) : base(message) { }
        public SudokuEngineNoMovesToUndoException(String message, Exception inner) : base(message, inner) { }
    }

    public class SudokuEngineNoMovesToRedoException : Exception
    {
        public SudokuEngineNoMovesToRedoException() { }
        public SudokuEngineNoMovesToRedoException(String message) : base(message) { }
        public SudokuEngineNoMovesToRedoException(String message, Exception inner) : base(message, inner) { }
    }

    public class SudokuEngineBoardNotInitializedException : Exception
    {
        public SudokuEngineBoardNotInitializedException() { }
        public SudokuEngineBoardNotInitializedException(String message) : base(message) { }
        public SudokuEngineBoardNotInitializedException(String message, Exception inner) : base(message, inner) { }
    }

    public class SudokuEngineInvalidGameStatusException : Exception
    {
        public SudokuEngineInvalidGameStatusException() { }
        public SudokuEngineInvalidGameStatusException(String message) : base(message) { }
        public SudokuEngineInvalidGameStatusException(String message, Exception inner) : base(message, inner) { }
    }

    #endregion

    public enum gSudokuGameStatus
    {
        Playing,
        Paused,
        Finished,
        NotStarted
    }

    public class gSudokuGame
    {
        private gSudokuCell[][] _Board = null;
        private List<gSudokuMove> _PlayerMoves = new List<gSudokuMove>();
        private Int32 _CurrentMoveIndex = -1;
        private Int32 _PlayerActionsCounter = 0;
        private gSudokuGameStatus _GameStatus = gSudokuGameStatus.NotStarted;
        private gSudokuDifficulty _GameDifficulty = new gSudokuDifficulty("Undefined", 0, 0, 0, false);

        /// <summary>
        /// Gets the current board
        /// </summary>
        public gSudokuCell[][] Board { get { return _Board; } }

        /// <summary>
        /// Gets the list with the player moves
        /// </summary>
        public List<gSudokuMove> PlayerMoves { get { return _PlayerMoves; } }

        /// <summary>
        /// Gets the current move index
        /// </summary>
        public Int32 CurrentMoveIndex { get { return _CurrentMoveIndex; } }

        /// <summary>
        /// Gets the actual player actions
        /// </summary>
        public Int32 PlayerActionsCounter { get { return _PlayerActionsCounter; } }

        /// <summary>
        /// Gets the current game status
        /// </summary>
        public gSudokuGameStatus GameStatus { get { return _GameStatus; } }

        /// <summary>
        /// Gets the current game difficulty
        /// </summary>
        public gSudokuDifficulty GameDifficulty { get { return _GameDifficulty; } }

        /// <summary>
        /// Returns a newly created empty board
        /// </summary>
        /// <returns></returns>
        public static gSudokuCell[][] GetCleanBoard()
        {
            gSudokuCell[][] board = new gSudokuCell[9][];
            for (Int32 i = 0; i < 9; i++)
            {
                board[i] = new gSudokuCell[9];
                for (Int32 j = 0; j < 9; j++)
                {
                    board[i][j] = new gSudokuCell();
                }
            }
            return board;
        }

        /// <summary>
        /// Returns the String Identity of a board
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static String GetBoardStringIdentity(gSudokuCell[][] board)
        {
            StringBuilder boardBuilder = new StringBuilder();
            for (Int32 i = 0; i < 9; i++)  // rows
            {
                for (Int32 j = 0; j < 9; j++) // cols
                {
                    Int32 value = board[i][j].SingleValue;
                    //boardBuilder.Append((char)(value + 48));
                    boardBuilder.Append(value.ToString());
                }
            }
            return boardBuilder.ToString();
        }

        /// <summary>
        /// Returns an exact copy of a board
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static gSudokuCell[][] CloneBoard(gSudokuCell[][] board)
        {
            gSudokuCell[][] myBoard = new gSudokuCell[9][];
            for (Int32 i = 0; i < 9; i++)
            {
                myBoard[i] = new gSudokuCell[9];
                for (Int32 j = 0; j < 9; j++)
                {
                    myBoard[i][j] = board[i][j].Clone();
                }
            }
            return myBoard;
        }

        public gSudokuGame()
        {
            InitBoard();
        }

        private void InitBoard()
        {
            _Board = new gSudokuCell[9][];
            for (Int32 i = 0; i < 9; i++)
            {
                _Board[i] = new gSudokuCell[9];
                for (Int32 j = 0; j < 9; j++)
                {
                    _Board[i][j] = new gSudokuCell();
                }
            }
        }

        /// <summary>
        /// Clears the board with new empty and unprotected sudoku cells
        /// It also clears all the player moves and actions
        /// </summary>
        private void ClearBoard()
        {
            if (_Board == null)
            {
                throw new SudokuEngineBoardNotInitializedException();
            }
            for (Int32 i = 0; i < 9; i++)
            {
                if (_Board[i] == null)
                {
                    throw new SudokuEngineBoardNotInitializedException();
                }
                for (Int32 j = 0; j < 9; j++)
                {
                    _Board[i][j] = new gSudokuCell();
                }
            }
            _PlayerMoves.Clear();
            _CurrentMoveIndex = -1;
            _PlayerActionsCounter = 0;
            
        }

        /// <summary>
        /// Creates a new board
        /// </summary>
        public void CreateNewBoard(gSudokuDifficulty difficulty)
        {
            ClearBoard();
            _GameDifficulty = difficulty;
            gSudokuHelper gHelper = new gSudokuHelper();
            Int32 clues = difficulty.Clues;
            Int32 attempts = 1;
            Debug.WriteLine("Clues requested: " + clues);
            //Keep trying to find a board with the clues requested
            while (Math.Abs(GetNumberOfUserFilledCells() - clues) > 2 && attempts < 6)
            {
                ClearBoard();
                gHelper.Generate(_Board, clues, difficulty.UniqueSolutionAttempts, difficulty.UseSymmetry);
                Debug.WriteLine("GetNumberOfUserFilledCells: " + GetNumberOfUserFilledCells());
                attempts++;
            }
            for (Int32 i = 0; i < 9; i++)
            {
                for (Int32 j = 0; j < 9; j++)
                {
                    if (!_Board[i][j].IsEmpty)
                    {
                        _Board[i][j].IsProtected = true;
                    }
                }
            }
            _GameStatus = gSudokuGameStatus.Playing;
        }

        /// <summary>
        /// Creates a new clean board
        /// </summary>
        public void CreateNewCleanBoard()
        {
            ClearBoard();
            _GameDifficulty = new gSudokuEngine.gSudokuDifficulty("Custom", 0, 0, 0, false); 
            _GameStatus = gSudokuGameStatus.Playing;
        }


        /// <summary>
        /// Check if a move is valid
        /// </summary>
        /// <returns></returns>
        public Boolean CheckMoveValidity(Int32 x, Int32 y, Int32 value)
        {            
            for (Int32 i = 0; i < 9; i++)
            {
                //Check the row
                if (_Board[x][i].ValueExists(value) && _Board[x][i].ValuesCount == 1 && i != y)
                {
                    return false;
                }
                //Check the column
                if (_Board[i][y].ValueExists(value) && _Board[i][y].ValuesCount == 1 && i != x)
                {
                    return false;
                }
            }
            Int32 startX = 0, endX = 0;
            //Find the box
            if (x >= 0 && x <= 2)
            {
                startX = 0;
                endX = 2;
            }
            else if (x >= 3 && x <= 5)
            {
                startX = 3;
                endX = 5;
            }
            else if (x >= 6 && x <= 8)
            {
                startX = 6;
                endX = 8;
            }
            Int32 startY = 0, endY = 0;
            if (y >= 0 && y <= 2)
            {
                startY = 0;
                endY = 2;
            }
            else if (y >= 3 && y <= 5)
            {
                startY = 3;
                endY = 5;
            }
            else if (y >= 6 && y <= 8)
            {
                startY = 6;
                endY = 8;
            }
            //Check the box
            for (Int32 i = startX; i <= endX; i++)
            {
                for (Int32 j = startY; j <= endY; j++)
                {
                    if (_Board[i][j].ValueExists(value) && _Board[i][j].ValuesCount == 1 && i != x && j != y)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Check if a game is finished
        /// </summary>
        /// <returns></returns>
        public Boolean IsGameFinished()
        {
            for (Int32 i = 0; i < 9; i++)
            {
                for (Int32 j = 0; j < 9; j++)
                {
                    if (_Board[i][j].ValuesCount != 1)
                    {
                        return false;
                    }                    
                    if (!CheckMoveValidity(i, j, _Board[i][j].SingleValue))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Returns a new id for a new move
        /// </summary>
        /// <returns></returns>
        public Int32 GetNewMoveId()
        {
            return _PlayerActionsCounter;
        }

        /// <summary>
        /// Plays a move and adds it to the list
        /// </summary>
        /// <param name="myMove"></param>
        public void PlayMove(gSudokuMove myMove, Boolean increaseActionCounter)
        {
            //Check game status
            if (_GameStatus != gSudokuGameStatus.Playing)
            {
                throw new SudokuEngineInvalidGameStatusException();
            }

            //Check if current move is not the last one
            if (_CurrentMoveIndex < _PlayerMoves.Count - 1)
            {
                //Remove the last moves
                while (_PlayerMoves.Count > _CurrentMoveIndex + 1)
                {
                    _PlayerMoves.RemoveAt(_PlayerMoves.Count - 1);
                }                
                //_PlayerMoves.RemoveRange(_CurrentMoveIndex + 1, _PlayerMoves.Count - _CurrentMoveIndex);
            }

            //Actually play the move
            if (myMove.MoveType == gSudokuMoveType.Add || myMove.MoveType == gSudokuMoveType.FillNote)
            {
                if (!myMove.Cell.ValueExists(myMove.Value))
                {
                    myMove.Cell.AddValue(myMove.Value);
                }
            }
            else if (myMove.MoveType == gSudokuMoveType.Remove)
            {
                myMove.Cell.RemoveValue(myMove.Value);
            }
            else if (myMove.MoveType == gSudokuMoveType.Clear || myMove.MoveType == gSudokuMoveType.ClearNote || myMove.MoveType == gSudokuMoveType.FillNoteClear)
            {
                myMove.Cell.ClearValues();
            }
            else if (myMove.MoveType == gSudokuMoveType.Inverse)
            {
                myMove.Cell.InverseValues();
            }
            else if (myMove.MoveType == gSudokuMoveType.Solve)
            {
                gSudokuHelper gHelper = new gSudokuHelper();
                gHelper.Solve(_Board);
            }
            else if (myMove.MoveType == gSudokuMoveType.ProtectCell)
            {
                myMove.Cell.IsProtected = true;
            }

            //Add the new move to the moves list
            _PlayerMoves.Add(myMove);
            _CurrentMoveIndex = _PlayerMoves.Count - 1;
            
            if (increaseActionCounter)
            {
                _PlayerActionsCounter++;
            }

            //Check if game is finished
            if (IsGameFinished())
            {
                _GameStatus = gSudokuGameStatus.Finished;
            }
        }

        /// <summary>
        /// Undo a move from the list
        /// </summary>
        public void UndoMove(Boolean increaseActionCounter)
        {
            //Check game status
            if (_GameStatus != gSudokuGameStatus.Playing)
            {
                throw new SudokuEngineInvalidGameStatusException();
            }

            //Check if there are moves
            if (_PlayerMoves.Count == 0 || _CurrentMoveIndex == -1)
            {
                throw new SudokuEngineNoMovesToUndoException();
            }

            //Get the last current move
            gSudokuMove myMove = _PlayerMoves[_CurrentMoveIndex];

            //Undo the move
            if (myMove.MoveType == gSudokuMoveType.Add)
            {
                myMove.Cell.RemoveValue(myMove.Value);
            }
            else if (myMove.MoveType == gSudokuMoveType.Remove)
            {
                myMove.Cell.AddValue(myMove.Value);
            }
            else if (myMove.MoveType == gSudokuMoveType.Clear)
            {
                myMove.Cell.ClearValues();
                myMove.Cell.AddValues(myMove.CellCopy.Values);
            }
            else if (myMove.MoveType == gSudokuMoveType.Inverse)
            {
                myMove.Cell.InverseValues();
            }
            else if (myMove.MoveType == gSudokuMoveType.FillNote || myMove.MoveType == gSudokuMoveType.FillNoteClear)
            {
                if (myMove.MoveType == gSudokuMoveType.FillNote)
                {
                    if (myMove.Cell.ValueExists(myMove.Value))
                    {
                        myMove.Cell.RemoveValue(myMove.Value);
                    }
                }
                else if (myMove.MoveType == gSudokuMoveType.FillNoteClear)
                {
                    myMove.Cell.ClearValues();
                    myMove.Cell.AddValues(myMove.CellCopy.Values);
                }
                while (_PlayerMoves[_CurrentMoveIndex - 1].Id == myMove.Id)
                {
                    if (_PlayerMoves[_CurrentMoveIndex - 1].MoveType == gSudokuMoveType.FillNote)
                    {
                        _CurrentMoveIndex -= 1;
                        if (_PlayerMoves[_CurrentMoveIndex].Cell.ValueExists(_PlayerMoves[_CurrentMoveIndex].Value))
                        {
                            _PlayerMoves[_CurrentMoveIndex].Cell.RemoveValue(_PlayerMoves[_CurrentMoveIndex].Value);
                        }
                    }
                    else if (_PlayerMoves[_CurrentMoveIndex - 1].MoveType == gSudokuMoveType.FillNoteClear)
                    {
                        _CurrentMoveIndex -= 1;
                        _PlayerMoves[_CurrentMoveIndex].Cell.ClearValues();
                        _PlayerMoves[_CurrentMoveIndex].Cell.AddValues(_PlayerMoves[_CurrentMoveIndex].CellCopy.Values);
                    }
                    if (_CurrentMoveIndex - 1 < 0)
                    {
                        break;
                    }
                }            
            }
            else if (myMove.MoveType == gSudokuMoveType.ClearNote)
            {
                myMove.Cell.ClearValues();
                myMove.Cell.AddValues(myMove.CellCopy.Values);
                while (_PlayerMoves[_CurrentMoveIndex - 1].Id == myMove.Id)
                {
                    _CurrentMoveIndex -= 1;
                    _PlayerMoves[_CurrentMoveIndex].Cell.ClearValues();
                    _PlayerMoves[_CurrentMoveIndex].Cell.AddValues(_PlayerMoves[_CurrentMoveIndex].CellCopy.Values);
                    if (_CurrentMoveIndex - 1 < 0)
                    {
                        break;
                    }
                }                
            }
            else if (myMove.MoveType == gSudokuMoveType.ProtectCell)
            {
                myMove.Cell.IsProtected = false;
                while (_PlayerMoves[_CurrentMoveIndex - 1].Id == myMove.Id)
                {
                    _CurrentMoveIndex -= 1;
                    _PlayerMoves[_CurrentMoveIndex].Cell.IsProtected = false;
                    if (_CurrentMoveIndex - 1 < 0)
                    {
                        break;
                    }
                }
            }

            //Go back a move
            _CurrentMoveIndex -= 1;

            if (increaseActionCounter)
            {
                _PlayerActionsCounter++;
            }
        }

        /// <summary>
        /// Redo the first move that has been undone
        /// </summary>
        public void RedoMove(Boolean increaseActionCounter)
        {
            //Check game status
            if (_GameStatus != gSudokuGameStatus.Playing)
            {
                throw new SudokuEngineInvalidGameStatusException();
            }

            //Check if there are moves
            if (_PlayerMoves.Count == 0)
            {
                throw new SudokuEngineNoMovesToRedoException();
            }
            //Check if we are at the last move already
            if (_CurrentMoveIndex == _PlayerMoves.Count - 1)
            {
                throw new SudokuEngineNoMovesToRedoException();
            }

            //Get the move to redo
            _CurrentMoveIndex += 1;
            gSudokuMove myMove = _PlayerMoves[_CurrentMoveIndex];

            //Actually redo the move
            if (myMove.MoveType == gSudokuMoveType.Add)
            {
                myMove.Cell.AddValue(myMove.Value);
            }
            else if (myMove.MoveType == gSudokuMoveType.Remove)
            {
                myMove.Cell.RemoveValue(myMove.Value);
            }
            else if (myMove.MoveType == gSudokuMoveType.Clear)
            {
                myMove.Cell.ClearValues();
            }
            else if (myMove.MoveType == gSudokuMoveType.Inverse)
            {
                myMove.Cell.InverseValues();
            }
            else if (myMove.MoveType == gSudokuMoveType.FillNote || myMove.MoveType == gSudokuMoveType.FillNoteClear)
            {
                if (myMove.MoveType == gSudokuMoveType.FillNote)
                {
                    if (!myMove.Cell.ValueExists(myMove.Value))
                    {
                        myMove.Cell.AddValue(myMove.Value);
                    }
                }
                else if (myMove.MoveType == gSudokuMoveType.FillNoteClear)
                {
                    myMove.Cell.ClearValues();
                }
                while (_PlayerMoves[_CurrentMoveIndex + 1].Id == myMove.Id)
                {
                    if (_PlayerMoves[_CurrentMoveIndex + 1].MoveType == gSudokuMoveType.FillNote)
                    {
                        _CurrentMoveIndex += 1;
                        if (!_PlayerMoves[_CurrentMoveIndex].Cell.ValueExists(_PlayerMoves[_CurrentMoveIndex].Value))
                        {
                            _PlayerMoves[_CurrentMoveIndex].Cell.AddValue(_PlayerMoves[_CurrentMoveIndex].Value);
                        }
                    }
                    else if (_PlayerMoves[_CurrentMoveIndex + 1].MoveType == gSudokuMoveType.FillNoteClear)
                    {
                        _CurrentMoveIndex += 1;
                        _PlayerMoves[_CurrentMoveIndex].Cell.ClearValues();
                    }
                    if (_CurrentMoveIndex + 1 > _PlayerMoves.Count - 1)
                    {
                        break;
                    }
                }
            }
            else if (myMove.MoveType == gSudokuMoveType.ClearNote)
            {
                myMove.Cell.ClearValues();
                while (_PlayerMoves[_CurrentMoveIndex + 1].Id == myMove.Id)
                {
                    _CurrentMoveIndex += 1;
                    _PlayerMoves[_CurrentMoveIndex].Cell.ClearValues();
                    if (_CurrentMoveIndex + 1 > _PlayerMoves.Count - 1)
                    {
                        break;
                    }
                }
            }
            else if (myMove.MoveType == gSudokuMoveType.ProtectCell)
            {
                myMove.Cell.IsProtected = true;
                while (_PlayerMoves[_CurrentMoveIndex + 1].Id == myMove.Id)
                {
                    _CurrentMoveIndex += 1;
                    _PlayerMoves[_CurrentMoveIndex].Cell.IsProtected = true;
                    if (_CurrentMoveIndex + 1 > _PlayerMoves.Count - 1)
                    {
                        break;
                    }
                }
            }

            if (increaseActionCounter)
            {
                _PlayerActionsCounter++;
            }
        }

        public Int32 GetNumberOfProtectedCells()
        {
            return _Board.Sum(s => s.Count(s2 => s2.IsProtected));
        }

        public Int32 GetNumberOfSingleValueCells(Int32 value)
        {
            return _Board.Sum(s => s.Count(s2 => s2.ValueExists(value) && s2.ValuesCount == 1));
        }

        public Int32 GetNumberOfUserFilledCells()
        {
            return _Board.Sum(s => s.Count(s2 => !s2.IsProtected && !s2.IsEmpty));
        }

        public Int32 GetNumberOfEmptyCells()
        {
            return _Board.Sum(s => s.Count(s2 => s2.IsEmpty));
        }

        public void FillNotes()
        {
            Int32 newMoveId = GetNewMoveId();
            for (Int32 i = 0; i < 9; i++)
            {
                for (Int32 j = 0; j < 9; j++)
                {
                    if (!_Board[i][j].IsProtected)
                    {
                        if (_Board[i][j].IsEmpty || _Board[i][j].ValuesCount > 1)
                        {
                            //_Board[i][j].ClearValues();
                            gSudokuMove myMove = new gSudokuMove(_Board[i][j], gSudokuMoveType.FillNoteClear, -1, newMoveId);
                            PlayMove(myMove, false);
                            for (Int32 w = 1; w <= 9; w++)
                            {
                                if (CheckMoveValidity(i, j, w))
                                {
                                    gSudokuMove myFillMove = new gSudokuMove(_Board[i][j], gSudokuMoveType.FillNote, w, newMoveId);
                                    PlayMove(myFillMove, false);
                                    //_Board[i][j].AddValue(w);
                                }
                            }
                        }
                    }
                }
            }
            //Manually increase action counter
            _PlayerActionsCounter++;
        }

        public void ClearNotes()
        {
            Int32 newMoveId = GetNewMoveId();
            for (Int32 i = 0; i < 9; i++)
            {
                for (Int32 j = 0; j < 9; j++)
                {
                    if (!_Board[i][j].IsProtected)
                    {
                        if (_Board[i][j].ValuesCount > 1)
                        {
                            gSudokuMove myMove = new gSudokuMove(_Board[i][j], gSudokuMoveType.ClearNote, -1, newMoveId);
                            PlayMove(myMove, false);
                            //_Board[i][j].ClearValues();
                        }
                    }
                }
            }
            //Manually increase action counter
            _PlayerActionsCounter++;
        }

        /// <summary>
        /// Attempts to solve the existing game
        /// </summary>
        public void Solve()
        {
            gSudokuMove myMove = new gSudokuMove(null, gSudokuMoveType.Solve, -1, GetNewMoveId());
            PlayMove(myMove, true);
        }

        /// <summary>
        /// Checks if the game has a unique solution
        /// </summary>
        /// <param name="maxTries"></param>
        /// <returns></returns>
        public Boolean CheckUniqueSolution(Int32 maxTries )
        {
            gSudokuHelper gHelper = new gSudokuHelper();
            _PlayerActionsCounter++;
            return gHelper.UniquelySolvable(_Board, maxTries);
        }

        public void ProtectAllSingleValueCells()
        {
            Int32 myId = GetNewMoveId();
            for (Int32 i = 0; i < 9; i++)
            {
                for (Int32 j = 0; j < 9; j++)
                {
                    if (_Board[i][j].IsSingleValue)
                    {
                        gSudokuMove myMove = new gSudokuMove(_Board[i][j], gSudokuMoveType.ProtectCell, -1, myId);
                        PlayMove(myMove, false);
                        //_Board[i][j].IsProtected = true;
                    }
                }
            }
            //Manually increase action counter
            _PlayerActionsCounter++;
        }

        public void Pause()
        {
            if (_GameStatus == gSudokuGameStatus.Playing)
            {
                _GameStatus = gSudokuGameStatus.Paused;
            }
            else
            {
                throw new SudokuEngineInvalidGameStatusException();
            }
        }

        public void Continue()
        {
            if (_GameStatus == gSudokuGameStatus.Paused)
            {
                _GameStatus = gSudokuGameStatus.Playing;
            }
            else
            {
                throw new SudokuEngineInvalidGameStatusException();
            }
        }
    }
}
