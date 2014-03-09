using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace gSudokuEngine
{
    public class gSudokuGraphics
    {
        private gSudokuGraphicsValues _GraphicsValues;

        public Int32 CellWidth(Size bitmapSize) 
        {
            Int32 totalLineWidth = 6 * _GraphicsValues.BoardThinLineWidth + 4 * _GraphicsValues.BoardThickLineWidth;
            return (bitmapSize.Width - totalLineWidth) / 9;
        }

        public Int32 CellHeight(Size bitmapSize)
        {
            Int32 totalLineWidth = 6 * _GraphicsValues.BoardThinLineWidth + 4 * _GraphicsValues.BoardThickLineWidth;
            return (bitmapSize.Height - totalLineWidth) / 9;
        }

        public gSudokuGraphics(gSudokuGraphicsValues gValues)
        {
            _GraphicsValues = gValues;
        }

        public Image DrawBoard(gSudokuGame game, Size bitmapSize, gSudokuCell selectedCell)
        {
            //Create new bitmap 
            Bitmap bmpBoard = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            //Get the Graphics object
            Graphics g = Graphics.FromImage(bmpBoard);

            //Calculate cell size
            Int32 totalLineWidth = 6 * _GraphicsValues.BoardThinLineWidth + 4 * _GraphicsValues.BoardThickLineWidth;
            Int32 cellWidth = (bitmapSize.Width - totalLineWidth) / 9;
            Int32 cellHeight = (bitmapSize.Height - totalLineWidth) / 9;
            Point p1 = new Point();
            Point p2 = new Point();
            Int32 selectedValue = -1;
            //Draw each cell
            //Get all invalid values       
            List<Int32> invalidValues = new List<Int32>();
            for (Int32 i = 0; i < 9; i++)
            {
                for (Int32 j = 0; j < 9; j++)
                {
                    if (game.Board[i][j].ValuesCount == 1 && !game.CheckMoveValidity(i, j, game.Board[i][j].SingleValue))
                    {
                        if (!invalidValues.Contains(game.Board[i][j].SingleValue))
                        {
                            invalidValues.Add(game.Board[i][j].SingleValue);
                        }
                    }
                }
            }
            if (selectedCell != null)
            {
                if (selectedCell.ValuesCount == 1)
                {
                    selectedValue = selectedCell.SingleValue;
                }
            }
            for (Int32 i = 0; i < 9; i++)
            {
                for (Int32 j = 0; j < 9; j++)
                {
                    //Get the coordinates of the cell
                    if (i >= 0 && i <= 2)
                    {
                        p1.Y = _GraphicsValues.BoardThickLineWidth + i * _GraphicsValues.BoardThinLineWidth + i * cellHeight;
                    }
                    else if (i >= 3 && i <= 5)
                    {
                        p1.Y = 2 * _GraphicsValues.BoardThickLineWidth + (i - 1) * _GraphicsValues.BoardThinLineWidth + i * cellHeight;
                    }
                    else if (i >= 6 && i <= 8)
                    {
                        p1.Y = 3 * _GraphicsValues.BoardThickLineWidth + (i - 2) * _GraphicsValues.BoardThinLineWidth + i * cellHeight;
                    }

                    if (j >= 0 && j <= 2)
                    {
                        p1.X = _GraphicsValues.BoardThickLineWidth + j * _GraphicsValues.BoardThinLineWidth + j * cellWidth;
                    }
                    else if (j >= 3 && j <= 5)
                    {
                        p1.X = 2 * _GraphicsValues.BoardThickLineWidth + (j - 1) * _GraphicsValues.BoardThinLineWidth + j * cellWidth;
                    }
                    else if (j >= 6 && j <= 8)
                    {
                        p1.X = 3 * _GraphicsValues.BoardThickLineWidth + (j - 2) * _GraphicsValues.BoardThinLineWidth + j * cellWidth;
                    }

                    //Draw cell background color
                    Boolean cellDrawn = false;
                    if (selectedCell != null)
                    {
                        if (selectedValue > -1)
                        {
                            if (game.Board[i][j] == selectedCell)
                            {
                                if (game.Board[i][j].IsProtected)
                                {
                                    g.FillRectangle(new SolidBrush(_GraphicsValues.BoardCellSelectedProtected), p1.X, p1.Y, cellWidth, cellHeight);
                                    cellDrawn = true;
                                }
                                else
                                {
                                    if (game.Board[i][j].ValuesCount == 1)
                                    {
                                        g.FillRectangle(new SolidBrush(_GraphicsValues.BoardCellSelected), p1.X, p1.Y, cellWidth, cellHeight);
                                        cellDrawn = true;
                                    }
                                }
                            }
                            else 
                            {
                                if (game.Board[i][j].ValuesCount == 1 && game.Board[i][j].ValueExists(selectedValue))
                                {
                                    if (!invalidValues.Contains(selectedValue))
                                    {
                                        g.FillRectangle(new SolidBrush(_GraphicsValues.BoardCellSingleValueValidSelected), p1.X, p1.Y, cellWidth, cellHeight);
                                        cellDrawn = true;
                                    }
                                    else
                                    {
                                        if (game.Board[i][j].IsProtected)
                                        {
                                            g.FillRectangle(new SolidBrush(_GraphicsValues.BoardCellProtectedInvalid), p1.X, p1.Y, cellWidth, cellHeight);
                                            cellDrawn = true;
                                        }
                                        else
                                        {
                                            g.FillRectangle(new SolidBrush(_GraphicsValues.BoardCellSingleValueInvalidSelected), p1.X, p1.Y, cellWidth, cellHeight);
                                            cellDrawn = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (game.Board[i][j] == selectedCell)
                            {
                                if (game.Board[i][j].ValuesCount <= 1)
                                {
                                    g.FillRectangle(new SolidBrush(_GraphicsValues.BoardCellSelected), p1.X, p1.Y, cellWidth, cellHeight);
                                    cellDrawn = true;
                                }
                                else
                                {
                                    g.FillRectangle(new SolidBrush(_GraphicsValues.BoardCellMultiValue), p1.X, p1.Y, cellWidth, cellHeight);
                                    cellDrawn = true;
                                }
                            }
                        }
                    }
                    if(!cellDrawn)
                    {
                        if (game.Board[i][j].IsProtected)
                        {
                            if (!invalidValues.Contains(game.Board[i][j].SingleValue))
                            {
                                g.FillRectangle(new SolidBrush(_GraphicsValues.BoardCellProtected), p1.X, p1.Y, cellWidth, cellHeight);
                            }
                            else
                            {
                                g.FillRectangle(new SolidBrush(_GraphicsValues.BoardCellProtectedInvalid), p1.X, p1.Y, cellWidth, cellHeight);
                            }                            
                        }
                        else
                        {
                            if (game.Board[i][j].ValuesCount == 1)
                            {
                                //if (eng.CheckMoveValidity(i, j, eng.Board[i][j].GetValues()[0]))
                                if (!invalidValues.Contains(game.Board[i][j].SingleValue))
                                {
                                    g.FillRectangle(new SolidBrush(_GraphicsValues.BoardCellSingleValueValid), p1.X, p1.Y, cellWidth, cellHeight);
                                }
                                else
                                {
                                    g.FillRectangle(new SolidBrush(_GraphicsValues.BoardCellSingleValueInvalid), p1.X, p1.Y, cellWidth, cellHeight);
                                }
                            }
                            else
                            {
                                g.FillRectangle(new SolidBrush(_GraphicsValues.BoardCellEmpty), p1.X, p1.Y, cellWidth, cellHeight);
                            }
                        }
                    }

                    //Draw cell values
                    if (game.Board[i][j].ValuesCount == 1)
                    {
                        //Single Value
                        Int32 fSize = Convert.ToInt32(0.7 * Convert.ToDouble(cellHeight));
                        FontStyle fStyle = game.Board[i][j].IsProtected ? FontStyle.Bold : FontStyle.Regular;
                        Color fColor = game.Board[i][j].IsProtected ? _GraphicsValues.BoardSingleValueProtected : _GraphicsValues.BoardSingleValue;
                        Font f = new Font(_GraphicsValues.BoardCellFontFamily, fSize, fStyle);
                        p2.X = (p1.X + (cellWidth - Convert.ToInt32(g.MeasureString(game.Board[i][j].SingleValue.ToString(), f).Width)) / 2);
                        p2.Y = (p1.Y + (cellHeight - Convert.ToInt32(g.MeasureString(game.Board[i][j].SingleValue.ToString(), f).Height)) / 2);
                        //p2.X = (p1.X + (cellWidth - fSize) / 2);
                        //p2.Y = (p1.Y + (cellHeight - fSize) / 2);
                        g.DrawString(game.Board[i][j].SingleValue.ToString(), f, new SolidBrush(fColor), p2);
                    }
                    else if(game.Board[i][j].ValuesCount>1)
                    {
                        //Multiple values
                        Int32 fSize = Convert.ToInt32(0.27 * Convert.ToDouble(cellHeight));
                        FontStyle fStyle = FontStyle.Regular;
                        Color fColor = _GraphicsValues.BoardSingleValue;
                        Font f = new Font(_GraphicsValues.BoardCellFontFamily, fSize, fStyle);
                        while (f.GetHeight() > fSize)
                        {
                            f = new Font(_GraphicsValues.BoardCellFontFamily, f.Size - 1, fStyle);
                        }
                        Int32 hGap = (cellWidth - Convert.ToInt32(3 * g.MeasureString("9", f).Width)) / 4;// Convert.ToInt32(0.05 * Convert.ToDouble(cellWidth));
                        Int32 vGap = (cellHeight - Convert.ToInt32(3 * g.MeasureString("9", f).Height)) / 4;// Convert.ToInt32(0.05 * Convert.ToDouble(cellHeight));

                        foreach (Int32 val in game.Board[i][j].Values)
                        {
                            switch (val)
                            {
                                case 1:
                                    p2.X = (p1.X + hGap);
                                    p2.Y = (p1.Y + vGap);
                                   break;
                                case 2:
                                   p2.X = (p1.X + 2 * hGap + Convert.ToInt32(g.MeasureString("1", f).Width));
                                    p2.Y = (p1.Y + vGap);
                                    break;
                                case 3:
                                    p2.X = (p1.X + 3 * hGap + Convert.ToInt32(g.MeasureString("1", f).Width) + Convert.ToInt32(g.MeasureString("2", f).Width));
                                    p2.Y = (p1.Y + vGap);
                                    break;
                                case 4:
                                    p2.X = (p1.X + hGap);
                                    p2.Y = (p1.Y + 2 * vGap + Convert.ToInt32(g.MeasureString("1", f).Height));
                                    break;
                                case 5:
                                   p2.X = (p1.X + 2 * hGap + Convert.ToInt32(g.MeasureString("1", f).Width));
                                    p2.Y = (p1.Y + 2 * vGap + Convert.ToInt32(g.MeasureString("1", f).Height));
                                    break;
                                case 6:
                                     p2.X = (p1.X + 3 * hGap + Convert.ToInt32(g.MeasureString("1", f).Width) + Convert.ToInt32(g.MeasureString("2", f).Width));
                                     p2.Y = (p1.Y + 2 * vGap + Convert.ToInt32(g.MeasureString("1", f).Height));
                                  break;
                                case 7:
                                  p2.X = (p1.X + hGap);
                                  p2.Y = (p1.Y + 3 * vGap + Convert.ToInt32(g.MeasureString("1", f).Height) + Convert.ToInt32(g.MeasureString("4", f).Height));
                                   break;
                                case 8:
                                   p2.X = (p1.X + 2 * hGap + Convert.ToInt32(g.MeasureString("1", f).Width));
                                   p2.Y = (p1.Y + 3 * vGap + Convert.ToInt32(g.MeasureString("1", f).Height) + Convert.ToInt32(g.MeasureString("4", f).Height));
                                   break;
                                case 9:
                                   p2.X = (p1.X + 3 * hGap + Convert.ToInt32(g.MeasureString("1", f).Width) + Convert.ToInt32(g.MeasureString("2", f).Width));
                                   p2.Y = (p1.Y + 3 * vGap + Convert.ToInt32(g.MeasureString("1", f).Height) + Convert.ToInt32(g.MeasureString("4", f).Height));
                                    break;
                            }
                            g.DrawString(val.ToString(), f, new SolidBrush(fColor), p2);

                        }

                    }

                }
            }

            //Draw lines
            //Draw thick lines
            //Horizontal lines
            p1.X = 0;
            p1.Y = 0;
            p2.X = 9 * cellWidth + 4 * _GraphicsValues.BoardThickLineWidth + 6 * _GraphicsValues.BoardThinLineWidth - 1;// bitmapSize.Width;
            p2.Y = 0;
            for (Int32 w = 0; w < _GraphicsValues.BoardThickLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThickLineColor, 1), p1.X, p1.Y + w, p2.X, p2.Y + w);
            }
            p1.Y = _GraphicsValues.BoardThickLineWidth + 2 * _GraphicsValues.BoardThinLineWidth + 3 * cellHeight;
            p2.Y = p1.Y;
            for (Int32 w = 0; w < _GraphicsValues.BoardThickLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThickLineColor, 1), p1.X, p1.Y + w, p2.X, p2.Y + w);
            }
            p1.Y = 2 * _GraphicsValues.BoardThickLineWidth + 4 * _GraphicsValues.BoardThinLineWidth + 6 * cellHeight;
            p2.Y = p1.Y;
            for (Int32 w = 0; w < _GraphicsValues.BoardThickLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThickLineColor, 1), p1.X, p1.Y + w, p2.X, p2.Y + w);
            }
            p1.Y = 3 * _GraphicsValues.BoardThickLineWidth + 6 * _GraphicsValues.BoardThinLineWidth + 9 * cellHeight;
            p2.Y = p1.Y;
            for (Int32 w = 0; w < _GraphicsValues.BoardThickLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThickLineColor, 1), p1.X, p1.Y + w, p2.X, p2.Y + w);
            }
            
            //Vertical lines
            p1.X = 0;
            p1.Y = 0;
            p2.X = 0;
            p2.Y = 9 * cellHeight + 4 * _GraphicsValues.BoardThickLineWidth + 6 * _GraphicsValues.BoardThinLineWidth - 1;//bitmapSize.Height;
            for (Int32 w = 0; w < _GraphicsValues.BoardThickLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThickLineColor, 1), p1.X + w, p1.Y, p2.X + w, p2.Y);
            }
            p1.X = _GraphicsValues.BoardThickLineWidth + 2 * _GraphicsValues.BoardThinLineWidth + 3 * cellWidth;
            p2.X = p1.X;
            for (Int32 w = 0; w < _GraphicsValues.BoardThickLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThickLineColor, 1), p1.X + w, p1.Y, p2.X + w, p2.Y);
            }
            p1.X = 2 * _GraphicsValues.BoardThickLineWidth + 4 * _GraphicsValues.BoardThinLineWidth + 6 * cellWidth;
            p2.X = p1.X;
            for (Int32 w = 0; w < _GraphicsValues.BoardThickLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThickLineColor, 1), p1.X + w, p1.Y, p2.X + w, p2.Y);
            }
            p1.X = 3 * _GraphicsValues.BoardThickLineWidth + 6 * _GraphicsValues.BoardThinLineWidth + 9 * cellWidth;
            p2.X = p1.X;
            for (Int32 w = 0; w < _GraphicsValues.BoardThickLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThickLineColor, 1), p1.X + w, p1.Y, p2.X + w, p2.Y);
            }

            //Draw thin lines
            //Horizontal lines
            p1.X = 0;
            p1.Y = _GraphicsValues.BoardThickLineWidth + cellHeight;
            p2.X = 9 * cellWidth + 4 * _GraphicsValues.BoardThickLineWidth + 6 * _GraphicsValues.BoardThinLineWidth - 1;// bitmapSize.Width;
            p2.Y = p1.Y;
            for (Int32 w = 0; w < _GraphicsValues.BoardThinLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThinLineColor, 1), p1.X, p1.Y + w, p2.X, p2.Y + w);
            }
            p1.Y = _GraphicsValues.BoardThickLineWidth + _GraphicsValues.BoardThinLineWidth + 2 * cellHeight;
            p2.Y = p1.Y;
            for (Int32 w = 0; w < _GraphicsValues.BoardThinLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThinLineColor, 1), p1.X, p1.Y + w, p2.X, p2.Y + w);
            }
            p1.Y = 2 * _GraphicsValues.BoardThickLineWidth + 2 * _GraphicsValues.BoardThinLineWidth + 4 * cellHeight;
            p2.Y = p1.Y;
            for (Int32 w = 0; w < _GraphicsValues.BoardThinLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThinLineColor, 1), p1.X, p1.Y + w, p2.X, p2.Y + w);
            }
            p1.Y = 2 * _GraphicsValues.BoardThickLineWidth + 3 * _GraphicsValues.BoardThinLineWidth + 5 * cellHeight;
            p2.Y = p1.Y;
            for (Int32 w = 0; w < _GraphicsValues.BoardThinLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThinLineColor, 1), p1.X, p1.Y + w, p2.X, p2.Y + w);
            }
            p1.Y = 3 * _GraphicsValues.BoardThickLineWidth + 4 * _GraphicsValues.BoardThinLineWidth + 7 * cellHeight;
            p2.Y = p1.Y;
            for (Int32 w = 0; w < _GraphicsValues.BoardThinLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThinLineColor, 1), p1.X, p1.Y + w, p2.X, p2.Y + w);
            }
            p1.Y = 3 * _GraphicsValues.BoardThickLineWidth + 5 * _GraphicsValues.BoardThinLineWidth + 8 * cellHeight;
            p2.Y = p1.Y;
            for (Int32 w = 0; w < _GraphicsValues.BoardThinLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThinLineColor, 1), p1.X, p1.Y + w, p2.X, p2.Y + w);
            }

            //Vertical lines
            p1.X = _GraphicsValues.BoardThickLineWidth + cellWidth;
            p1.Y = 0;
            p2.X = p1.X;
            p2.Y = 9 * cellHeight + 4 * _GraphicsValues.BoardThickLineWidth + 6 * _GraphicsValues.BoardThinLineWidth - 1;//bitmapSize.Height;
            for (Int32 w = 0; w < _GraphicsValues.BoardThinLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThinLineColor, 1), p1.X + w, p1.Y, p2.X + w, p2.Y);
            }
            p1.X = _GraphicsValues.BoardThickLineWidth + _GraphicsValues.BoardThinLineWidth + 2 * cellWidth;
            p2.X = p1.X;
            for (Int32 w = 0; w < _GraphicsValues.BoardThinLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThinLineColor, 1), p1.X + w, p1.Y, p2.X + w, p2.Y);
            }
            p1.X = 2 * _GraphicsValues.BoardThickLineWidth + 2 * _GraphicsValues.BoardThinLineWidth + 4 * cellWidth;
            p2.X = p1.X;
            for (Int32 w = 0; w < _GraphicsValues.BoardThinLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThinLineColor, 1), p1.X + w, p1.Y, p2.X + w, p2.Y);
            }
            p1.X = 2 * _GraphicsValues.BoardThickLineWidth + 3 * _GraphicsValues.BoardThinLineWidth + 5 * cellWidth;
            p2.X = p1.X;
            for (Int32 w = 0; w < _GraphicsValues.BoardThinLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThinLineColor, 1), p1.X + w, p1.Y, p2.X + w, p2.Y);
            }
            p1.X = 3 * _GraphicsValues.BoardThickLineWidth + 4 * _GraphicsValues.BoardThinLineWidth + 7 * cellWidth;
            p2.X = p1.X;
            for (Int32 w = 0; w < _GraphicsValues.BoardThinLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThinLineColor, 1), p1.X + w, p1.Y, p2.X + w, p2.Y);
            }
            p1.X = 3 * _GraphicsValues.BoardThickLineWidth + 5 * _GraphicsValues.BoardThinLineWidth + 8 * cellWidth;
            p2.X = p1.X;
            for (Int32 w = 0; w < _GraphicsValues.BoardThinLineWidth; w++)
            {
                g.DrawLine(new Pen(_GraphicsValues.BoardThinLineColor, 1), p1.X + w, p1.Y, p2.X + w, p2.Y);
            }

            return bmpBoard;
        }

        public Tuple<gSudokuCell, Int32, Int32> GetSelectedCell(gSudokuGame game, Size bitmapSize, Int32 mouseX, Int32 mouseY)
        {
            //Get the selected cell
            Int32 cellWidth = CellWidth(bitmapSize);
            Int32 cellHeight = CellHeight(bitmapSize);
            Int32 x = -1, y = -1;
            Int32 tmpX = 0, tmpY = 0;
            gSudokuCell myCell = null;
            Int32 myCellX = -1, myCellY = -1;

            tmpY = _GraphicsValues.BoardThickLineWidth + cellWidth;
            if (mouseX < tmpY)
            {
                y = 0;
            }
            if (y < 0)
            {
                tmpY += _GraphicsValues.BoardThinLineWidth + cellWidth;
                if (mouseX < tmpY)
                {
                    y = 1;
                }
                if (y < 0)
                {
                    tmpY += _GraphicsValues.BoardThinLineWidth + cellWidth;
                    if (mouseX < tmpY)
                    {
                        y = 2;
                    }
                    if (y < 0)
                    {
                        tmpY += _GraphicsValues.BoardThickLineWidth + cellWidth;
                        if (mouseX < tmpY)
                        {
                            y = 3;
                        }
                        if (y < 0)
                        {
                            tmpY += _GraphicsValues.BoardThinLineWidth + cellWidth;
                            if (mouseX < tmpY)
                            {
                                y = 4;
                            }
                            if (y < 0)
                            {
                                tmpY += _GraphicsValues.BoardThinLineWidth + cellWidth;
                                if (mouseX < tmpY)
                                {
                                    y = 5;
                                }
                                if (y < 0)
                                {
                                    tmpY += _GraphicsValues.BoardThickLineWidth + cellWidth;
                                    if (mouseX < tmpY)
                                    {
                                        y = 6;
                                    }
                                    if (y < 0)
                                    {
                                        tmpY += _GraphicsValues.BoardThinLineWidth + cellWidth;
                                        if (mouseX < tmpY)
                                        {
                                            y = 7;
                                        }
                                        if (y < 0)
                                        {
                                            tmpY += _GraphicsValues.BoardThinLineWidth + cellWidth;
                                            if (mouseX < tmpY)
                                            {
                                                y = 8;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            tmpX = _GraphicsValues.BoardThickLineWidth + cellHeight;
            if (mouseY < tmpX)
            {
                x = 0;
            }
            if (x < 0)
            {
                tmpX += _GraphicsValues.BoardThinLineWidth + cellHeight;
                if (mouseY < tmpX)
                {
                    x = 1;
                }
                if (x < 0)
                {
                    tmpX += _GraphicsValues.BoardThinLineWidth + cellHeight;
                    if (mouseY < tmpX)
                    {
                        x = 2;
                    }
                    if (x < 0)
                    {
                        tmpX += _GraphicsValues.BoardThickLineWidth + cellHeight;
                        if (mouseY < tmpX)
                        {
                            x = 3;
                        }
                        if (x < 0)
                        {
                            tmpX += _GraphicsValues.BoardThinLineWidth + cellHeight;
                            if (mouseY < tmpX)
                            {
                                x = 4;
                            }
                            if (x < 0)
                            {
                                tmpX += _GraphicsValues.BoardThinLineWidth + cellHeight;
                                if (mouseY < tmpX)
                                {
                                    x = 5;
                                }
                                if (x < 0)
                                {
                                    tmpX += _GraphicsValues.BoardThickLineWidth + cellHeight;
                                    if (mouseY < tmpX)
                                    {
                                        x = 6;
                                    }
                                    if (x < 0)
                                    {
                                        tmpX += _GraphicsValues.BoardThinLineWidth + cellHeight;
                                        if (mouseY < tmpX)
                                        {
                                            x = 7;
                                        }
                                        if (x < 0)
                                        {
                                            tmpX += _GraphicsValues.BoardThinLineWidth + cellHeight;
                                            if (mouseY < tmpX)
                                            {
                                                x = 8;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (x > -1 && y > -1)
            {
                myCell = game.Board[x][y];
                myCellX = x;
                myCellY = y;
            }
            else
            {
                myCell = null;
                myCellX = -1;
                myCellY = -1;
            }

            return new Tuple<gSudokuCell, Int32, Int32>(myCell, myCellX, myCellY);
        }
    }
}
