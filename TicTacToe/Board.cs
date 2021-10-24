using System.Linq;
using DymanLibrary.Graphics;
using DymanLibrary.Input;
using Microsoft.Xna.Framework;
using TicTacToe.States;

namespace TicTacToe
{
    public class Board
    {
        private readonly Cell[,] _cells;
        private readonly int[,,] possibleWins;
        private int[,] winnerLine;
        private bool isGameEnded;
        private bool isGameWinned;
        private int CircleWinCount;
        private int CrossWinCount;
        private bool turn;

        public int CircleWin => CircleWinCount;
        public int CrossWin => CrossWinCount;
        public int CellWidth { get; }
        public bool Turn => turn;
        
        public Board(int cellWidth)
        {
            
            CircleWinCount = 0;
            CrossWinCount = 0;
            
            isGameEnded = false;
            isGameWinned = false;
            _cells = new Cell[3, 3];
            CellWidth = cellWidth;
            ClearBoard();
            winnerLine = new int[3, 2];
            possibleWins = new [,,]
            {
                {{0, 0}, {0, 1}, {0, 2}},
                {{1, 0}, {1, 1}, {1, 2}},
                {{2, 0}, {2, 1}, {2, 2}},
                
                {{0, 0}, {1, 1}, {2, 2}},
                {{0, 2}, {1, 1}, {2, 0}},
                
                {{0, 0}, {1, 0}, {2, 0}},
                {{0, 1}, {1, 1}, {2, 1}},
                {{0, 2}, {1, 2}, {2, 2}},
            };
        }
        
        private bool CheckWin(bool isCircle)
        {
            var count = 0;
            for (var line = 0; line < possibleWins.GetLength(0); line++)
            {
                for (var cell = 0; cell < possibleWins.GetLength(1); cell++)
                {
                    if (_cells[possibleWins[line, cell, 0], possibleWins[line, cell, 1]].isClicked &&
                        _cells[possibleWins[line, cell, 0], possibleWins[line, cell, 1]].isCircle == isCircle)
                    {
                        count++;
                        continue;
                    }
                    count = 0;
                    break;
                }

                if (count == 3)
                {
                    for (var cell = 0; cell < possibleWins.GetLength(1); cell++)
                    {
                        winnerLine[cell, 0] = possibleWins[line, cell, 0];
                        winnerLine[cell, 1] = possibleWins[line, cell, 1];
                    }
                    return true;
                }
            }

            return false;
        }

        private bool isFull()
        {
            return _cells.Cast<Cell>().All(cell => cell.isClicked);
        }

        private void ClearBoard()
        {
            for (var y = 0; y < _cells.GetLength(0); y++)
            {
                for (var x = 0; x < _cells.GetLength(1); x++)
                {
                    _cells[y, x] = new Cell(
                        new Vector2(
                            Game1.ScreenWidth / 2 - CellWidth + x * CellWidth, 
                            Game1.ScreenHeight / 2 - CellWidth + y * CellWidth
                            ),
                        CellWidth);
                }
            }
        }

        public void Update(GameTime gameTime, Screen screen, FlatMouse mouse)
        {
            GameState._texts["TurnText"].Content = Turn ? "Turn: Circle" : "Turn: Cross";
            GameState._texts["WinnerCount"].Content = $"Circle Wins: {CircleWin}\nCross Wins: {CrossWin}";

            if (mouse.IsLeftButtonClicked() && !isGameEnded)
            {
                foreach (var cell in _cells)
                {
                    if (!cell.Click(mouse.GetScreenPosition(screen), turn)) continue;
                    
                    turn = !turn;
                    var isCircleWin = CheckWin(true);
                    var isCrossWin = CheckWin(false);
                    if (isCircleWin)
                    {
                        GameState._texts["Winner"].Content = "Winner: Circle\nPress LMB to continue";
                        CircleWinCount++;
                    }

                    if (isCrossWin)
                    {
                        GameState._texts["Winner"].Content = "Winner: Cross\nPress LMB to continue";
                        CrossWinCount++;
                    }
                    if (isFull()) GameState._texts["Winner"].Content = "Tie\nPress LMB to continue";
                    if (isCircleWin || isCrossWin) isGameWinned = true;
                    if (isCircleWin || isCrossWin || isFull())
                    {
                        isGameEnded = true;
                    }
                }
            } else if (mouse.IsLeftButtonClicked() && isGameEnded)
            {
                ClearBoard();
                GameState._texts["Winner"].Content = "";
                isGameEnded = false;
                isGameWinned = false;
            }
        }

        public void Draw(Shapes shapes)
        {
            foreach (var cell in _cells) cell.Draw(shapes);
            if (isGameWinned)
            {
                var first = _cells[winnerLine[0, 0], winnerLine[0, 1]];
                var last = _cells[winnerLine[2, 0], winnerLine[2, 1]];

                var startPos = new Vector2(first.Position.X, first.Position.Y);
                var endPos = new Vector2(last.Position.X, last.Position.Y);
                
                shapes.DrawLine(startPos, endPos, 8f, Color.Green);
            }
        }
    }
}