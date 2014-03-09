namespace gSudoku
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.picSudokuBoard = new System.Windows.Forms.PictureBox();
            this.contextMenuPicture = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.btnVal1 = new System.Windows.Forms.Button();
            this.btnVal2 = new System.Windows.Forms.Button();
            this.btnVal3 = new System.Windows.Forms.Button();
            this.btnVal6 = new System.Windows.Forms.Button();
            this.btnVal5 = new System.Windows.Forms.Button();
            this.btnVal4 = new System.Windows.Forms.Button();
            this.btnVal9 = new System.Windows.Forms.Button();
            this.btnVal8 = new System.Windows.Forms.Button();
            this.btnVal7 = new System.Windows.Forms.Button();
            this.btnClearCell = new System.Windows.Forms.Button();
            this.btnInverseCell = new System.Windows.Forms.Button();
            this.btnNewGame = new System.Windows.Forms.Button();
            this.grpValues = new System.Windows.Forms.GroupBox();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnRedo = new System.Windows.Forms.Button();
            this.grpCellActions = new System.Windows.Forms.GroupBox();
            this.btnCheckUniqueSolution = new System.Windows.Forms.Button();
            this.btnSolve = new System.Windows.Forms.Button();
            this.btnClearNotes = new System.Windows.Forms.Button();
            this.btnFillNotes = new System.Windows.Forms.Button();
            this.grpGameActions = new System.Windows.Forms.GroupBox();
            this.cmbDifficulty = new System.Windows.Forms.ComboBox();
            this.btnProtectCells = new System.Windows.Forms.Button();
            this.btnCleanGame = new System.Windows.Forms.Button();
            this.btnPauseContinue = new System.Windows.Forms.Button();
            this.txtStats = new System.Windows.Forms.TextBox();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.tlpBoard = new System.Windows.Forms.TableLayoutPanel();
            this.grpStats = new System.Windows.Forms.GroupBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblTimeStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblEllapsedTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.tSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblGameDifficulty = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblGameDifficultyValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.tSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.lblGameStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblGameStatusValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlpForm = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.picSudokuBoard)).BeginInit();
            this.contextMenuPicture.SuspendLayout();
            this.grpValues.SuspendLayout();
            this.grpCellActions.SuspendLayout();
            this.grpGameActions.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.tlpBoard.SuspendLayout();
            this.grpStats.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.tlpForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // picSudokuBoard
            // 
            this.picSudokuBoard.ContextMenuStrip = this.contextMenuPicture;
            this.picSudokuBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picSudokuBoard.Location = new System.Drawing.Point(3, 3);
            this.picSudokuBoard.Name = "picSudokuBoard";
            this.picSudokuBoard.Size = new System.Drawing.Size(424, 372);
            this.picSudokuBoard.TabIndex = 0;
            this.picSudokuBoard.TabStop = false;
            this.picSudokuBoard.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnPicSudokuBoardMouseClick);
            // 
            // contextMenuPicture
            // 
            this.contextMenuPicture.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSaveImage});
            this.contextMenuPicture.Name = "contextMenuPicture";
            this.contextMenuPicture.Size = new System.Drawing.Size(144, 26);
            // 
            // toolStripMenuItemSaveImage
            // 
            this.toolStripMenuItemSaveImage.Name = "toolStripMenuItemSaveImage";
            this.toolStripMenuItemSaveImage.Size = new System.Drawing.Size(143, 22);
            this.toolStripMenuItemSaveImage.Text = "Save Image...";
            this.toolStripMenuItemSaveImage.Click += new System.EventHandler(this.OnToolStripMenuItemSaveImageClick);
            // 
            // btnVal1
            // 
            this.btnVal1.Location = new System.Drawing.Point(14, 19);
            this.btnVal1.Name = "btnVal1";
            this.btnVal1.Size = new System.Drawing.Size(45, 45);
            this.btnVal1.TabIndex = 1;
            this.btnVal1.Tag = "1";
            this.btnVal1.Text = "1";
            this.btnVal1.UseVisualStyleBackColor = true;
            this.btnVal1.Click += new System.EventHandler(this.OnBtnValueClick);
            // 
            // btnVal2
            // 
            this.btnVal2.Location = new System.Drawing.Point(65, 19);
            this.btnVal2.Name = "btnVal2";
            this.btnVal2.Size = new System.Drawing.Size(45, 45);
            this.btnVal2.TabIndex = 2;
            this.btnVal2.Tag = "2";
            this.btnVal2.Text = "2";
            this.btnVal2.UseVisualStyleBackColor = true;
            this.btnVal2.Click += new System.EventHandler(this.OnBtnValueClick);
            // 
            // btnVal3
            // 
            this.btnVal3.Location = new System.Drawing.Point(116, 19);
            this.btnVal3.Name = "btnVal3";
            this.btnVal3.Size = new System.Drawing.Size(45, 45);
            this.btnVal3.TabIndex = 3;
            this.btnVal3.Tag = "3";
            this.btnVal3.Text = "3";
            this.btnVal3.UseVisualStyleBackColor = true;
            this.btnVal3.Click += new System.EventHandler(this.OnBtnValueClick);
            // 
            // btnVal6
            // 
            this.btnVal6.Location = new System.Drawing.Point(116, 70);
            this.btnVal6.Name = "btnVal6";
            this.btnVal6.Size = new System.Drawing.Size(45, 45);
            this.btnVal6.TabIndex = 6;
            this.btnVal6.Tag = "6";
            this.btnVal6.Text = "6";
            this.btnVal6.UseVisualStyleBackColor = true;
            this.btnVal6.Click += new System.EventHandler(this.OnBtnValueClick);
            // 
            // btnVal5
            // 
            this.btnVal5.Location = new System.Drawing.Point(65, 70);
            this.btnVal5.Name = "btnVal5";
            this.btnVal5.Size = new System.Drawing.Size(45, 45);
            this.btnVal5.TabIndex = 5;
            this.btnVal5.Tag = "5";
            this.btnVal5.Text = "5";
            this.btnVal5.UseVisualStyleBackColor = true;
            this.btnVal5.Click += new System.EventHandler(this.OnBtnValueClick);
            // 
            // btnVal4
            // 
            this.btnVal4.Location = new System.Drawing.Point(14, 70);
            this.btnVal4.Name = "btnVal4";
            this.btnVal4.Size = new System.Drawing.Size(45, 45);
            this.btnVal4.TabIndex = 4;
            this.btnVal4.Tag = "4";
            this.btnVal4.Text = "4";
            this.btnVal4.UseVisualStyleBackColor = true;
            this.btnVal4.Click += new System.EventHandler(this.OnBtnValueClick);
            // 
            // btnVal9
            // 
            this.btnVal9.Location = new System.Drawing.Point(116, 121);
            this.btnVal9.Name = "btnVal9";
            this.btnVal9.Size = new System.Drawing.Size(45, 45);
            this.btnVal9.TabIndex = 9;
            this.btnVal9.Tag = "9";
            this.btnVal9.Text = "9";
            this.btnVal9.UseVisualStyleBackColor = true;
            this.btnVal9.Click += new System.EventHandler(this.OnBtnValueClick);
            // 
            // btnVal8
            // 
            this.btnVal8.Location = new System.Drawing.Point(65, 121);
            this.btnVal8.Name = "btnVal8";
            this.btnVal8.Size = new System.Drawing.Size(45, 45);
            this.btnVal8.TabIndex = 8;
            this.btnVal8.Tag = "8";
            this.btnVal8.Text = "8";
            this.btnVal8.UseVisualStyleBackColor = true;
            this.btnVal8.Click += new System.EventHandler(this.OnBtnValueClick);
            // 
            // btnVal7
            // 
            this.btnVal7.Location = new System.Drawing.Point(14, 121);
            this.btnVal7.Name = "btnVal7";
            this.btnVal7.Size = new System.Drawing.Size(45, 45);
            this.btnVal7.TabIndex = 7;
            this.btnVal7.Tag = "7";
            this.btnVal7.Text = "7";
            this.btnVal7.UseVisualStyleBackColor = true;
            this.btnVal7.Click += new System.EventHandler(this.OnBtnValueClick);
            // 
            // btnClearCell
            // 
            this.btnClearCell.Location = new System.Drawing.Point(6, 14);
            this.btnClearCell.Name = "btnClearCell";
            this.btnClearCell.Size = new System.Drawing.Size(80, 35);
            this.btnClearCell.TabIndex = 10;
            this.btnClearCell.Text = "Clear";
            this.btnClearCell.UseVisualStyleBackColor = true;
            this.btnClearCell.Click += new System.EventHandler(this.OnBtnClearCellClick);
            // 
            // btnInverseCell
            // 
            this.btnInverseCell.Location = new System.Drawing.Point(89, 14);
            this.btnInverseCell.Name = "btnInverseCell";
            this.btnInverseCell.Size = new System.Drawing.Size(80, 35);
            this.btnInverseCell.TabIndex = 11;
            this.btnInverseCell.Text = "Inverse";
            this.btnInverseCell.UseVisualStyleBackColor = true;
            this.btnInverseCell.Click += new System.EventHandler(this.OnBtnInverseCellClick);
            // 
            // btnNewGame
            // 
            this.btnNewGame.Location = new System.Drawing.Point(6, 17);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(80, 35);
            this.btnNewGame.TabIndex = 12;
            this.btnNewGame.Text = "New";
            this.btnNewGame.UseVisualStyleBackColor = true;
            this.btnNewGame.Click += new System.EventHandler(this.OnBtnNewGameClick);
            // 
            // grpValues
            // 
            this.grpValues.Controls.Add(this.btnVal2);
            this.grpValues.Controls.Add(this.btnVal1);
            this.grpValues.Controls.Add(this.btnVal3);
            this.grpValues.Controls.Add(this.btnVal4);
            this.grpValues.Controls.Add(this.btnVal9);
            this.grpValues.Controls.Add(this.btnVal5);
            this.grpValues.Controls.Add(this.btnVal8);
            this.grpValues.Controls.Add(this.btnVal6);
            this.grpValues.Controls.Add(this.btnVal7);
            this.grpValues.Location = new System.Drawing.Point(8, 2);
            this.grpValues.Name = "grpValues";
            this.grpValues.Size = new System.Drawing.Size(176, 176);
            this.grpValues.TabIndex = 13;
            this.grpValues.TabStop = false;
            // 
            // btnUndo
            // 
            this.btnUndo.Location = new System.Drawing.Point(6, 51);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(80, 35);
            this.btnUndo.TabIndex = 14;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.OnBtnUndoClick);
            // 
            // btnRedo
            // 
            this.btnRedo.Location = new System.Drawing.Point(90, 51);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(80, 35);
            this.btnRedo.TabIndex = 15;
            this.btnRedo.Text = "Redo";
            this.btnRedo.UseVisualStyleBackColor = true;
            this.btnRedo.Click += new System.EventHandler(this.OnBtnRedoClick);
            // 
            // grpCellActions
            // 
            this.grpCellActions.Controls.Add(this.btnCheckUniqueSolution);
            this.grpCellActions.Controls.Add(this.btnSolve);
            this.grpCellActions.Controls.Add(this.btnClearNotes);
            this.grpCellActions.Controls.Add(this.btnFillNotes);
            this.grpCellActions.Controls.Add(this.btnClearCell);
            this.grpCellActions.Controls.Add(this.btnRedo);
            this.grpCellActions.Controls.Add(this.btnInverseCell);
            this.grpCellActions.Controls.Add(this.btnUndo);
            this.grpCellActions.Location = new System.Drawing.Point(9, 181);
            this.grpCellActions.Name = "grpCellActions";
            this.grpCellActions.Size = new System.Drawing.Size(175, 170);
            this.grpCellActions.TabIndex = 16;
            this.grpCellActions.TabStop = false;
            // 
            // btnCheckUniqueSolution
            // 
            this.btnCheckUniqueSolution.Location = new System.Drawing.Point(90, 128);
            this.btnCheckUniqueSolution.Name = "btnCheckUniqueSolution";
            this.btnCheckUniqueSolution.Size = new System.Drawing.Size(80, 35);
            this.btnCheckUniqueSolution.TabIndex = 19;
            this.btnCheckUniqueSolution.Text = "Unique?";
            this.btnCheckUniqueSolution.UseVisualStyleBackColor = true;
            this.btnCheckUniqueSolution.Click += new System.EventHandler(this.OnBtnCheckUniqueSolutionClick);
            // 
            // btnSolve
            // 
            this.btnSolve.Location = new System.Drawing.Point(6, 128);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(80, 35);
            this.btnSolve.TabIndex = 18;
            this.btnSolve.Text = "Solve!";
            this.btnSolve.UseVisualStyleBackColor = true;
            this.btnSolve.Click += new System.EventHandler(this.OnBtnSolveClick);
            // 
            // btnClearNotes
            // 
            this.btnClearNotes.Location = new System.Drawing.Point(90, 89);
            this.btnClearNotes.Name = "btnClearNotes";
            this.btnClearNotes.Size = new System.Drawing.Size(80, 35);
            this.btnClearNotes.TabIndex = 17;
            this.btnClearNotes.Text = "Clear Notes";
            this.btnClearNotes.UseVisualStyleBackColor = true;
            this.btnClearNotes.Click += new System.EventHandler(this.OnBtnClearNotesClick);
            // 
            // btnFillNotes
            // 
            this.btnFillNotes.Location = new System.Drawing.Point(6, 89);
            this.btnFillNotes.Name = "btnFillNotes";
            this.btnFillNotes.Size = new System.Drawing.Size(80, 35);
            this.btnFillNotes.TabIndex = 16;
            this.btnFillNotes.Text = "Fill Notes";
            this.btnFillNotes.UseVisualStyleBackColor = true;
            this.btnFillNotes.Click += new System.EventHandler(this.OnBtnFillNotesClick);
            // 
            // grpGameActions
            // 
            this.grpGameActions.Controls.Add(this.cmbDifficulty);
            this.grpGameActions.Controls.Add(this.btnProtectCells);
            this.grpGameActions.Controls.Add(this.btnCleanGame);
            this.grpGameActions.Controls.Add(this.btnPauseContinue);
            this.grpGameActions.Controls.Add(this.btnNewGame);
            this.grpGameActions.Location = new System.Drawing.Point(10, 350);
            this.grpGameActions.Name = "grpGameActions";
            this.grpGameActions.Size = new System.Drawing.Size(174, 125);
            this.grpGameActions.TabIndex = 17;
            this.grpGameActions.TabStop = false;
            // 
            // cmbDifficulty
            // 
            this.cmbDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDifficulty.FormattingEnabled = true;
            this.cmbDifficulty.Location = new System.Drawing.Point(6, 97);
            this.cmbDifficulty.Name = "cmbDifficulty";
            this.cmbDifficulty.Size = new System.Drawing.Size(162, 23);
            this.cmbDifficulty.TabIndex = 18;
            this.cmbDifficulty.SelectedIndexChanged += new System.EventHandler(this.OnCmbDifficultySelectedIndexChanged);
            // 
            // btnProtectCells
            // 
            this.btnProtectCells.Location = new System.Drawing.Point(89, 55);
            this.btnProtectCells.Name = "btnProtectCells";
            this.btnProtectCells.Size = new System.Drawing.Size(80, 35);
            this.btnProtectCells.TabIndex = 17;
            this.btnProtectCells.Text = "Protect";
            this.btnProtectCells.UseVisualStyleBackColor = true;
            this.btnProtectCells.Click += new System.EventHandler(this.OnBtnProtectCellsClick);
            // 
            // btnCleanGame
            // 
            this.btnCleanGame.Location = new System.Drawing.Point(89, 17);
            this.btnCleanGame.Name = "btnCleanGame";
            this.btnCleanGame.Size = new System.Drawing.Size(80, 35);
            this.btnCleanGame.TabIndex = 16;
            this.btnCleanGame.Text = "Clean";
            this.btnCleanGame.UseVisualStyleBackColor = true;
            this.btnCleanGame.Click += new System.EventHandler(this.OnBtnCleanGameClick);
            // 
            // btnPauseContinue
            // 
            this.btnPauseContinue.Location = new System.Drawing.Point(6, 55);
            this.btnPauseContinue.Name = "btnPauseContinue";
            this.btnPauseContinue.Size = new System.Drawing.Size(80, 35);
            this.btnPauseContinue.TabIndex = 14;
            this.btnPauseContinue.Text = "Pause";
            this.btnPauseContinue.UseVisualStyleBackColor = true;
            this.btnPauseContinue.Click += new System.EventHandler(this.OnBtnPauseContinueClick);
            // 
            // txtStats
            // 
            this.txtStats.BackColor = System.Drawing.SystemColors.Window;
            this.txtStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtStats.Location = new System.Drawing.Point(3, 19);
            this.txtStats.Multiline = true;
            this.txtStats.Name = "txtStats";
            this.txtStats.ReadOnly = true;
            this.txtStats.Size = new System.Drawing.Size(418, 72);
            this.txtStats.TabIndex = 18;
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tlpMain.Controls.Add(this.pnlActions, 1, 0);
            this.tlpMain.Controls.Add(this.tlpBoard, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(3, 3);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(636, 484);
            this.tlpMain.TabIndex = 19;
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.grpValues);
            this.pnlActions.Controls.Add(this.grpCellActions);
            this.pnlActions.Controls.Add(this.grpGameActions);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlActions.Location = new System.Drawing.Point(439, 3);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(194, 478);
            this.pnlActions.TabIndex = 0;
            // 
            // tlpBoard
            // 
            this.tlpBoard.ColumnCount = 1;
            this.tlpBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBoard.Controls.Add(this.picSudokuBoard, 0, 0);
            this.tlpBoard.Controls.Add(this.grpStats, 0, 1);
            this.tlpBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBoard.Location = new System.Drawing.Point(3, 3);
            this.tlpBoard.Name = "tlpBoard";
            this.tlpBoard.RowCount = 2;
            this.tlpBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpBoard.Size = new System.Drawing.Size(430, 478);
            this.tlpBoard.TabIndex = 1;
            // 
            // grpStats
            // 
            this.grpStats.Controls.Add(this.txtStats);
            this.grpStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpStats.Location = new System.Drawing.Point(3, 381);
            this.grpStats.Name = "grpStats";
            this.grpStats.Size = new System.Drawing.Size(424, 94);
            this.grpStats.TabIndex = 1;
            this.grpStats.TabStop = false;
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblTimeStatus,
            this.lblEllapsedTime,
            this.tSeparator1,
            this.lblGameDifficulty,
            this.lblGameDifficultyValue,
            this.tSeparator2,
            this.lblGameStatus,
            this.lblGameStatusValue});
            this.statusStrip.Location = new System.Drawing.Point(0, 490);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(642, 20);
            this.statusStrip.TabIndex = 20;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblTimeStatus
            // 
            this.lblTimeStatus.Name = "lblTimeStatus";
            this.lblTimeStatus.Size = new System.Drawing.Size(83, 15);
            this.lblTimeStatus.Text = "Time ellapsed:";
            // 
            // lblEllapsedTime
            // 
            this.lblEllapsedTime.Name = "lblEllapsedTime";
            this.lblEllapsedTime.Size = new System.Drawing.Size(0, 15);
            // 
            // tSeparator1
            // 
            this.tSeparator1.Name = "tSeparator1";
            this.tSeparator1.Size = new System.Drawing.Size(6, 20);
            // 
            // lblGameDifficulty
            // 
            this.lblGameDifficulty.Name = "lblGameDifficulty";
            this.lblGameDifficulty.Size = new System.Drawing.Size(58, 15);
            this.lblGameDifficulty.Text = "Difficulty:";
            // 
            // lblGameDifficultyValue
            // 
            this.lblGameDifficultyValue.Name = "lblGameDifficultyValue";
            this.lblGameDifficultyValue.Size = new System.Drawing.Size(0, 15);
            // 
            // tSeparator2
            // 
            this.tSeparator2.Name = "tSeparator2";
            this.tSeparator2.Size = new System.Drawing.Size(6, 20);
            // 
            // lblGameStatus
            // 
            this.lblGameStatus.Name = "lblGameStatus";
            this.lblGameStatus.Size = new System.Drawing.Size(42, 15);
            this.lblGameStatus.Text = "Status:";
            // 
            // lblGameStatusValue
            // 
            this.lblGameStatusValue.Name = "lblGameStatusValue";
            this.lblGameStatusValue.Size = new System.Drawing.Size(0, 15);
            // 
            // tlpForm
            // 
            this.tlpForm.ColumnCount = 1;
            this.tlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpForm.Controls.Add(this.tlpMain, 0, 0);
            this.tlpForm.Controls.Add(this.statusStrip, 0, 1);
            this.tlpForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpForm.Location = new System.Drawing.Point(0, 0);
            this.tlpForm.Name = "tlpForm";
            this.tlpForm.RowCount = 2;
            this.tlpForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpForm.Size = new System.Drawing.Size(642, 510);
            this.tlpForm.TabIndex = 21;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 510);
            this.Controls.Add(this.tlpForm);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.KeyPreview = true;
            this.Name = "frmMain";
            this.Text = "gSudoku";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.Resize += new System.EventHandler(this.OnFrmMainResize);
            ((System.ComponentModel.ISupportInitialize)(this.picSudokuBoard)).EndInit();
            this.contextMenuPicture.ResumeLayout(false);
            this.grpValues.ResumeLayout(false);
            this.grpCellActions.ResumeLayout(false);
            this.grpGameActions.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.tlpBoard.ResumeLayout(false);
            this.grpStats.ResumeLayout(false);
            this.grpStats.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tlpForm.ResumeLayout(false);
            this.tlpForm.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picSudokuBoard;
        private System.Windows.Forms.Button btnVal1;
        private System.Windows.Forms.Button btnVal2;
        private System.Windows.Forms.Button btnVal3;
        private System.Windows.Forms.Button btnVal6;
        private System.Windows.Forms.Button btnVal5;
        private System.Windows.Forms.Button btnVal4;
        private System.Windows.Forms.Button btnVal9;
        private System.Windows.Forms.Button btnVal8;
        private System.Windows.Forms.Button btnVal7;
        private System.Windows.Forms.Button btnClearCell;
        private System.Windows.Forms.Button btnInverseCell;
        private System.Windows.Forms.Button btnNewGame;
        private System.Windows.Forms.GroupBox grpValues;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnRedo;
        private System.Windows.Forms.GroupBox grpCellActions;
        private System.Windows.Forms.GroupBox grpGameActions;
        private System.Windows.Forms.TextBox txtStats;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnPauseContinue;
        private System.Windows.Forms.TableLayoutPanel tlpBoard;
        private System.Windows.Forms.GroupBox grpStats;
        private System.Windows.Forms.Button btnFillNotes;
        private System.Windows.Forms.Button btnClearNotes;
        private System.Windows.Forms.Button btnSolve;
        private System.Windows.Forms.Button btnCleanGame;
        private System.Windows.Forms.Button btnCheckUniqueSolution;
        private System.Windows.Forms.Button btnProtectCells;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.TableLayoutPanel tlpForm;
        private System.Windows.Forms.ToolStripStatusLabel lblEllapsedTime;
        private System.Windows.Forms.ToolStripStatusLabel lblTimeStatus;
        private System.Windows.Forms.ToolStripSeparator tSeparator1;
        private System.Windows.Forms.ToolStripSeparator tSeparator2;
        private System.Windows.Forms.ComboBox cmbDifficulty;
        private System.Windows.Forms.ContextMenuStrip contextMenuPicture;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSaveImage;
        private System.Windows.Forms.ToolStripStatusLabel lblGameStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblGameStatusValue;
        private System.Windows.Forms.ToolStripStatusLabel lblGameDifficulty;
        private System.Windows.Forms.ToolStripStatusLabel lblGameDifficultyValue;
    }
}

