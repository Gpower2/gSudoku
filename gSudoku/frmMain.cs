using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using gSudokuEngine;
using System.Reflection;

namespace gSudoku
{
    public partial class frmMain : Form
    {
        private gSudokuGame _game = new gSudokuGame();
        private gSudokuGraphicsValues _grapValues = new gSudokuGraphicsValues();
        private gSudokuGraphics _grap = null;
        private gSudokuCell _selectedCell = null;
        private Int32 _selectedX = -1;
        private Int32 _selectedY = -1;
        private Stopwatch _gameTimer = new Stopwatch();
        private BackgroundWorker _bgWorker = new BackgroundWorker();

        private ToolTip _myTooltip = new ToolTip();

        public frmMain()
        {
            try
            {
                InitializeComponent();
                InitDifficulty();
                Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
                Text = "gSudoku v" + Assembly.GetExecutingAssembly().GetName().Version + " -- By Gpower2";
                _grap = new gSudokuGraphics(_grapValues);
                picSudokuBoard.Image = _grap.DrawBoard(_game, picSudokuBoard.Size, _selectedCell);
                UpdateStatistics();
                _bgWorker.WorkerReportsProgress = true;
                _bgWorker.DoWork += new DoWorkEventHandler(OnBgWorkerDoWork);
                _bgWorker.ProgressChanged += new ProgressChangedEventHandler(OnBgWorkerProgressChanged);

                _myTooltip.SetToolTip(btnNewGame, "Starts a new Sudoku Game with the selected difficulty");
                _myTooltip.SetToolTip(btnCleanGame, "Starts a new clean Sudoku, in order to fill it with user defined values and solve it");
                _myTooltip.SetToolTip(btnCheckUniqueSolution, "Checks if the current Sudoku board has a unique solution, or a solution at all");
                _myTooltip.SetToolTip(btnClearCell, "Clears all the values from the selected cell");
                _myTooltip.SetToolTip(btnClearNotes, "Clears all the values from the cells that contains notes");
                _myTooltip.SetToolTip(btnFillNotes, "Fills all the empty cells with the available valid values");
                _myTooltip.SetToolTip(btnInverseCell, "Inverts the values of the selected cell");
                _myTooltip.SetToolTip(btnPauseContinue, "Pauses/Continues the current game");
                _myTooltip.SetToolTip(btnProtectCells, "Marks all the single value cells of the board as protected (mainly used for solving user defined boards)");
                _myTooltip.SetToolTip(btnRedo, "Redo the last move that was undone");
                _myTooltip.SetToolTip(btnSolve, "Solves the current board!");
                _myTooltip.SetToolTip(btnUndo, "Undo the last played move");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitDifficulty()
        {
            gSudokuEngine.gSudokuDifficulty easy = new gSudokuEngine.gSudokuDifficulty("Easy", 50, 60, 10, false);
            gSudokuEngine.gSudokuDifficulty normal = new gSudokuEngine.gSudokuDifficulty("Normal", 40, 50, 8, false);
            gSudokuEngine.gSudokuDifficulty medium = new gSudokuEngine.gSudokuDifficulty("Medium", 35, 40, 6, false);
            gSudokuEngine.gSudokuDifficulty hard = new gSudokuEngine.gSudokuDifficulty("Hard", 30, 35, 5, false);
            gSudokuEngine.gSudokuDifficulty hardSymmetry = new gSudokuEngine.gSudokuDifficulty("Hard (Symmetry)", 30, 35, 5, true);
            gSudokuEngine.gSudokuDifficulty challenging = new gSudokuEngine.gSudokuDifficulty("Challenging", 25, 30, 10, false);
            gSudokuEngine.gSudokuDifficulty challengingSymmetry = new gSudokuEngine.gSudokuDifficulty("Challenging (Symmetry)", 25, 30, 10, true);
            gSudokuEngine.gSudokuDifficulty veryHard = new gSudokuEngine.gSudokuDifficulty("Very Hard", 20, 25, 10, false);
            gSudokuEngine.gSudokuDifficulty veryHardSymmetry = new gSudokuEngine.gSudokuDifficulty("Very Hard (Symmetry)", 20, 25, 10, true);
            gSudokuEngine.gSudokuDifficulty evil = new gSudokuEngine.gSudokuDifficulty("Evil", 20, 25, 2, false);
            cmbDifficulty.Items.Clear();
            cmbDifficulty.Items.Add(easy);
            cmbDifficulty.Items.Add(normal);
            cmbDifficulty.Items.Add(medium);
            cmbDifficulty.Items.Add(hard);
            cmbDifficulty.Items.Add(hardSymmetry);
            cmbDifficulty.Items.Add(challenging);
            cmbDifficulty.Items.Add(challengingSymmetry);
            cmbDifficulty.Items.Add(veryHard);
            cmbDifficulty.Items.Add(veryHardSymmetry);
            cmbDifficulty.Items.Add(evil);
            cmbDifficulty.SelectedItem = normal;
        }

        private void OnBgWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                lblEllapsedTime.Text = _gameTimer.Elapsed.ToString(@"hh\:mm\:ss");
                lblGameStatusValue.Text = GetGameStatus();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnBgWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (_game.GameStatus == gSudokuEngine.gSudokuGameStatus.Playing)
                {
                    System.Threading.Thread.Sleep(200);
                    _bgWorker.ReportProgress(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatistics()
        {
            StringBuilder info = new StringBuilder();
            info.AppendFormat("Protected Cells: {0}\r\n", _game.GetNumberOfProtectedCells());
            info.AppendFormat("User filled Cells: {0}\r\n", _game.GetNumberOfUserFilledCells());
            info.AppendFormat("Empty Cells: {0}\r\n", _game.GetNumberOfEmptyCells());
            info.AppendFormat("Current Move: {0}/{1} [{2}]\r\n", 
                _game.CurrentMoveIndex + 1, _game.PlayerMoves.Count, _game.PlayerActionsCounter);
            txtStats.Text = info.ToString();
            for (Int32 i = 1; i <= 9; i++)
            {
                Control[] myControls = this.Controls.Find("btnVal" + i.ToString(), true);
                if (myControls.Length == 1)
                {
                    if (myControls[0] is Button)
                    {
                        ((Button)myControls[0]).Text = String.Format("{0} [{1}]", i, _game.GetNumberOfSingleValueCells(i));
                    }
                }
            }
            if (_game.GameDifficulty != null)
            {
                lblGameDifficultyValue.Text = _game.GameDifficulty.Name;
            }
            else
            {
                lblGameDifficultyValue.Text = string.Empty;
            }
        }

        private String GetGameStatus()
        {
            return Enum.GetName(typeof(gSudokuGameStatus), _game.GameStatus);
        }

        private String GetGameStats()
        {
            StringBuilder gameStats = new StringBuilder();
            gameStats.AppendFormat("Difficulty : {0}\r\n", _game.GameDifficulty.Name);
            gameStats.AppendFormat("Clues : {0}\r\n\r\n", _game.GetNumberOfProtectedCells());
            gameStats.AppendFormat("Completed in : {0}\r\n", _gameTimer.Elapsed.ToString(@"hh\:mm\:ss"));
            gameStats.AppendFormat("Total user actions : {0}\r\n\r\n", _game.PlayerActionsCounter);
            Int32 autoNotes = _game.PlayerMoves.Count(s => s.MoveType == gSudokuEngine.gSudokuMoveType.FillNote);
            gameStats.AppendFormat("Used auto notes : {0}\r\n", autoNotes > 0 ? "Yes" : "No");
            Int32 autoSolve = _game.PlayerMoves.Count(s => s.MoveType == gSudokuEngine.gSudokuMoveType.Solve);
            gameStats.AppendFormat("Used auto solve : {0}\r\n", autoSolve > 0 ? "Yes" : "No");
            return gameStats.ToString();
        }

        private void OnPicSudokuBoardMouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                Tuple<gSudokuCell, Int32, Int32> myTuple = _grap.GetSelectedCell(_game, picSudokuBoard.Size, e.X, e.Y);
                _selectedCell = myTuple.Item1;
                _selectedX = myTuple.Item2;
                _selectedY = myTuple.Item3;
                picSudokuBoard.Image = _grap.DrawBoard(_game, picSudokuBoard.Size, _selectedCell);
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnBtnValueClick(object sender, EventArgs e)
        {
            try
            {
                if (_game.GameStatus == gSudokuGameStatus.Playing)
                {
                    if (sender is Button)
                    {
                        if (_selectedCell != null)
                        {
                            if (!_selectedCell.IsProtected)
                            {
                                Int32 btnValue = Convert.ToInt32(((Button)sender).Tag);
                                gSudokuEngine.gSudokuMoveType myType;
                                if (_selectedCell.ValueExists(btnValue))
                                {
                                    myType = gSudokuEngine.gSudokuMoveType.Remove;
                                }
                                else
                                {
                                    myType = gSudokuEngine.gSudokuMoveType.Add;
                                }
                                gSudokuEngine.gSudokuMove myMove = new gSudokuEngine.gSudokuMove(_selectedCell, myType, btnValue, _game.GetNewMoveId());
                                _game.PlayMove(myMove, true);
                                picSudokuBoard.Image = _grap.DrawBoard(_game, picSudokuBoard.Size, _selectedCell);
                                UpdateStatistics();
                                if (_game.GameStatus == gSudokuGameStatus.Finished)
                                {
                                    _gameTimer.Stop();
                                    MessageBox.Show("Game Finished!\r\n\r\n" + GetGameStats(), "Success!",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Game is Paused or Finished!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnBtnNewGameClick(object sender, EventArgs e)
        {
            try
            {
                tlpMain.Enabled = false;
                _game.CreateNewBoard((gSudokuEngine.gSudokuDifficulty)cmbDifficulty.SelectedItem);
                _game.Pause();
                picSudokuBoard.Image = _grap.DrawBoard(_game, picSudokuBoard.Size, _selectedCell);
                UpdateStatistics();
                _gameTimer.Stop();
                while (_bgWorker.IsBusy)
                {
                    Application.DoEvents();
                }
                _game.Continue();
                btnPauseContinue.Text = "Pause";
                _gameTimer.Restart();
                _bgWorker.RunWorkerAsync();
                tlpMain.Enabled = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tlpMain.Enabled = true;
            }
        }

        private void OnBtnClearCellClick(object sender, EventArgs e)
        {
            try
            {
                if (_game.GameStatus == gSudokuGameStatus.Playing)
                {
                    if (_selectedCell != null)
                    {
                        if (!_selectedCell.IsProtected)
                        {
                            gSudokuEngine.gSudokuMove myMove = new gSudokuEngine.gSudokuMove(_selectedCell, gSudokuEngine.gSudokuMoveType.Clear, -1,_game.GetNewMoveId());
                            _game.PlayMove(myMove, true);
                            picSudokuBoard.Image = _grap.DrawBoard(_game, picSudokuBoard.Size, _selectedCell);
                            UpdateStatistics();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Game is Paused or Finished!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnBtnInverseCellClick(object sender, EventArgs e)
        {
            try
            {
                if (_game.GameStatus == gSudokuGameStatus.Playing)
                {
                    if (_selectedCell != null)
                    {
                        if (!_selectedCell.IsProtected)
                        {
                            gSudokuEngine.gSudokuMove myMove = new gSudokuEngine.gSudokuMove(_selectedCell, gSudokuEngine.gSudokuMoveType.Inverse, -1, _game.GetNewMoveId());
                            _game.PlayMove(myMove, true);
                            picSudokuBoard.Image = _grap.DrawBoard(_game, picSudokuBoard.Size, _selectedCell);
                            UpdateStatistics();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Game is Paused or Finished!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnBtnUndoClick(object sender, EventArgs e)
        {
            try
            {
                if (_game.GameStatus == gSudokuGameStatus.Playing)
                {
                    _game.UndoMove(true);
                    picSudokuBoard.Image = _grap.DrawBoard(_game, picSudokuBoard.Size, _selectedCell);
                    UpdateStatistics();
                }
                else
                {
                    MessageBox.Show("Game is Paused or Finished!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (gSudokuEngine.SudokuEngineNoMovesToUndoException ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("No moves to undo!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnBtnRedoClick(object sender, EventArgs e)
        {
            try
            {
                if (_game.GameStatus == gSudokuGameStatus.Playing)
                {
                    _game.RedoMove(true);
                    picSudokuBoard.Image = _grap.DrawBoard(_game, picSudokuBoard.Size, _selectedCell);
                    UpdateStatistics();
                }
                else
                {
                    MessageBox.Show("Game is Paused or Finished!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (gSudokuEngine.SudokuEngineNoMovesToRedoException ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("No moves to redo!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnBtnPauseContinueClick(object sender, EventArgs e)
        {
            try
            {
                if (_game.GameStatus != gSudokuGameStatus.Finished)
                {
                    if (_game.GameStatus == gSudokuGameStatus.Playing)
                    {
                        _gameTimer.Stop();
                        _game.Pause();
                        btnPauseContinue.Text = "Continue";
                    }
                    else
                    {
                        _gameTimer.Start();
                        _game.Continue();
                        btnPauseContinue.Text = "Pause";
                        if (!_bgWorker.IsBusy)
                        {
                            _bgWorker.RunWorkerAsync();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Game is Finished!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnFrmMainResize(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState != FormWindowState.Minimized)
                {
                    picSudokuBoard.Image = _grap.DrawBoard(_game, picSudokuBoard.Size, _selectedCell);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //It gets fucking annoying!
                //MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnBtnFillNotesClick(object sender, EventArgs e)
        {
            try
            {
                if (_game.GameStatus == gSudokuGameStatus.Playing)
                {
                    _game.FillNotes();
                    picSudokuBoard.Image = _grap.DrawBoard(_game, picSudokuBoard.Size, _selectedCell);
                    UpdateStatistics();
                    if (_game.GameStatus == gSudokuGameStatus.Finished)
                    {
                        _gameTimer.Stop();
                        MessageBox.Show("Game Finished!\r\n\r\n" + GetGameStats(),
                            "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Game is Paused or Finished!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnBtnClearNotesClick(object sender, EventArgs e)
        {
            try
            {
                if (_game.GameStatus == gSudokuGameStatus.Playing)
                {
                    _game.ClearNotes();
                    picSudokuBoard.Image = _grap.DrawBoard(_game, picSudokuBoard.Size, _selectedCell);
                    UpdateStatistics();
                }
                else
                {
                    MessageBox.Show("Game is Paused or Finished!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnBtnSolveClick(object sender, EventArgs e)
        {
            try
            {
                if (_game.GameStatus == gSudokuGameStatus.Playing)
                {
                    _game.Solve();
                    picSudokuBoard.Image = _grap.DrawBoard(_game, picSudokuBoard.Size, _selectedCell);
                    UpdateStatistics();
                    if (_game.GameStatus == gSudokuGameStatus.Finished)
                    {
                        _gameTimer.Stop();
                        MessageBox.Show("Game Finished!\r\n\r\n" + GetGameStats(),
                            "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Game is Paused or Finished!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnBtnCleanGameClick(object sender, EventArgs e)
        {
            try
            {
                tlpMain.Enabled = false;
                _game.CreateNewCleanBoard();
                _game.Pause();
                picSudokuBoard.Image = _grap.DrawBoard(_game, picSudokuBoard.Size, _selectedCell);
                UpdateStatistics();
                _gameTimer.Stop();
                while (_bgWorker.IsBusy)
                {
                    Application.DoEvents();
                }
                _game.Continue();
                btnPauseContinue.Text = "Pause";
                _gameTimer.Restart();
                _bgWorker.RunWorkerAsync();
                tlpMain.Enabled = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tlpMain.Enabled = true;
            }
        }

        private void OnBtnCheckUniqueSolutionClick(object sender, EventArgs e)
        {
            try
            {
                if (_game.GameStatus == gSudokuGameStatus.Playing)
                {
                    if (_game.CheckUniqueSolution(30))
                    {
                        MessageBox.Show("Board is uniquely solvable!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Board has many solutions!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Game is Paused or Finished!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnBtnProtectCellsClick(object sender, EventArgs e)
        {
            try
            {
                if (_game.GameStatus == gSudokuGameStatus.Playing)
                {
                    _game.ProtectAllSingleValueCells();
                    picSudokuBoard.Image = _grap.DrawBoard(_game, picSudokuBoard.Size, _selectedCell);
                    UpdateStatistics();
                }
                else
                {
                    MessageBox.Show("Game is Paused or Finished!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnToolStripMenuItemSaveImageClick(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Title = "Select image file...";
                    sfd.FileName = "mySudoku.png";
                    sfd.AddExtension = true;
                    sfd.Filter = "png files (*.png|(*.png))";
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        picSudokuBoard.Image.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        MessageBox.Show("Screenshot was saved!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnCmbDifficultySelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                String toolTipInfo = String.Empty;
                if (cmbDifficulty.SelectedIndex > -1)
                {
                    if (cmbDifficulty.SelectedItem != null)
                    {
                        if (cmbDifficulty.SelectedItem is gSudokuEngine.gSudokuDifficulty)
                        {
                            toolTipInfo = ((gSudokuEngine.gSudokuDifficulty)cmbDifficulty.SelectedItem).Info();
                        }
                    }
                }
                _myTooltip.SetToolTip(cmbDifficulty, toolTipInfo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnFrmMainKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (_game.GameStatus == gSudokuGameStatus.Playing)
                {
                    if (_selectedCell != null)
                    {
                        // check if valid key was pressed
                        if(e.KeyCode == Keys.W ||
                            e.KeyCode == Keys.A ||
                            e.KeyCode == Keys.S ||
                            e.KeyCode == Keys.D ||
                            e.KeyCode == Keys.Up ||
                            e.KeyCode == Keys.Down ||
                            e.KeyCode == Keys.Left ||
                            e.KeyCode == Keys.Right ||
                            e.KeyCode == Keys.D1 ||
                            e.KeyCode == Keys.D2 ||
                            e.KeyCode == Keys.D3 ||
                            e.KeyCode == Keys.D4 ||
                            e.KeyCode == Keys.D5 ||
                            e.KeyCode == Keys.D6 ||
                            e.KeyCode == Keys.D7 ||
                            e.KeyCode == Keys.D8 ||
                            e.KeyCode == Keys.D9 ||
                            e.KeyCode == Keys.NumPad1 ||
                            e.KeyCode == Keys.NumPad2 ||
                            e.KeyCode == Keys.NumPad3 ||
                            e.KeyCode == Keys.NumPad4 ||
                            e.KeyCode == Keys.NumPad5 ||
                            e.KeyCode == Keys.NumPad6 ||
                            e.KeyCode == Keys.NumPad7 ||
                            e.KeyCode == Keys.NumPad8 ||
                            e.KeyCode == Keys.NumPad9)
                        {
                            // if key was a direction key
                            if (e.KeyCode == Keys.W ||
                                e.KeyCode == Keys.A ||
                                e.KeyCode == Keys.S ||
                                e.KeyCode == Keys.D ||
                                e.KeyCode == Keys.Up ||
                                e.KeyCode == Keys.Down ||
                                e.KeyCode == Keys.Left ||
                                e.KeyCode == Keys.Right)
                            {
                                Int32 myX = -1;
                                Int32 myY = -1;
                                // check for up
                                if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
                                {
                                    if (_selectedX == 0)
                                    {
                                        myX = 8;
                                    }
                                    else
                                    {
                                        myX = _selectedX - 1;
                                    }
                                    myY = _selectedY;
                                }
                                // check for down
                                else if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
                                {
                                    if (_selectedX == 8)
                                    {
                                        myX = 0;
                                    }
                                    else
                                    {
                                        myX = _selectedX + 1;
                                    }
                                    myY = _selectedY;
                                }
                                // check for left
                                else if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
                                {
                                    if (_selectedY == 0)
                                    {
                                        myY = 8;
                                    }
                                    else
                                    {
                                        myY = _selectedY - 1;
                                    }
                                    myX = _selectedX;
                                }
                                // check for right
                                else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
                                {
                                    if (_selectedY == 8)
                                    {
                                        myY = 0;
                                    }
                                    else
                                    {
                                        myY = _selectedY + 1;
                                    }
                                    myX = _selectedX;
                                }
                                _selectedCell = _game.Board[myX][myY];
                                _selectedX = myX;
                                _selectedY = myY;
                                picSudokuBoard.Image = _grap.DrawBoard(_game, picSudokuBoard.Size, _selectedCell);
                                UpdateStatistics();
                            }
                            // if key was a number key
                            else if (e.KeyCode == Keys.D1 ||
                            e.KeyCode == Keys.D2 ||
                            e.KeyCode == Keys.D3 ||
                            e.KeyCode == Keys.D4 ||
                            e.KeyCode == Keys.D5 ||
                            e.KeyCode == Keys.D6 ||
                            e.KeyCode == Keys.D7 ||
                            e.KeyCode == Keys.D8 ||
                            e.KeyCode == Keys.D9 ||
                            e.KeyCode == Keys.NumPad1 ||
                            e.KeyCode == Keys.NumPad2 ||
                            e.KeyCode == Keys.NumPad3 ||
                            e.KeyCode == Keys.NumPad4 ||
                            e.KeyCode == Keys.NumPad5 ||
                            e.KeyCode == Keys.NumPad6 ||
                            e.KeyCode == Keys.NumPad7 ||
                            e.KeyCode == Keys.NumPad8 ||
                            e.KeyCode == Keys.NumPad9)
                            {
                                if (!_selectedCell.IsProtected)
                                {
                                    // determine value
                                    Int32 btnValue = 0;
                                    if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
                                    {
                                        btnValue = 1;
                                    }
                                    else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
                                    {
                                        btnValue = 2;
                                    }
                                    else if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
                                    {
                                        btnValue = 3;
                                    }
                                    else if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4)
                                    {
                                        btnValue = 4;
                                    }
                                    else if (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5)
                                    {
                                        btnValue = 5;
                                    }
                                    else if (e.KeyCode == Keys.D6 || e.KeyCode == Keys.NumPad6)
                                    {
                                        btnValue = 6;
                                    }
                                    else if (e.KeyCode == Keys.D7 || e.KeyCode == Keys.NumPad7)
                                    {
                                        btnValue = 7;
                                    }
                                    else if (e.KeyCode == Keys.D8 || e.KeyCode == Keys.NumPad8)
                                    {
                                        btnValue = 8;
                                    }
                                    else if (e.KeyCode == Keys.D9 || e.KeyCode == Keys.NumPad9)
                                    {
                                        btnValue = 9;
                                    }                                  
                                    gSudokuEngine.gSudokuMoveType myType;
                                    if (_selectedCell.ValueExists(btnValue))
                                    {
                                        myType = gSudokuEngine.gSudokuMoveType.Remove;
                                    }
                                    else
                                    {
                                        myType = gSudokuEngine.gSudokuMoveType.Add;
                                    }
                                    gSudokuEngine.gSudokuMove myMove = new gSudokuEngine.gSudokuMove(_selectedCell, myType, btnValue, _game.GetNewMoveId());
                                    _game.PlayMove(myMove, true);
                                    picSudokuBoard.Image = _grap.DrawBoard(_game, picSudokuBoard.Size, _selectedCell);
                                    UpdateStatistics();
                                    if (_game.GameStatus == gSudokuGameStatus.Finished)
                                    {
                                        _gameTimer.Stop();
                                        MessageBox.Show("Game Finished!\r\n\r\n" + GetGameStats(), "Success!",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error!\r\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
