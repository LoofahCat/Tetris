using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Tetris
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<SoundEffect> soundEffects;
        private SpriteFont font;
        Song theme;
        private Texture2D MenuTexture;
        private Texture2D GameBackground;
        public static int[] HighScores { get; set; }
        public Keys[] KeyConfig { get; set; }
        public static int currentScore { get; set; }
        Random random;
        Keys rotRight;
        Keys rotLeft;
        Keys softDrop;
        Keys hardDrop;
        Keys moveLeft;
        Keys moveRight;
        int screenWidth;
        int screenHeight;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            

            Content.RootDirectory = "Content";

            //LoadKeys();
            //LoadScores();
            //rotLeft = KeyConfig[1];
            //rotRight = KeyConfig[2];
            //thrust = KeyConfig[0];
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            MenuTexture = Content.Load<Texture2D>("Main Menu");
            GameBackground = Content.Load<Texture2D>("Main Screen");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _spriteBatch.Draw(MenuTexture, new Rectangle((int)(screenWidth/4.4f), 0, screenHeight, screenHeight), Color.White);

            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}
