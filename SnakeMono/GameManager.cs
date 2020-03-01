using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SnakeMono
{
    public class GameManager : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _headTexture, _tailTexture, _fruitTexture, _frameTexture;
        private SpriteFont _font;
        private Vector2 _lostPos;


        private Input _input;
        private Timer _timer;
        private Random rng;
        private Rectangle[] _frames;

        private Vector2 _fruit;
        private Snake _snake;
        private bool _end = false;

        public GameManager()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _snake = new Snake();
            _fruit = new Vector2(0,0);
            rng = new Random();
            _frames = new Rectangle[4];
            _frames[0] = new Rectangle(0,0,Constants.FieldWidth * Constants.WindowSize,Constants.FieldHeight);
            _frames[1] = new Rectangle(0,0,Constants.FieldWidth,Constants.FieldHeight * Constants.WindowSize);
            _frames[2] = new Rectangle(Constants.FieldWidth * (Constants.WindowSize-1),0,Constants.FieldWidth,Constants.FieldHeight * Constants.WindowSize);
            _frames[3] = new Rectangle(0,Constants.FieldHeight * (Constants.WindowSize - 1),Constants.FieldWidth * Constants.WindowSize,Constants.FieldHeight);
            _input = new Input();
            _timer = new Timer();
            _lostPos = new Vector2(100,100);
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = Constants.WindowSize * Constants.FieldWidth;
            _graphics.PreferredBackBufferHeight = Constants.WindowSize * Constants.FieldHeight;
            _graphics.ApplyChanges();
            _font = Content.Load<SpriteFont>("ArialFont");
            _input.Update();
            MoveFruit();
            base.Initialize();
        }

        private Texture2D MakeTexture(Color color, int width, int height)
        {
            Color[] colorData = new Color[width * height];
            for (int i = 0; i < Constants.FieldWidth * Constants.FieldHeight; i++)
                colorData[i] = color;
            var texture = new Texture2D(this.GraphicsDevice,width,height);
            texture.SetData<Color>(colorData);
            return texture;
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _headTexture = this.Content.Load<Texture2D>("Aoba");
            _tailTexture = MakeTexture(Color.YellowGreen, Constants.FieldWidth, Constants.FieldHeight);
            _fruitTexture = MakeTexture(Color.Aquamarine, Constants.FieldWidth, Constants.FieldHeight);
            _frameTexture = MakeTexture(Color.Black, Constants.FieldWidth, Constants.FieldHeight);
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                _input.Update();
                _timer.Update(gameTime);
                if (_input.KeyDown(Keys.Escape))
                        Exit();
                if (_input.KeyPress(Keys.A))
                        _snake.SetDirection(Snake.Direction.Left);
                if (_input.KeyPress(Keys.S))
                        _snake.SetDirection(Snake.Direction.Down);
                if (_input.KeyPress(Keys.D))
                    _snake.SetDirection(Snake.Direction.Right);
                if (_input.KeyPress(Keys.W))
                    _snake.SetDirection(Snake.Direction.Up);
                if (_timer.IsTimeUp() && !_end)
                {
                    if (_snake.NextPosition() == _fruit)
                    {
                        _snake.Grow();
                        MoveFruit();
                    }
                    else
                    {
                        if (_snake.WillEatHimself() || OutOfBonds())
                        {
                            _end = true;
                            Window.Title = "U SUCK XDDDD";
                        }
                        else
                        {
                            _snake.Move();
                        }
                    }
                }

                base.Update(gameTime);
            }
        }

        private void DrawSnake()
        {
            foreach (var pos in _snake.Body)
            {
                _spriteBatch.Draw(_tailTexture,pos,Color.White);
            }
            _spriteBatch.Draw(_headTexture,_snake.Body.Last.Value,Color.Red);
        }

        private void DrawFrame()
        {
            foreach (var frame in _frames)
            {
                _spriteBatch.Draw(_frameTexture,frame,Color.White);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();
            DrawSnake();
            DrawFrame();
            _spriteBatch.Draw(_fruitTexture,_fruit,Color.White);
            if (_end) _spriteBatch.DrawString(_font, "U lost!", _lostPos, Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
        
        private bool OutOfBonds()
        {
            var next = _snake.NextPosition();
            if (next.X < Constants.FieldWidth || next.X >= Constants.FieldWidth * (Constants.WindowSize - 1)) return true;
            return next.Y < Constants.FieldHeight || next.Y >= Constants.FieldHeight * (Constants.WindowSize - 1);
        }
        private void MoveFruit()
        {
            _fruit.X = rng.Next(1, Constants.WindowSize-1) * Constants.FieldWidth;
            _fruit.Y = rng.Next(1, Constants.WindowSize-1) * Constants.FieldHeight;
        }
        
    }
}