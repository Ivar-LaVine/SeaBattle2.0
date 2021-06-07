using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
    public class BattleField : Control
    {
        /// <summary>
        /// Battlefield Constructor
        /// </summary>
        private int mapSize = 10;
        private int cellSize = 30;
        private int scoreNeed = 20; // 
        private bool isPlacement;
        private int[,] myMap;
        private int[,] enemyMap;
        private bool isGameStart;
        private bool turn = true;
        private bool orientation = true;
        private List<BattleShip> battleShips = new List<BattleShip>();
        private List<BattleShip> enemyBattleShips = new List<BattleShip>();
        //Foreach
        private int index = 0;
        private int[] shipsLengthArray = new int[] { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
        public int[,] MyMap
        {
            get
            {
                return myMap;
            }
        }
        public int[,] EnemyMap
        {
            get
            {
                return enemyMap;
            }
        }
        private string alphabet = "АБВГДЕЖЗИКЛV";
        private Font font = new Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point);
        public BattleField()
        {
            myMap = new int[mapSize, mapSize];
            enemyMap = new int[mapSize, mapSize];
            FillMap();
            isPlacement = true;
            CreateBattleShips();
            StartAIPlacement();
            isGameStart = false;
        }
        private void CreateBattleShips()
        {
            for (int i = 0; i < shipsLengthArray.Count(); i++)
            {
                battleShips.Add(new BattleShip(shipsLengthArray[i], this));
                enemyBattleShips.Add(new BattleShip(shipsLengthArray[i], this));
            }
        }

        private void FillMap()
        {
            var rand = new Random();
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    myMap[i, j] = 0;
                    // AIMap Random Generate
                    enemyMap[i, j] = 0;
                }
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            for (int i = 0; i <= mapSize; i++)
            {
                for (int j = 0; j <= mapSize * 2 + 3; j++)
                {
                    Rectangle rectangle = new Rectangle()
                    {
                        Location = new Point(cellSize * j, cellSize * i),
                        Size = new Size(cellSize, cellSize)
                    };
                    if (!(j == mapSize + 1 || j == mapSize + 2))
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.AliceBlue), rectangle);
                        e.Graphics.DrawRectangle(new Pen(Color.Black), rectangle);
                    }
                    if (i == 0 && j > 0 && j <= mapSize)
                    {
                        e.Graphics.DrawString(alphabet[j - 1].ToString(), font, new SolidBrush(Color.Black), new Point(cellSize * j + 9, cellSize * i + 9));
                    }
                    else if (i == 0 && j > mapSize + 3 && j <= mapSize * 2 + 3)
                    {
                        e.Graphics.DrawString(alphabet[j - 1 - mapSize - 3].ToString(), font, new SolidBrush(Color.Black), new Point(cellSize * j + 9, cellSize * i + 9));
                    }
                    else if (i > 0 && j == 0 || i > 0 && j == mapSize + 3)
                    {
                        if (i == 10)
                        {
                            e.Graphics.DrawString(i.ToString(), font, new SolidBrush(Color.Black), new Point(cellSize * j + 5, cellSize * i + 9));
                        }
                        else
                        {
                            e.Graphics.DrawString(i.ToString(), font, new SolidBrush(Color.Black), new Point(cellSize * j + 9, cellSize * i + 9));
                        }
                    }
                }
            }
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    Rectangle rectangle = new Rectangle()
                    {
                        Location = new Point(cellSize + cellSize * j, cellSize + cellSize * i),
                        Size = new Size(cellSize, cellSize)
                    };
                    if (myMap[i, j] == 1)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.YellowGreen), rectangle);
                    }
                    if (myMap[i, j] == 1)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.YellowGreen), rectangle);
                    }
                    if (enemyMap[i, j] == 3)
                    {
                        e.Graphics.DrawLine(new Pen(Color.Red, 2), i + cellSize, j * cellSize, (i + 1) * cellSize, (j + 1) * cellSize);
                        e.Graphics.DrawLine(new Pen(Color.Red, 2), 1, 10, 10, 1);
                    }
                    //else if (myMap[i, j] == 2)
                    //{
                    //    e.Graphics.FillRectangle(new SolidBrush(Color.Red), rectangle);
                    //}
                }
            }
            foreach (BattleShip ship in battleShips)
            {
                if (ship.Orientation == true)
                {
                    if ((ship.Column >= 0) && (ship.Row >= 0))
                        //for (int i = ship.Column; i < ship.Column + ship.Length; i++)
                        e.Graphics.FillEllipse(new SolidBrush(Color.CadetBlue), cellSize + ship.Column * cellSize, cellSize + ship.Row * cellSize, cellSize + (ship.Length - 1) * cellSize, cellSize);
                }
                else
                {
                    if ((ship.Column >= 0) && (ship.Row >= 0))
                        //for (int i = ship.Row; i < ship.Row + ship.Length; i++)
                        e.Graphics.FillEllipse(new SolidBrush(Color.CadetBlue), cellSize + ship.Column * cellSize, cellSize + ship.Row * cellSize, cellSize, cellSize + (ship.Length - 1) * cellSize);
                }
            }
            foreach (BattleShip enemyShip in enemyBattleShips)
            {
                if (enemyShip.Orientation == true)
                {
                    if ((enemyShip.Column >= 0) && (enemyShip.Row >= 0))
                        //for (int i = ship.Column; i < ship.Column + ship.Length; i++)
                        e.Graphics.FillEllipse(new SolidBrush(Color.CadetBlue), cellSize + enemyShip.Column * cellSize + cellSize * (mapSize + 3), cellSize + enemyShip.Row * cellSize, cellSize + (enemyShip.Length - 1) * cellSize, cellSize);
                }
                else
                {
                    if ((enemyShip.Column >= 0) && (enemyShip.Row >= 0))
                        //for (int i = ship.Row; i < ship.Row + ship.Length; i++)
                        e.Graphics.FillEllipse(new SolidBrush(Color.CadetBlue), (cellSize + enemyShip.Column * cellSize) + cellSize * (mapSize + 3), cellSize + enemyShip.Row * cellSize, cellSize, cellSize + (enemyShip.Length - 1) * cellSize);
                }
            }
            UpdateMap();
        }
        private void UpdateMap()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (myMap[i, j] == 1)
                    {
                        myMap[i, j] = 0;
                    }
                }
            }
        }
        private void StartAIPlacement()
        {
            int k = 0;
            while (k < 10)
            {
                BattleShip enemyShip = enemyBattleShips[k];
                Random random = new Random();
                int buf = random.Next(0, 2);
                if (buf == 1)
                {
                    orientation = true;
                }
                else
                {
                    orientation = false;
                }
                int x = random.Next(1, mapSize);
                int y = random.Next(1, mapSize);
                if (orientation)
                {
                    enemyShip.Orientation = orientation;
                    enemyShip.SetCoordinatesAI(y, x);
                    if (enemyShip.Column == x && enemyShip.Row == y)
                    {
                        k++;
                        for (int i = 0; i < enemyShip.Length; i++)
                        {
                            enemyMap[enemyShip.Row, enemyShip.Column + i] = 2;
                        }
                    }


                }
                else
                {
                    enemyShip.Orientation = orientation;
                    enemyShip.SetCoordinatesAI(y, x);
                    if (enemyShip.Column == x && enemyShip.Row == y)
                    {
                        k++;
                        for (int i = 0; i < enemyShip.Length; i++)
                        {
                            enemyMap[enemyShip.Row + i, enemyShip.Column] = 2;
                        }
                    }
                }
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (isPlacement)
            {
                BattleShip ship = battleShips[index];
                int x = e.X / cellSize - 1;
                int y = e.Y / cellSize - 1;
                if (e.X > cellSize && e.X < cellSize * mapSize + cellSize && e.Y > cellSize && e.Y < cellSize * mapSize + cellSize)
                {
                    for (int i = 0; i < ship.Length; i++)
                    {
                        try
                        {
                            if (orientation && (myMap[y, x + i] != 2) && x + ship.Length - 1 <= 9) myMap[y, x + i] = 1;
                            else if (!orientation && (myMap[y + i, x] != 2) && y + ship.Length - 1 <= 9) myMap[y + i, x] = 1;
                        }
                        catch (IndexOutOfRangeException) { }
                    }
                }
                Invalidate();
            }
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (isPlacement)
            {
                BattleShip ship = battleShips[index];
                int x = e.X / cellSize - 1;
                int y = e.Y / cellSize - 1;
                if (e.X > cellSize && e.X < cellSize * mapSize + cellSize && e.Y > cellSize && e.Y < cellSize * mapSize + cellSize)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        try
                        {
                            if (orientation)
                            {
                                ship.Orientation = orientation;
                                ship.SetCoordinates(y, x);
                                if ((ship.Column == x && ship.Row == y))
                                {

                                    index++;
                                    for (int i = 0; i < ship.Length; i++)
                                    {
                                        myMap[ship.Row, ship.Column + i] = 2;
                                    }
                                }


                            }
                            else
                            {
                                ship.Orientation = orientation;
                                ship.SetCoordinates(y, x);
                                if (ship.Column == x && ship.Row == y)
                                {
                                    index++;
                                    for (int i = 0; i < ship.Length; i++)
                                    {
                                        myMap[ship.Row + i, ship.Column] = 2;
                                    }
                                }
                            }
                        }
                        catch (IndexOutOfRangeException) { }
                        if (index == 10)
                        {
                            isPlacement = false;
                            isGameStart = true;
                        }
                    }
                    else if (e.Button == MouseButtons.Right) orientation = !orientation;
                    Invalidate();
                }
            }
            else if (isGameStart)
            {
                int x = (e.X / cellSize - 1) - mapSize - 3;
                int y = e.Y / cellSize - 1;
                
                if (turn)
                {
                    try
                    {
                        enemyMap[x, y] = 3;
                    }
                    catch (IndexOutOfRangeException) { }
                }
                Invalidate();
            }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
    }
}
