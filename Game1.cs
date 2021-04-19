using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Media;
using System.IO;

/*
 * TODO:
 * 1. Get a shape to appear on the board
 * 2. Get a shape to fall at an increasing rate
 * 3. Make shape collide with any occupied cells. Stop all movement of cells in the shape. Record locations. Check for complete lines. Trigger score and gravity.
 * 4. Make next shape appear in window.
 * 5. Make a 'forbidden zone' in the board. Falling shapes are fine to exist there. Any stopped cell in the forbidden zone will trigger loss
 * 6. Make shape appear/fall from forbidden zone. Make them invisible before they hit the board space (cover them with a panel?)
 * 8. Add song and sound effects
 * 9. Add Particle Effects on Line Clear.
 * 10. Add AI
 * 11. Trigger AI after 10 seconds of idle menu.
 */

namespace Tetris
{
    public class Shape {
        public int[,] cells;
        public Game1.SHAPE_TYPE type;
        bool falling;

        public Shape(Game1.SHAPE_TYPE shape)
        {
            type = shape;
            falling = true;
            
            switch (type) {
                case Game1.SHAPE_TYPE.I:
                    cells = new int[4, 2] { { 0, 5 }, { 1, 5 }, { 2, 5 }, { 3, 5 } };
                    break;
                case Game1.SHAPE_TYPE.J:
                    cells = new int[4, 2] { { 0, 5 }, { 1, 5 }, { 2, 5 }, { 2, 4 } };
                    break;
                case Game1.SHAPE_TYPE.L:
                    cells = new int[4, 2] { { 0, 5 }, { 1, 5 }, { 2, 5 }, { 2, 6 } };
                    break;
                case Game1.SHAPE_TYPE.O:
                    cells = new int[4, 2] { { 0, 5 }, { 0, 4 }, { 1, 5 }, { 1, 4 } };
                    break;
                case Game1.SHAPE_TYPE.S:
                    cells = new int[4, 2] { { 0, 5 }, { 0, 6 }, { 1, 5 }, { 1, 4 } };
                    break;
                case Game1.SHAPE_TYPE.T:
                    cells = new int[4, 2] { { 0, 5 }, { 0, 6 }, { 0, 4 }, { 1, 5 } };
                    break;
                case Game1.SHAPE_TYPE.Z:
                    cells = new int[4, 2] { { 0, 5 }, { 0, 4 }, { 1, 5 }, { 1, 6 } };
                    break;
            }

        }
    }


    public class Game1 : Game
    {
        public enum SHAPE_TYPE { I, J, L, O, S, T, Z }
        public enum SCREEN { LOSE, QUIT, PLAY, MAIN, HIGH_SCORES, CREDITS, CONTROLS, NULL }
        public enum ACTION { PLAY, HIGH_SCORES, CREDITS, CONTROLS, BACK, QUIT, CONTINUE, CHANGE_ROTRIGHT, CHANGE_ROTLEFT, CHANGE_MOVERIGHT, CHANGE_MOVELEFT, CHANGE_DROP, CHANGE_HARDDROP, NULL }
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Menu myMenu;
        private List<SoundEffect> soundEffects;
        private SpriteFont font;
        Song theme;
        private Texture2D MenuTexture;
        private Texture2D GameBackground;
        private Texture2D iCell;
        private Texture2D iShape;
        private Texture2D jCell;
        private Texture2D jShape;
        private Texture2D lCell;
        private Texture2D lShape;
        private Texture2D oCell;
        private Texture2D oShape;
        private Texture2D sCell;
        private Texture2D sShape;
        private Texture2D tCell;
        private Texture2D tShape;
        private Texture2D zCell;
        private Texture2D zShape;
        private List<Texture2D> cellTextures;
        private Texture2D nextShape;

        private Shape currentShape;
        private SCREEN curScreen;
        private ACTION action;

        private GameBoard gameBoard;

        public static int[] HighScores { get; set; }
        public Keys[] KeyConfig { get; set; }
        public static int currentScore { get; set; }
        Random random;
        Keys rotRight;
        Keys rotLeft;
        Keys drop;
        Keys hardDrop;
        Keys moveLeft;
        Keys moveRight;
        int screenWidth;
        int screenHeight;
        Color iColor;
        Color oColor;
        Color jColor;
        Color lColor;
        Color sColor;
        Color tColor;
        Color zColor;
        Vector2 cell;
        bool changingKey;
        float gravityMultiplier;

        public Game1()
        {
            random = new Random();
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            iColor = Color.LightBlue;
            jColor = Color.LightPink;
            lColor = Color.Lavender;
            iColor = Color.LightYellow;
            sColor = Color.LightGreen;
            tColor = Color.Orange;
            zColor = Color.Red;

            curScreen = SCREEN.MAIN;
            LoadKeys();
            LoadScores();
            rotRight = KeyConfig[3];
            rotLeft = KeyConfig[2];
            drop = KeyConfig[0];
            hardDrop = KeyConfig[1];
            moveLeft = KeyConfig[4];
            moveRight = KeyConfig[5];

            cellTextures = new List<Texture2D>();
            changingKey = false;
        }

        public SHAPE_TYPE getRandomShape()
        {
            Array values = Enum.GetValues(typeof(SHAPE_TYPE));
            SHAPE_TYPE s = (SHAPE_TYPE)values.GetValue(random.Next(values.Length));
            return s;
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            cell = new Vector2((int)(screenWidth * 0.0185f), (int)(screenHeight * 0.03287f));
            gameBoard = new GameBoard(screenWidth, screenHeight, cell);
            gameBoard.initializeBoardValues();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            MenuTexture = Content.Load<Texture2D>("Main Menu");
            GameBackground = Content.Load<Texture2D>("Main Screen");
            iCell = Content.Load<Texture2D>("iCell");
            iShape = Content.Load<Texture2D>("i");
            jCell = Content.Load<Texture2D>("jCell");
            jShape = Content.Load<Texture2D>("j");
            lCell = Content.Load<Texture2D>("lCell");
            lShape = Content.Load<Texture2D>("l");
            oCell = Content.Load<Texture2D>("oCell");
            oShape = Content.Load<Texture2D>("o");
            sCell = Content.Load<Texture2D>("sCell");
            sShape = Content.Load<Texture2D>("s");
            tCell = Content.Load<Texture2D>("tCell");
            tShape = Content.Load<Texture2D>("t");
            zCell = Content.Load<Texture2D>("zCell");
            zShape = Content.Load<Texture2D>("z");

            cellTextures.Add(iCell);
            cellTextures.Add(jCell);
            cellTextures.Add(lCell);
            cellTextures.Add(oCell);
            cellTextures.Add(sCell);
            cellTextures.Add(tCell);
            cellTextures.Add(zCell);

            nextShape = zShape;
            myMenu = new Menu(Content, screenWidth, screenHeight, drop, rotRight, rotLeft, moveRight, moveLeft, hardDrop);
        }

        public void LoadKeys()
        {
            KeyConfig = new Keys[6];
            string line = "";
            int iterator = 0;
            System.IO.StreamReader reader = new System.IO.StreamReader(Content.RootDirectory + "/keyConfig.txt");
            while ((line = reader.ReadLine()) != null)
            {
                Keys key = (Keys)Enum.Parse(typeof(Keys), line, true);
                KeyConfig[iterator] = key;
                iterator++;
            }
            reader.Dispose();
        }

        public void SaveKeys()
        {
            using (StreamWriter outputFile = new StreamWriter(Content.RootDirectory + "/keyConfig.txt"))
            {
                foreach (Keys key in KeyConfig)
                {
                    outputFile.WriteLine(key.ToString());
                }
                outputFile.Dispose();
            }
        }

        public void SaveHighScores()
        {
            using (StreamWriter outputFile = new StreamWriter(Content.RootDirectory + "/HighScores.txt"))
            {
                foreach (int score in HighScores)
                {
                    outputFile.WriteLine(score.ToString());
                }
                outputFile.Dispose();
            }
        }

        public void LoadScores()
        {
            HighScores = new int[5];
            string line = "";
            int iterator = 0;
            System.IO.StreamReader reader = new System.IO.StreamReader(Content.RootDirectory + "/HighScores.txt");
            while ((line = reader.ReadLine()) != null)
            {
                HighScores[iterator] = Int32.Parse(line);
                iterator++;
            }
            reader.Dispose();
        }

        public void Quit()
        {
            //SaveHighScores();
            //SaveKeys();
            Exit();
        }


        protected override void Update(GameTime gameTime)
        {
            action = myMenu.Update(curScreen);
            if (changingKey)
            {
                if (Keyboard.GetState().GetPressedKeyCount() == 1 && Keyboard.GetState().IsKeyUp(Keys.Enter))
                {
                    if (drop == Keys.None)
                    {
                        drop = Keyboard.GetState().GetPressedKeys()[0];
                        KeyConfig[0] = drop;
                        myMenu.updateDrop(this.drop);
                    }
                    else if (hardDrop == Keys.None)
                    {
                        hardDrop = Keyboard.GetState().GetPressedKeys()[0];
                        KeyConfig[1] = hardDrop;
                        myMenu.updateDrop(this.hardDrop);
                    }
                    else if (rotLeft == Keys.None)
                    {
                        rotLeft = Keyboard.GetState().GetPressedKeys()[0];
                        KeyConfig[2] = rotLeft;
                        myMenu.updateRotLeft(this.rotLeft);
                    }
                    else if (rotRight == Keys.None)
                    {
                        rotRight = Keyboard.GetState().GetPressedKeys()[0];
                        KeyConfig[3] = rotRight;
                        myMenu.updateRotRight(this.rotRight);
                    }
                    else if (moveLeft == Keys.None)
                    {
                        moveLeft = Keyboard.GetState().GetPressedKeys()[0];
                        KeyConfig[4] = moveLeft;
                        myMenu.updateRotLeft(this.moveLeft);
                    }
                    else if (moveRight == Keys.None)
                    {
                        moveRight = Keyboard.GetState().GetPressedKeys()[0];
                        KeyConfig[5] = moveRight;
                        myMenu.updateRotRight(this.moveRight);
                    }
                    changingKey = false;
                }
            }
            else
            {
                if (action != ACTION.NULL)
                {
                    switch (action)
                    {
                        case ACTION.PLAY:
                            curScreen = SCREEN.PLAY;
                            currentShape = new Shape(getRandomShape());
                            break;
                        case ACTION.HIGH_SCORES:
                            curScreen = SCREEN.HIGH_SCORES;
                            break;
                        case ACTION.CONTROLS:
                            curScreen = SCREEN.CONTROLS;
                            break;
                        case ACTION.CREDITS:
                            curScreen = SCREEN.CREDITS;
                            break;
                        case ACTION.BACK:
                            curScreen = myMenu.destinationScreen;
                            break;
                        case ACTION.QUIT:
                            Quit();
                            break;
                        case ACTION.CONTINUE:
                            break;
                        case ACTION.CHANGE_DROP:
                            drop = Keys.None;
                            myMenu.updateDrop(Keys.None);
                            changingKey = true;
                            break;
                        case ACTION.CHANGE_ROTLEFT:
                            rotLeft = Keys.None;
                            myMenu.updateRotLeft(Keys.None);
                            changingKey = true;
                            break;
                        case ACTION.CHANGE_ROTRIGHT:
                            rotRight = Keys.None;
                            myMenu.updateRotRight(Keys.None);
                            changingKey = true;
                            break;
                        case ACTION.CHANGE_HARDDROP:
                            hardDrop = Keys.None;
                            myMenu.updateHardDrop(Keys.None);
                            changingKey = true;
                            break;
                        case ACTION.CHANGE_MOVELEFT:
                            moveLeft = Keys.None;
                            myMenu.updateMoveLeft(Keys.None);
                            changingKey = true;
                            break;
                        case ACTION.CHANGE_MOVERIGHT:
                            rotRight = Keys.None;
                            myMenu.updateMoveRight(Keys.None);
                            changingKey = true;
                            break;

                    }
                }
                switch (curScreen)
                {
                    case SCREEN.MAIN:
                        break;
                    case SCREEN.PLAY:
                        break;
                }

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            switch (curScreen)
            {
                case SCREEN.CREDITS:
                case SCREEN.HIGH_SCORES:
                case SCREEN.CONTROLS:
                case SCREEN.LOSE:
                case SCREEN.QUIT:
                    _spriteBatch.Draw(MenuTexture, new Rectangle((int)(screenWidth / 4.4f), 0, screenHeight, screenHeight), Color.White);
                    break;
                case SCREEN.MAIN:
                    _spriteBatch.Draw(MenuTexture, new Rectangle((int)(screenWidth / 4.4f), 0, screenHeight, screenHeight), Color.White);
                    break;
                case SCREEN.PLAY:
                    _spriteBatch.Draw(GameBackground, new Rectangle((int)(screenWidth / 4.4f), 0, screenHeight, screenHeight), Color.White);
                    //Render falling shape
                    for(int i = 0; i < 4; i++)
                    {
                        _spriteBatch.Draw(cellTextures[(int)currentShape.type], new Rectangle((int)gameBoard.board[currentShape.cells[i, 0], currentShape.cells[i,1]].location.X, (int)gameBoard.board[currentShape.cells[i, 0], currentShape.cells[i, 1]].location.Y, (int)cell.X, (int)cell.Y), Color.White);
                    }
                    //Render existing Board
                    //Render Particle effects on line clear and set
                    _spriteBatch.Draw(nextShape, new Rectangle(1100, 243, 180, 180), Color.White);
                    break;
            }

            
            _spriteBatch.End();
            myMenu.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}
