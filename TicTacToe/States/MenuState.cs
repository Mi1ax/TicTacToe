using System;
using System.Collections.Generic;
using DymanLibrary.Graphics;
using DymanLibrary.Graphics.GUI;
using DymanLibrary.Graphics.GUI.Components;
using DymanLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TicTacToe.States
{
    public class MenuState : State
    {
        private readonly List<Drawable> _components;
        
        public MenuState(Game1 game, Screen screen, GraphicsDevice graphicsDevice, ContentManager content) 
            : base(game, screen, graphicsDevice, content)
        {
            var Font = content.Load<SpriteFont>("Font");
            var screenCenter = new Vector2((float)_screen.Width / 2, (float)_screen.Height / 2);

            var Text = new Text(Font, "Tic Tac Toe", 2f)
            {
                Position = new Vector2(screenCenter.X, screenCenter.Y + 150)
            };

            
            var startButton = new Button(Font, _screen)
            {
                Position = new Vector2((int)screenCenter.X, (int)screenCenter.Y - 70 / 2),
                Size = new Vector2(150, 70),
                Text = "Start"
            };
            startButton.Click += StartButton_Click;
            
            var exitButton = new Button(Font, _screen)
            {
                Position = new Vector2((int)screenCenter.X, (int)screenCenter.Y - 70 / 2 - 85),
                Size = new Vector2(150, 70),
                Text = "Exit"
            };
            exitButton.Click += ExitButton_Click;

            _components = new List<Drawable>
            {
                Text,
                startButton,
                exitButton
            };
        }

        public override void DrawSprite(GameTime gameTime, Sprites spriteBatch)
        {
            foreach (var component in _components) 
                component.DrawSprite(gameTime, spriteBatch);
            
        }

        public override void DrawShape(GameTime gameTime, Shapes shapes)
        {
            foreach (var component in _components) 
                component.DrawShape(gameTime, shapes);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            var game = (Game1)_game;
            game.ChangeState(new GameState(game, _screen, _graphicsDevice, _content));
        }
        
        public override void Update(GameTime gameTime, FlatKeyboard keyboard, FlatMouse mouse)
        {
            foreach (var component in _components)
                component.Update(gameTime, mouse, keyboard);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }
    }
}