using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gSudokuEngine
{
    public class gSudokuSaveGame
    {
        public gSudokuDifficulty Difficulty { get; set; }
        public Int32 TotalClues { get; set; }
        public Int32 UserActions { get; set; }
        public Int64 ElapsedTime { get; set; }
    }
}
