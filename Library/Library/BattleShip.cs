using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class BattleShip
    {
        public BattleShip(int Length, BattleField BattleField)
        {
            Status = new int[Length];
            for (int i = 0; i < Length; i++)
            {
                Status[i] = 0;
            }
            _battleField = BattleField;
            Row = -1;
            Column = -1;
            Orientation = true;
        }
        private BattleField _battleField;
        //private int _row;
        //private int _column;
        //private int _status;
        public int Row { get; set; }
        public int Length => Status.GetLength(0); // { get { return Status.GetLength(0); } }
        public bool Orientation { get; set; }
        public int[] Status { get; set; }
        public int Column { get; set; }
        public void SetCoordinates(int r, int c)
        {
            // Row
            if (r < 0 || r > 9) Row = -1;
            if (c >= 0)
                if ((r >= 0) && CanPut(r, c)) Row = r;
                else Row = -1;
            else Row = r;
            // Column
            if (c < 0 || c > 9) Column = -1;
            if (r >= 0)
                if ((c >= 0) && CanPut(r, c)) Column = c;
                else Column = -1;
            else Column = c;
        }
        public void SetCoordinatesAI(int r, int c)
        {
            // Row
            if (r < 0 || r > 9) Row = -1;
            if (c >= 0)
                if ((r >= 0) && CanPutAI(r, c)) Row = r;
                else Row = -1;
            else Row = r;
            // Column
            if (c < 0 || c > 9) Column = -1;
            if (r >= 0)
                if ((c >= 0) && CanPutAI(r, c)) Column = c;
                else Column = -1;
            else Column = c;
        }
        private bool CanPutAI(int r, int c)
        {
            if (Orientation)
            {
                int bufRow = r - 1;
                while (bufRow <= r + 1)
                {
                    int bufCol = c - 1;
                    while (bufCol <= c + Length)
                    {
                        if ((bufRow >= 0) && (bufRow <= 9) && (bufCol >= 0) && (bufCol <= 9) && (_battleField.EnemyMap[bufRow, bufCol] > 1) && (c + Length - 1 <= 9)) return false;
                        bufCol++;
                    }
                    bufRow++;
                }
                if (c + Length - 1 > 9)
                {
                    return false;
                }
            }
            else
            {
                int bufCol = c - 1;
                while (bufCol <= c + 1)
                {
                    int bufRow = r - 1;
                    while (bufRow <= r + Length)
                    {
                        if ((bufRow >= 0) && (bufRow <= 9) && (bufCol >= 0) && (bufCol <= 9) && (_battleField.EnemyMap[bufRow, bufCol] > 1)) return false;
                        bufRow++;
                    }

                    bufCol++;
                }
                if (r + Length - 1 > 9)
                {
                    return false;
                }
            }
            return true;
        }
        private bool CanPut(int r, int c)
        {
            if (Orientation)
            {
                int bufRow = r - 1;
                while (bufRow <= r + 1)
                {
                    int bufCol = c - 1;
                    while (bufCol <= c + Length)
                    {
                        if ((bufRow >= 0) && (bufRow <= 9) && (bufCol >= 0) && (bufCol <= 9) && (_battleField.MyMap[bufRow, bufCol] > 1) && (c + Length - 1 <= 9)) return false;
                        bufCol++;
                    }
                    bufRow++;
                }
                if (c + Length - 1 > 9)
                {
                    return false;
                }
            }
            else
            {
                int bufCol = c - 1;
                while (bufCol <= c + 1)
                {
                    int bufRow = r - 1;
                    while (bufRow <= r + Length)
                    {
                        if ((bufRow >= 0) && (bufRow <= 9) && (bufCol >= 0) && (bufCol <= 9) && (_battleField.MyMap[bufRow, bufCol] > 1)) return false;
                        bufRow++;
                    }
                    
                    bufCol++;
                }
                if (r + Length - 1 > 9)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
