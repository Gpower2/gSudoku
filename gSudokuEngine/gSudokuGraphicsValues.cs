using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;

namespace gSudokuEngine
{
    [Serializable()]
    public class gSudokuGraphicsValues
    {
        public Int32 BoardThinLineWidth { get; set; }
        public Int32 BoardThickLineWidth { get; set; }

        public Color BoardThickLineColor { get; set; }
        public Color BoardThinLineColor { get; set; }

        public Color BoardCellEmpty { get; set; }
        public Color BoardCellProtected { get; set; }
        public Color BoardCellProtectedInvalid { get; set; }
        public Color BoardCellSingleValueValid { get; set; }
        public Color BoardCellSingleValueValidSelected { get; set; }
        public Color BoardCellSingleValueInvalid { get; set; }
        public Color BoardCellSingleValueInvalidSelected { get; set; }
        public Color BoardCellMultiValue { get; set; }
        public Color BoardCellSelected { get; set; }
        public Color BoardCellSelectedProtected { get; set; }

        public Color BoardSingleValue { get; set; }
        public Color BoardSingleValueProtected { get; set; }
        public Color BoardMultiValue { get; set; }

        public FontFamily BoardCellFontFamily { get; set; }

        public gSudokuGraphicsValues() 
        {
            BoardThinLineWidth = 2;
            BoardThickLineWidth = 4;

            BoardThickLineColor = Color.Black;
            BoardThinLineColor = Color.Black;

            BoardCellEmpty = Color.White;
            BoardCellProtected = Color.WhiteSmoke;
            BoardCellProtectedInvalid = Color.IndianRed;
            BoardCellSingleValueValid = Color.WhiteSmoke;
            BoardCellSingleValueValidSelected = Color.Yellow;
            BoardCellSingleValueInvalid = Color.Red;
            BoardCellSingleValueInvalidSelected = Color.OrangeRed;
            BoardCellMultiValue = Color.LightBlue;
            BoardCellSelected = Color.LightGreen;
            BoardCellSelectedProtected = Color.Orange;

            BoardSingleValue = Color.Black;
            BoardSingleValueProtected = Color.Black;
            BoardMultiValue = Color.Black;

            BoardCellFontFamily = new FontFamily("Arial");
        }

        //Deserialization constructor.
        public gSudokuGraphicsValues(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties

            //EmpId = (int)info.GetValue("EmployeeId", typeof(int));
            //EmpName = (String)info.GetValue("EmployeeName", typeof(string));
        }
        
        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            //You can use any custom name for your name-value pair. But make sure you
            //read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
            //then you should read the same with "EmployeeId"

            //info.AddValue("EmployeeId", EmpId);
            //info.AddValue("EmployeeName", EmpName);
        }
    }
}
