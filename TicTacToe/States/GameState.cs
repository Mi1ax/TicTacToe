using System.Collections.Generic;
using DymanLibrary.Graphics;
using DymanLibrary.Graphics.GUI.Components;
using DymanLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TicTacToe.States
{
    public class GameState : State
    {
        public static Dictionary<string, Text> _texts;
        private readonly Board _board;
        
        public GameState(Game1 game, Screen screen, GraphicsDevice graphicsDevice, ContentManager content) 
            : base(game, screen, graphicsDevice, content)
        {
            _game = game;

            _board = new Board(32 * Game1.ScreenScale);
            TextInit();
        }

        private void TextInit()
        {
            _texts = new Dictionary<string, Text>();
            var Font = _content.Load<SpriteFont>("Font");
            
            var posTurnTxt = new Vector2(
                (float) Game1.ScreenWidth / 2, 
                (float) Game1.ScreenHeight / 2 + _board.CellWidth * 2
            );
            var posWinnerTxt = new Vector2(
                (float) Game1.ScreenWidth / 2, 
                (float) Game1.ScreenHeight / 2 - _board.CellWidth * 2
            );
            var posWinnerCountTxt = new Vector2(
                (float) Game1.ScreenWidth / 2 - _board.CellWidth * 3, 
                (float) Game1.ScreenHeight / 2
            );

            _texts.Add("TurnText", new Text(Font, "") {
                Position = posTurnTxt
            });
            _texts.Add("Winner", new Text(Font, "")
            {
                Position = posWinnerTxt
            });
            _texts.Add("WinnerCount", new Text(Font, "")
            {
                Position = posWinnerCountTxt
            });
        }

        public override void DrawSprite(GameTime gameTime, Sprites spriteBatch)
        {
            foreach (var (_, text) in _texts) text.DrawSprite(gameTime, spriteBatch);
        }

        public override void DrawShape(GameTime gameTime, Shapes shapes)
        {
            _board.Draw(shapes);
        }

        public override void Update(GameTime gameTime, FlatKeyboard keyboard, FlatMouse mouse)
        {
            if (keyboard.IsKeyClicked(Keys.Escape))
            {
                var game = (Game1)_game;
                game.ChangeState(new MenuState(game, _screen, _graphicsDevice, _content));
            }

            _board.Update(gameTime, _screen, mouse);
        }
    }
}