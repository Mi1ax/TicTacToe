using DymanLibrary.Graphics;
using Microsoft.Xna.Framework;

namespace TicTacToe
{
    public class Cell
    {
        private readonly int _width;
        private readonly float thickness;
        
        private Vector2 _position;
        private readonly Vector2 _origin;
        
        public bool isClicked;
        public bool isCircle;
        
        private readonly Vector2 _bottomLeft;
        private readonly Vector2 _topRight;

        public Vector2 Position => _position;
        public int Width => _width;
        
        public Cell(Vector2 position, int width)
        {
            _position = position;
            _width = width;
            thickness = 3f;
            _origin = new Vector2((float) width / 2, (float) width / 2);
            
            var (x, y) = _position;
            _bottomLeft = new Vector2(x - _width / 2f, y - _width / 2f);
            _topRight = new Vector2(x + _width / 2f, y + _width / 2f);
            isCircle = true;
        }

        public void Draw(Shapes shapes)
        {
            var (x, y) = _position;
            shapes.DrawRectangle(x - _origin.X, y - _origin.Y, _width, _width, thickness, Color.White);

            if (!isClicked) return;
            if (isCircle) DrawCircle(shapes);
            else DrawCross(shapes);
        }

        private void DrawCross(Shapes shapes)
        {
            var (x, y) = _position;
            shapes.DrawLine(
                new Vector2(_bottomLeft.X + 6f, _bottomLeft.Y + 6f), 
                new Vector2(_topRight.X - 6f, _topRight.Y - 6f), 
                thickness, 
                Color.WhiteSmoke
            );
            shapes.DrawLine(
                new Vector2(x + _width / 2f - 6f, y - _width / 2f + 6f), 
                new Vector2(x - _width / 2f + 6f, y + _width / 2f - 6f), 
                thickness, 
                Color.WhiteSmoke
            );
        }

        private void DrawCircle(Shapes shapes)
        {
            var (x, y) = _position;
            shapes.DrawCircle(x, y, _width / 2f - 6f, 24, thickness, Color.WhiteSmoke);
        }

        public bool Click(Vector2 mousePosition, bool pIsCircle)
        {
            if (!isInCell(mousePosition) || isClicked) return false;
            isClicked = true;
            isCircle = pIsCircle;
            return true;
        }

        private bool isInCell(Vector2 point)
        {
            var (pointX, pointY) = point;

            return (pointX > _bottomLeft.X && pointX < _topRight.X) &&
                   (pointY > _bottomLeft.Y && pointY < _topRight.Y);
        }
    }
}