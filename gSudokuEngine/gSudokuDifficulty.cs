using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gSudokuEngine
{
    public class gSudokuDifficulty
    {
        public Int32 Clues { get { return _rnd.Next(_minClues, _maxClues); } }
        public Int32 UniqueSolutionAttempts { get { return _uniqueSolutionAttempts; } }
        public Boolean UseSymmetry { get { return _useSymmetry; } }
        public String Name { get { return _name; } }

        private Int32 _minClues;
        private Int32 _maxClues;
        private Int32 _uniqueSolutionAttempts;
        private Boolean _useSymmetry;
        private Random _rnd;
        private String _name;

        public gSudokuDifficulty(String name, Int32 minClues, Int32 maxClues, Int32 uniqueSolutionAttempts, Boolean useSymmetry)
        {
            _rnd = new Random(DateTime.Now.Millisecond);
            _name = name;
            _minClues = minClues;
            _maxClues = maxClues;
            _uniqueSolutionAttempts = uniqueSolutionAttempts;
            _useSymmetry = useSymmetry;
        }

        public override string ToString()
        {
            return _name;
        }

        public String Info()
        {
            StringBuilder info = new StringBuilder();
            info.AppendFormat("Clues: {0} - {1}\r\nUnique Solution Attempts: {2}\r\nSymmetry: {3}",
                _minClues, _maxClues, _uniqueSolutionAttempts, _useSymmetry ? "Yes" : "No");
            return info.ToString();
        }
    }
}
