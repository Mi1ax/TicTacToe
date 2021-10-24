using DymanLibrary.Graphics;
using DymanLibrary.Input;
using Microsoft.Xna.Framework;
using TicTacToe.States;

namespace TicTacToe
{
    public class Game1 : Game
    {
        public const int ScreenWidth = 1280;
        public const int ScreenHeight = 720;
        public const int ScreenScale = 3;

        private readonly GraphicsDeviceManager _graphics;
        private Sprites _sprites;
        private Shapes _shapes;
        private Screen _screen;
        
        private State _currentState;
        private State _nextState;

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.ApplyChanges();

            _sprites = new Sprites(this);
            _shapes = new Shapes(this);
            _screen = new Screen(this, ScreenWidth, ScreenHeight);

            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _currentState = new MenuState(this, _screen, _graphics.GraphicsDevice, Content);
            //_currentState = new GameState(this, _screen, _graphics.GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            var mouse = FlatMouse.Instance;
            mouse.Update();
            var keyboard = FlatKeyboard.Instance;
            keyboard.Update();
            
            if (_nextState != null)
            {
                _currentState = _nextState;
                _nextState = null;
            }
            
            _currentState.Update(gameTime, keyboard, mouse);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _screen.Set();
            GraphicsDevice.Clear(Color.Black);
            
            _sprites.Begin(null, false);
            _currentState.DrawSprite(gameTime, _sprites);
            _sprites.End();

            _shapes.Begin(null);
            _currentState.DrawShape(gameTime, _shapes);
            _shapes.End();

            _screen.UnSet();
            _screen.Present(_sprites, false);
            base.Draw(gameTime);
        }
    }
}