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
 * 8. Add song and sound effects
 * 10. Add AI
 * 11. Trigger AI after 10 seconds of idle menu.
 */

namespace Tetris
{
    public class Shape {
        public int[,] cells;
        public Game1.SHAPE_TYPE type;
        public enum DIRECTION { UP, RIGHT, DOWN, LEFT };
        public DIRECTION direction;

        public Shape(Game1.SHAPE_TYPE shape)
        {
            type = shape;
            direction = DIRECTION.UP;
            switch (type) {
                case Game1.SHAPE_TYPE.I:
                    cells = new int[4, 2] { { 5, 0 }, { 5, 1 }, { 5, 2 }, { 5, 3 } };
                    break;
                case Game1.SHAPE_TYPE.J:
                    cells = new int[4, 2] { { 5, 0 }, { 5, 1 }, { 5, 2 }, { 4, 2 } };
                    break;
                case Game1.SHAPE_TYPE.L:
                    cells = new int[4, 2] { { 5, 0 }, { 5, 1 }, { 5, 2 }, { 6, 2 } };
                    break;
                case Game1.SHAPE_TYPE.O:
                    cells = new int[4, 2] { { 5, 0 }, { 4, 0 }, { 5, 1 }, { 4, 1 } };
                    break;
                case Game1.SHAPE_TYPE.S:
                    cells = new int[4, 2] { { 5, 0 }, { 6, 0 }, { 5, 1 }, { 4, 1 } };
                    break;
                case Game1.SHAPE_TYPE.T:
                    cells = new int[4, 2] { { 5, 1 }, { 6, 1 }, { 4, 1 }, { 5, 2 } };
                    break;
                case Game1.SHAPE_TYPE.Z:
                    cells = new int[4, 2] { { 5, 0 }, { 4, 0 }, { 5, 1 }, { 6, 1 } };
                    break;
            }
        }

        

        #region "Shape Movement Methods"
        public void fall()
        {
            for(int i = 0; i < 4; i++)
            {
                cells[i, 1] += 1;
            }
        }

        public bool canFall(Tile[,] board)
        {
            bool fall = true;
            for(int i = 0; i < 4; i++)
            {
                if(cells[i,1] == 24)//tile has reached the bottom of the screen
                {
                    return false;
                }
                if(board[cells[i,0], cells[i, 1] + 1].isFilled)
                {
                    return false;
                }
            }
            return fall;
        }

        public bool canMoveRight(Tile[,] board)
        {
            bool move = true;
            for(int i = 0; i < 4; i++)
            {
                if(cells[i,0] == 9)//tile has reached right side of screen
                {
                    return false;
                }
                if(board[cells[i,0] + 1, cells[i, 1]].isFilled)
                {
                    return false;
                }
            }
            return move;
        }

        public void moveRight()
        {
            for(int i = 0; i < 4; i++)
            {
                cells[i, 0] += 1;
            }
            kick();
        }

        public bool canMoveLeft(Tile[,] board)
        {
            bool move = true;
            for (int i = 0; i < 4; i++)
            {
                if (cells[i, 0] == 0)//tile has reached left side of screen
                {
                    return false;
                }
                if (board[cells[i, 0] - 1, cells[i, 1]].isFilled)
                {
                    return false;
                }
            }
            return move;
        }

        public void moveLeft()
        {
            for (int i = 0; i < 4; i++)
            {
                cells[i, 0] -= 1;
            }
            kick();
        }

        public void rotRight(Tile[,] board)
        {
            switch (type) {
                case Game1.SHAPE_TYPE.I:
                    if(direction == DIRECTION.UP)
                    {
                        direction = DIRECTION.RIGHT;
                        cells[0, 0] += 1;
                        cells[0, 1] += 2;
                        cells[1, 1] += 1;
                        cells[2, 0] -= 1;
                        cells[3, 0] -= 2;
                        cells[3, 1] -= 1;
                    }
                    else if(direction == DIRECTION.RIGHT)
                    {
                        direction = DIRECTION.DOWN;
                        cells[0, 0] -= 2;
                        cells[0, 1] += 1;
                        cells[1, 0] -= 1;
                        cells[2, 1] -= 1;
                        cells[3, 0] += 1;
                        cells[3, 1] -= 2;
                    }
                    else if (direction == DIRECTION.DOWN)
                    {
                        direction = DIRECTION.LEFT;
                        cells[0, 0] -= 1;
                        cells[0, 1] -= 2;
                        cells[1, 1] -= 1;
                        cells[2, 0] += 1;
                        cells[3, 0] += 2;
                        cells[3, 1] += 1;
                    }
                    else if (direction == DIRECTION.LEFT)
                    {
                        direction = DIRECTION.UP;
                        cells[0, 0] += 2;
                        cells[0, 1] -= 1;
                        cells[1, 0] += 1;
                        cells[2, 1] += 1;
                        cells[3, 0] -= 1;
                        cells[3, 1] += 2;
                    }
                    break;
                case Game1.SHAPE_TYPE.J:
                    if (direction == DIRECTION.UP)
                    {
                        direction = DIRECTION.RIGHT;
                        cells[0, 0] += 1;
                        cells[0, 1] += 1;
                        cells[2, 0] -= 1;
                        cells[2, 1] -= 1;
                        cells[3, 1] -= 2;
                    }
                    else if (direction == DIRECTION.RIGHT)
                    {
                        direction = DIRECTION.DOWN;
                        cells[0, 0] -= 1;
                        cells[0, 1] += 1;
                        cells[2, 0] += 1;
                        cells[2, 1] -= 1;
                        cells[3, 0] += 2;
                    }
                    else if (direction == DIRECTION.DOWN)
                    {
                        direction = DIRECTION.LEFT;
                        cells[0, 0] -= 1;
                        cells[0, 1] -= 1;
                        cells[2, 0] += 1;
                        cells[2, 1] += 1;
                        cells[3, 1] += 2;
                    }
                    else if (direction == DIRECTION.LEFT)
                    {
                        direction = DIRECTION.UP;
                        cells[0, 0] += 1;
                        cells[0, 1] -= 1;
                        cells[2, 0] -= 1;
                        cells[2, 1] += 1;
                        cells[3, 0] -= 2;
                    }
                    break;
                case Game1.SHAPE_TYPE.L:
                    if (direction == DIRECTION.UP)
                    {
                        direction = DIRECTION.RIGHT;
                        cells[0, 0] += 1;
                        cells[0, 1] += 1;
                        cells[2, 0] -= 1;
                        cells[2, 1] -= 1;
                        cells[3, 0] -= 2;
                    }
                    else if (direction == DIRECTION.RIGHT)
                    {
                        direction = DIRECTION.DOWN;
                        cells[0, 0] -= 1;
                        cells[0, 1] += 1;
                        cells[2, 0] += 1;
                        cells[2, 1] -= 1;
                        cells[3, 1] -= 2;
                    }
                    else if (direction == DIRECTION.DOWN)
                    {
                        direction = DIRECTION.LEFT;
                        cells[0, 0] -= 1;
                        cells[0, 1] -= 1;
                        cells[2, 0] += 1;
                        cells[2, 1] += 1;
                        cells[3, 0] += 2;
                    }
                    else if (direction == DIRECTION.LEFT)
                    {
                        direction = DIRECTION.UP;
                        cells[0, 0] += 1;
                        cells[0, 1] -= 1;
                        cells[2, 0] -= 1;
                        cells[2, 1] += 1;
                        cells[3, 1] += 2;
                    }
                    break;
                case Game1.SHAPE_TYPE.O://Doesn't need to rotate
                    break;
                case Game1.SHAPE_TYPE.S:
                    if (direction == DIRECTION.UP)
                    {
                        direction = DIRECTION.RIGHT;
                        cells[0, 0] += 1;
                        cells[0, 1] += 1;
                        cells[1, 1] += 2;
                        cells[3, 0] += 1;
                        cells[3, 1] -= 1;
                    }
                    else if (direction == DIRECTION.RIGHT)
                    {
                        direction = DIRECTION.DOWN;
                        cells[0, 0] -= 1;
                        cells[0, 1] += 1;
                        cells[1, 0] -= 2;
                        cells[3, 0] += 1;
                        cells[3, 1] += 1;
                    }
                    else if (direction == DIRECTION.DOWN)
                    {
                        direction = DIRECTION.LEFT;
                        cells[0, 0] -= 1;
                        cells[0, 1] -= 1;
                        cells[1, 1] -= 2;
                        cells[3, 0] -= 1;
                        cells[3, 1] += 1;
                    }
                    else if (direction == DIRECTION.LEFT)
                    {
                        direction = DIRECTION.UP;
                        cells[0, 0] += 1;
                        cells[0, 1] -= 1;
                        cells[1, 0] += 2;
                        cells[3, 0] -= 1;
                        cells[3, 1] -= 1;
                    }
                    break;
                case Game1.SHAPE_TYPE.T:
                    if (direction == DIRECTION.UP)
                    {
                        direction = DIRECTION.RIGHT;
                        cells[1, 0] -= 1;
                        cells[1, 1] += 1;
                        cells[2, 0] += 1;
                        cells[2, 1] -= 1;
                        cells[3, 0] -= 1;
                        cells[3, 1] -= 1;
                    }
                    else if (direction == DIRECTION.RIGHT)
                    {
                        direction = DIRECTION.DOWN;
                        cells[1, 0] -= 1;
                        cells[1, 1] -= 1;
                        cells[2, 0] += 1;
                        cells[2, 1] += 1;
                        cells[3, 0] += 1;
                        cells[3, 1] -= 1;
                    }
                    else if (direction == DIRECTION.DOWN)
                    {
                        direction = DIRECTION.LEFT;
                        cells[1, 0] += 1;
                        cells[1, 1] -= 1;
                        cells[2, 0] -= 1;
                        cells[2, 1] += 1;
                        cells[3, 0] += 1;
                        cells[3, 1] += 1;
                    }
                    else if (direction == DIRECTION.LEFT)
                    {
                        direction = DIRECTION.UP;
                        cells[1, 0] += 1;
                        cells[1, 1] += 1;
                        cells[2, 0] -= 1;
                        cells[2, 1] -= 1;
                        cells[3, 0] -= 1;
                        cells[3, 1] += 1;
                    }
                    break;
                case Game1.SHAPE_TYPE.Z:
                    if (direction == DIRECTION.UP)
                    {
                        direction = DIRECTION.RIGHT;
                        cells[0, 0] += 1;
                        cells[0, 1] += 1;
                        cells[1, 0] += 2;
                        cells[3, 0] -= 1;
                        cells[3, 1] += 1;
                    }
                    else if (direction == DIRECTION.RIGHT)
                    {
                        direction = DIRECTION.DOWN;
                        cells[0, 0] -= 1;
                        cells[0, 1] += 1;
                        cells[1, 1] += 2;
                        cells[3, 0] -= 1;
                        cells[3, 1] -= 1;
                    }
                    else if (direction == DIRECTION.DOWN)
                    {
                        direction = DIRECTION.LEFT;
                        cells[0, 0] -= 1;
                        cells[0, 1] -= 1;
                        cells[1, 0] -= 2;
                        cells[3, 0] += 1;
                        cells[3, 1] -= 1;
                    }
                    else if (direction == DIRECTION.LEFT)
                    {
                        direction = DIRECTION.UP;
                        cells[0, 0] += 1;
                        cells[0, 1] -= 1;
                        cells[1, 1] -= 2;
                        cells[3, 0] += 1;
                        cells[3, 1] += 1;
                    }
                    break;
            }

            kick();

            for (int i = 0; i < 4; i++) // Do not allow rotation into filled cells
            {
                if(board[cells[i,0], cells[i, 1]].isFilled || cells[i, 1] > 24)
                {
                    rotLeft(board);
                }
            }
        }
        public void rotLeft(Tile[,] board)
        {
            switch (type)
            {
                case Game1.SHAPE_TYPE.I:
                    if (direction == DIRECTION.UP)
                    {
                        direction = DIRECTION.LEFT;
                        cells[0, 0] -= 2;
                        cells[0, 1] += 1;
                        cells[1, 0] -= 1;
                        cells[2, 1] -= 1;
                        cells[3, 0] += 1;
                        cells[3, 1] -= 2;
                    }
                    else if (direction == DIRECTION.RIGHT)
                    {
                        direction = DIRECTION.UP;
                        cells[0, 0] -= 1;
                        cells[0, 1] -= 2;
                        cells[1, 1] -= 1;
                        cells[2, 0] += 1;
                        cells[3, 0] += 2;
                        cells[3, 1] += 1;
                    }
                    else if (direction == DIRECTION.DOWN)
                    {
                        direction = DIRECTION.RIGHT;
                        cells[0, 0] += 2;
                        cells[0, 1] -= 1;
                        cells[1, 0] += 1;
                        cells[2, 1] += 1;
                        cells[3, 0] -= 1;
                        cells[3, 1] += 2;
                    }
                    else if (direction == DIRECTION.LEFT)
                    {
                        direction = DIRECTION.DOWN;
                        cells[0, 0] += 1;
                        cells[0, 1] += 2;
                        cells[1, 1] += 1;
                        cells[2, 0] -= 1;
                        cells[3, 0] -= 2;
                        cells[3, 1] -= 1;
                    }
                    break;
                case Game1.SHAPE_TYPE.J:
                    if (direction == DIRECTION.UP)
                    {
                        direction = DIRECTION.LEFT;
                        cells[0, 0] -= 1;
                        cells[0, 1] += 1;
                        cells[2, 0] += 1;
                        cells[2, 1] -= 1;
                        cells[3, 0] += 2;
                    }
                    else if (direction == DIRECTION.RIGHT)
                    {
                        direction = DIRECTION.UP;
                        cells[0, 0] -= 1;
                        cells[0, 1] -= 1;
                        cells[2, 0] += 1;
                        cells[2, 1] += 1;
                        cells[3, 1] += 2;
                    }
                    else if (direction == DIRECTION.DOWN)
                    {
                        direction = DIRECTION.RIGHT;
                        cells[0, 0] += 1;
                        cells[0, 1] -= 1;
                        cells[2, 0] -= 1;
                        cells[2, 1] += 1;
                        cells[3, 0] -= 2;
                    }
                    else if (direction == DIRECTION.LEFT)
                    {
                        direction = DIRECTION.DOWN;
                        cells[0, 0] += 1;
                        cells[0, 1] += 1;
                        cells[2, 0] -= 1;
                        cells[2, 1] -= 1;
                        cells[3, 1] -= 2;
                    }
                    break;
                case Game1.SHAPE_TYPE.L:
                    if (direction == DIRECTION.UP)
                    {
                        direction = DIRECTION.LEFT;
                        cells[0, 0] -= 1;
                        cells[0, 1] += 1;
                        cells[2, 0] += 1;
                        cells[2, 1] -= 1;
                        cells[3, 1] -= 2;
                    }
                    else if (direction == DIRECTION.RIGHT)
                    {
                        direction = DIRECTION.UP;
                        cells[0, 0] -= 1;
                        cells[0, 1] -= 1;
                        cells[2, 0] += 1;
                        cells[2, 1] += 1;
                        cells[3, 0] += 2;
                    }
                    else if (direction == DIRECTION.DOWN)
                    {
                        direction = DIRECTION.RIGHT;
                        cells[0, 0] += 1;
                        cells[0, 1] -= 1;
                        cells[2, 0] -= 1;
                        cells[2, 1] += 1;
                        cells[3, 1] += 2;
                    }
                    else if (direction == DIRECTION.LEFT)
                    {
                        direction = DIRECTION.DOWN;
                        cells[0, 0] += 1;
                        cells[0, 1] += 1;
                        cells[2, 0] -= 1;
                        cells[2, 1] -= 1;
                        cells[3, 0] -= 2;
                    }
                    break;
                case Game1.SHAPE_TYPE.O://Doesn't need to rotate
                    break;
                case Game1.SHAPE_TYPE.S:
                    if (direction == DIRECTION.UP)
                    {
                        direction = DIRECTION.LEFT;
                        cells[0, 0] -= 1;
                        cells[0, 1] += 1;
                        cells[1, 0] -= 2;
                        cells[3, 0] += 1;
                        cells[3, 1] += 1;

                    }
                    else if (direction == DIRECTION.RIGHT)
                    {
                        direction = DIRECTION.UP;
                        cells[0, 0] -= 1;
                        cells[0, 1] -= 1;
                        cells[1, 1] -= 2;
                        cells[3, 0] -= 1;
                        cells[3, 1] += 1;
                    }
                    else if (direction == DIRECTION.DOWN)
                    {
                        direction = DIRECTION.RIGHT;
                        cells[0, 0] += 1;
                        cells[0, 1] -= 1;
                        cells[1, 0] += 2;
                        cells[3, 0] -= 1;
                        cells[3, 1] -= 1;
                    }
                    else if (direction == DIRECTION.LEFT)
                    {
                        direction = DIRECTION.DOWN;
                        cells[0, 0] += 1;
                        cells[0, 1] += 1;
                        cells[1, 1] += 2;
                        cells[3, 0] += 1;
                        cells[3, 1] -= 1;
                    }
                    break;
                case Game1.SHAPE_TYPE.T:
                    if (direction == DIRECTION.UP)
                    {
                        direction = DIRECTION.LEFT;
                        cells[1, 0] -= 1;
                        cells[1, 1] -= 1;
                        cells[2, 0] += 1;
                        cells[2, 1] += 1;
                        cells[3, 0] += 1;
                        cells[3, 1] -= 1;
                    }
                    else if (direction == DIRECTION.RIGHT)
                    {
                        direction = DIRECTION.UP;
                        cells[1, 0] += 1;
                        cells[1, 1] -= 1;
                        cells[2, 0] -= 1;
                        cells[2, 1] += 1;
                        cells[3, 0] += 1;
                        cells[3, 1] += 1;
                    }
                    else if (direction == DIRECTION.DOWN)
                    {
                        direction = DIRECTION.RIGHT;
                        cells[1, 0] += 1;
                        cells[1, 1] += 1;
                        cells[2, 0] -= 1;
                        cells[2, 1] -= 1;
                        cells[3, 0] -= 1;
                        cells[3, 1] += 1;
                    }
                    else if (direction == DIRECTION.LEFT)
                    {
                        direction = DIRECTION.DOWN;
                        cells[1, 0] -= 1;
                        cells[1, 1] += 1;
                        cells[2, 0] += 1;
                        cells[2, 1] -= 1;
                        cells[3, 0] -= 1;
                        cells[3, 1] -= 1;
                    }
                    break;
                case Game1.SHAPE_TYPE.Z:
                    if (direction == DIRECTION.UP)
                    {
                        direction = DIRECTION.LEFT;
                        cells[0, 0] -= 1;
                        cells[0, 1] += 1;
                        cells[1, 1] += 2;
                        cells[3, 0] -= 1;
                        cells[3, 1] -= 1;
                    }
                    else if (direction == DIRECTION.RIGHT)
                    {
                        direction = DIRECTION.UP;
                        cells[0, 0] -= 1;
                        cells[0, 1] -= 1;
                        cells[1, 0] -= 2;
                        cells[3, 0] += 1;
                        cells[3, 1] -= 1;
                    }
                    else if (direction == DIRECTION.DOWN)
                    {
                        direction = DIRECTION.RIGHT;
                        cells[0, 0] += 1;
                        cells[0, 1] -= 1;
                        cells[1, 1] -= 2;
                        cells[3, 0] += 1;
                        cells[3, 1] += 1;
                    }
                    else if (direction == DIRECTION.LEFT)
                    {
                        direction = DIRECTION.DOWN;
                        cells[0, 0] += 1;
                        cells[0, 1] += 1;
                        cells[1, 0] += 2;
                        cells[3, 0] -= 1;
                        cells[3, 1] += 1;
                    }
                    break;
            }

            kick();

            for (int i = 0; i < 4; i++) // Do not allow rotation into filled cells
            {
                if (board[cells[i, 0], cells[i, 1]].isFilled || cells[i, 1] > 24)
                {
                    rotRight(board);
                }
            }
        }

        private void kick()
        {
            int leftOverlap = 0;
            int rightOverlap = 0;
            int topOverlap = 0;
            for (int i = 0; i < 4; i++)
            {
                if (cells[i, 0] > 9)
                    rightOverlap++;
                else if (cells[i, 0] < 0)
                    leftOverlap++;
            }

            for (int i = 0; i < 4; i++)
            {
                if (cells[i, 1] < 0)
                    topOverlap++;
            }

            if(leftOverlap > 0)
            {
                for(int i = 0; i < leftOverlap; i++)
                {
                    moveRight();
                }
            }
            else if (rightOverlap > 0)
            {
                for(int i = 0; i < rightOverlap; i++)
                {
                    moveLeft();
                }
            }
            if(topOverlap > 0)
            {
                for(int i = 0; i < topOverlap; i++)
                {
                    fall();
                }
            }
        }

        public void moveToTarget(Shape targetShape, Tile[,] board)
        {
            //if CurrentShape and targetShape are not in the same orientation, rotate the currentShape
            if (direction != targetShape.direction)
            {
                switch (targetShape.direction)
                {
                    case DIRECTION.LEFT:
                        rotLeft(board);
                        break;
                    case DIRECTION.RIGHT:
                        rotRight(board);
                        break;
                    default:
                        rotRight(board);
                        break;
                }
            }
            //if CurrentShape.X and targetShape.X are not the same, move in the x-axis
            else
            {
                if (targetShape.cells[0, 0] > cells[0, 0])
                {
                    moveRight();
                }
                else if (targetShape.cells[0, 0] < cells[0, 0])
                {
                    moveLeft();
                }
            }
        }

        public bool canMoveUp()
        {
            bool canMove = true;
            for(int i = 0; i < 4; i++)
            {
                if((cells[i,1] == 0 && this.type != Game1.SHAPE_TYPE.T) || (cells[i,1] == 1 && this.type == Game1.SHAPE_TYPE.T))
                {
                    return false;
                }
            }
            return canMove;
        }

        public void moveUp()
        {
            for(int i = 0; i < 4; i++)
            {
                cells[i, 1] -= 1;
            }
        }
        #endregion
    }


    public class Game1 : Game
    {
        #region "Class Properties"
        public enum SHAPE_TYPE { I, J, L, O, S, T, Z }
        public enum SCREEN { LOSE, QUIT, PLAY, MAIN, HIGH_SCORES, CREDITS, CONTROLS, NULL }
        public enum ACTION { PLAY, HIGH_SCORES, CREDITS, CONTROLS, BACK, QUIT, CONTINUE, CHANGE_ROTRIGHT, CHANGE_ROTLEFT, CHANGE_MOVERIGHT, CHANGE_MOVELEFT, CHANGE_DROP, CHANGE_HARDDROP, NULL }
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Menu myMenu;
        private List<SoundEffect> soundEffects;
        private SpriteFont font;
        Song theme;
        bool themePlaying;
        bool AI_MODE;
        TimeSpan timeInMainMenu;
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
        private List<Texture2D> shapeTextures;
        private Texture2D nextShape;
        private SHAPE_TYPE nextType;
        private Shape currentShape;
        private Shape targetShape;
        private SCREEN curScreen;
        private ACTION action;

        private GameBoard gameBoard;
        private ParticleEmitter cellBreaker;
        private ParticleEmitter shapeSetter;

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
        bool dropPressed;
        bool hardDropPressed;
        bool moveLeftPressed;
        bool moveRightPressed;
        bool rotLeftPressed;
        bool rotRightPressed;
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
        bool gravityAfterLineClear;
        double shapeFallRate;
        double fallRateRestore;
        int totalShapesUsed;
        int totalLinesCleared;
        int curLevel;
        TimeSpan lastShapeFallTime;
        TimeSpan lastPlayTime;
        #endregion

        #region "Constructor"
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

            soundEffects = new List<SoundEffect>();
            cellTextures = new List<Texture2D>();
            shapeTextures = new List<Texture2D>();
            themePlaying = false;
            changingKey = false;
            gravityAfterLineClear = false;
            dropPressed = false;
            hardDropPressed = false;
            moveLeftPressed = false;
            moveRightPressed = false;
            rotLeftPressed = false;
            rotRightPressed = false;
            AI_MODE = false;
            timeInMainMenu = new TimeSpan();
            shapeFallRate = 1000;
            fallRateRestore = 1000;
            totalShapesUsed = 0;
            totalLinesCleared = 0;
            curLevel = 0;
        }
        #endregion

        #region "Memory Persistence"
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
            SaveHighScores();
            SaveKeys();
            Exit();
        }
        #endregion

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

            font = Content.Load<SpriteFont>("font");

            soundEffects.Add(Content.Load<SoundEffect>("crash"));
            soundEffects.Add(Content.Load<SoundEffect>("beep"));
            soundEffects.Add(Content.Load<SoundEffect>("chime"));
            soundEffects.Add(Content.Load<SoundEffect>("thunder"));
            soundEffects.Add(Content.Load<SoundEffect>("lightning"));
            
            theme = Content.Load<Song>("theme");

            cellTextures.Add(iCell);
            cellTextures.Add(jCell);
            cellTextures.Add(lCell);
            cellTextures.Add(oCell);
            cellTextures.Add(sCell);
            cellTextures.Add(tCell);
            cellTextures.Add(zCell);

            shapeTextures.Add(iShape);
            shapeTextures.Add(jShape);
            shapeTextures.Add(lShape);
            shapeTextures.Add(oShape);
            shapeTextures.Add(sShape);
            shapeTextures.Add(tShape);
            shapeTextures.Add(zShape);

            myMenu = new Menu(Content, screenWidth, screenHeight, drop, rotRight, rotLeft, moveRight, moveLeft, hardDrop);
            cellBreaker = new ParticleEmitter(Content, 200, 200, (int)(cell.X / 3f), 250, new TimeSpan(0, 0, 0, 0, 3000), cell);
            shapeSetter = new ParticleEmitter(Content, 200, 200, (int)(cell.X / 3f), 250, new TimeSpan(0, 0, 0, 0, 1000), cell);
        }



        protected override void Update(GameTime gameTime)
        {
            if(timeInMainMenu == new TimeSpan())
            {
                timeInMainMenu = gameTime.TotalGameTime;
            }
            else if(gameTime.TotalGameTime.TotalMilliseconds - timeInMainMenu.TotalMilliseconds > 10000 && curScreen == SCREEN.MAIN)
            {
                AI_MODE = true;
                curScreen = SCREEN.PLAY;
                currentShape = new Shape(getRandomShape());
                targetShape = gameBoard.getTargetShape(currentShape);
                nextType = getRandomShape();
                lastShapeFallTime = gameTime.TotalGameTime;
                gravityAfterLineClear = false;
                totalShapesUsed = 0;
                totalLinesCleared = 0;
                gameBoard = new GameBoard(screenWidth, screenHeight, cell);
                gameBoard.initializeBoardValues();
                curLevel = 0;
                currentScore = 0;
                timeInMainMenu = new TimeSpan();
                myMenu.Update(curScreen);
            }
            action = myMenu.Update(curScreen);
            cellBreaker.Update(gameTime);
            shapeSetter.Update(gameTime);
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
                        myMenu.updateHardDrop(this.hardDrop);
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
                        myMenu.updateMoveLeft(this.moveLeft);
                    }
                    else if (moveRight == Keys.None)
                    {
                        moveRight = Keyboard.GetState().GetPressedKeys()[0];
                        KeyConfig[5] = moveRight;
                        myMenu.updateMoveRight(this.moveRight);
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
                            nextType = getRandomShape();
                            lastShapeFallTime = gameTime.TotalGameTime;
                            gravityAfterLineClear = false;
                            totalShapesUsed = 0;
                            totalLinesCleared = 0;
                            gameBoard = new GameBoard(screenWidth, screenHeight, cell);
                            gameBoard.initializeBoardValues();
                            curLevel = 0;
                            currentScore = 0;
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
                            timeInMainMenu = gameTime.TotalGameTime;
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
                            moveRight = Keys.None;
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
                        if (!gravityAfterLineClear)
                        {
                            if (gameTime.TotalGameTime.TotalMilliseconds - lastShapeFallTime.TotalMilliseconds > (AI_MODE ? 50 : (shapeFallRate - (curLevel*100))))
                            {
                                if (currentShape.canFall(gameBoard.board))
                                {
                                    currentShape.fall();
                                    lastShapeFallTime = gameTime.TotalGameTime;
                                }
                                else
                                {
                                    //save shape to board
                                    gameBoard.saveShape(currentShape);
                                    for(int i = 0; i < 4; i++)
                                    {
                                        shapeSetter.shapeSet(gameTime, gameBoard.board[currentShape.cells[i, 0], currentShape.cells[i, 1]].location, 0, 7, 0);
                                    }
                                    if (!gameBoard.isLoss())
                                    {
                                        //remove lines and calculate scores
                                        List<int> linesRemoved = gameBoard.detectLine();
                                        if (linesRemoved.Count > 0)
                                        {
                                            for (int i = 0; i < linesRemoved.Count; i++)
                                            {
                                                //figure out particle effects
                                                for(int j = 0; j < 10; j++)
                                                {
                                                    cellBreaker.lineClear(gameTime, gameBoard.board[j, linesRemoved[i]].location, 0, (int)(gameBoard.board[j, linesRemoved[i]].currentCell), 0);
                                                }
                                                gameBoard.removeLine(linesRemoved[i]);
                                            }
                                            gravityAfterLineClear = true;

                                            switch (linesRemoved.Count)
                                            {
                                                case 1:
                                                    currentScore += 40 * (curLevel + 1);
                                                    soundEffects[1].Play();
                                                    break;
                                                case 2:
                                                    currentScore += 100 * (curLevel + 1);
                                                    soundEffects[2].Play();
                                                    break;
                                                case 3:
                                                    currentScore += 300 * (curLevel + 1);
                                                    soundEffects[3].Play();
                                                    break;
                                                case 4:
                                                    currentScore += 1200 * (curLevel + 1);
                                                    soundEffects[4].Play();
                                                    break;
                                                default:
                                                    break;
                                            }

                                            totalLinesCleared += linesRemoved.Count;
                                        }
                                        else
                                        {
                                            soundEffects[0].Play();//Thud noise
                                        }

                                        
                                        //update currentShape and NextShape
                                        currentShape = new Shape(nextType);
                                        if (AI_MODE)
                                        {
                                            targetShape = gameBoard.getTargetShape(currentShape);
                                        }
                                        nextType = getRandomShape();

                                        //Increment Level for scoring and speed
                                        totalShapesUsed++;
                                        curLevel = (int)(totalShapesUsed / 10f) > 9 ? 9 : (int)(totalShapesUsed / 10f);
                                    }
                                    else
                                    {
                                        if (!AI_MODE)
                                        {
                                            curScreen = SCREEN.LOSE;
                                            myMenu.Update(curScreen);
                                            bool breaker = false;
                                            for (int i = 0; i < HighScores.Length; i++)
                                            {
                                                if (currentScore > HighScores[i] && !breaker)
                                                {
                                                    HighScores[i] = currentScore;
                                                    breaker = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            curScreen = SCREEN.PLAY;
                                            currentShape = new Shape(getRandomShape());
                                            nextType = getRandomShape();
                                            lastShapeFallTime = gameTime.TotalGameTime;
                                            gravityAfterLineClear = false;
                                            totalShapesUsed = 0;
                                            totalLinesCleared = 0;
                                            gameBoard = new GameBoard(screenWidth, screenHeight, cell);
                                            gameBoard.initializeBoardValues();
                                            curLevel = 0;
                                            currentScore = 0;
                                        }
                                    }
                                }
                            }
                        }
                        else //Allow pieces to fall before moving on to next shape
                        {
                            if (gameBoard.needsGravity())
                            {
                                if (gameTime.TotalGameTime.TotalMilliseconds - lastShapeFallTime.TotalMilliseconds > 400)
                                {
                                    gameBoard.applyGravity();
                                    lastShapeFallTime = gameTime.TotalGameTime;
                                }
                            }
                            else
                            {
                                gravityAfterLineClear = false;
                            }
                        }
                        if (!AI_MODE)
                        {
                            //Get User Input and respond accordingly

                            //DROP
                            if (Keyboard.GetState().IsKeyDown(drop))
                            {
                                if (!dropPressed)
                                {
                                    fallRateRestore = shapeFallRate;
                                    shapeFallRate = 100;
                                    dropPressed = true;
                                }
                            }
                            else if (Keyboard.GetState().IsKeyUp(drop))
                            {
                                shapeFallRate = fallRateRestore;
                                dropPressed = false;
                            }

                            //ROTATE LEFT
                            if (Keyboard.GetState().IsKeyDown(rotLeft))
                            {
                                if (!rotLeftPressed)
                                {
                                    currentShape.rotLeft(gameBoard.board);
                                    rotLeftPressed = true;
                                }
                            }
                            else if (Keyboard.GetState().IsKeyUp(rotLeft))
                            {
                                rotLeftPressed = false;
                            }

                            //ROTATE RIGHT
                            if (Keyboard.GetState().IsKeyDown(rotRight))
                            {
                                if (!rotRightPressed)
                                {
                                    currentShape.rotRight(gameBoard.board);
                                    rotRightPressed = true;
                                }
                            }
                            else if (Keyboard.GetState().IsKeyUp(rotRight))
                            {
                                rotRightPressed = false;
                            }

                            //HARD DROP
                            if (Keyboard.GetState().IsKeyDown(hardDrop))
                            {
                                if (!hardDropPressed)
                                {
                                    while (currentShape.canFall(gameBoard.board))
                                    {
                                        currentShape.fall();
                                    }
                                    hardDropPressed = true;
                                }
                            }
                            else if (Keyboard.GetState().IsKeyUp(hardDrop))
                            {
                                hardDropPressed = false;
                            }

                            //MOVE LEFT
                            if (Keyboard.GetState().IsKeyDown(moveLeft))
                            {
                                if (!moveLeftPressed)
                                {
                                    if (currentShape.canMoveLeft(gameBoard.board))
                                    {
                                        currentShape.moveLeft();
                                    }
                                    moveLeftPressed = true;
                                }
                            }
                            else if (Keyboard.GetState().IsKeyUp(moveLeft))
                            {
                                moveLeftPressed = false;
                            }

                            //MOVE RIGHT
                            if (Keyboard.GetState().IsKeyDown(moveRight))
                            {
                                if (!moveRightPressed)
                                {
                                    if (currentShape.canMoveRight(gameBoard.board))
                                    {
                                        currentShape.moveRight();
                                    }
                                    moveRightPressed = true;
                                }
                            }
                            else if (Keyboard.GetState().IsKeyUp(moveRight))
                            {
                                moveRightPressed = false;
                            }
                        }
                        else
                        {
                            currentShape.moveToTarget(targetShape, gameBoard.board);
                            if(Keyboard.GetState().GetPressedKeyCount() > 0)
                            {
                                AI_MODE = false;
                                curScreen = SCREEN.MAIN;
                                myMenu.Update(curScreen);

                            }
                        }
                        break;

                }

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            if (!themePlaying)
            {
                MediaPlayer.Play(theme);
                themePlaying = true;
                lastPlayTime = gameTime.TotalGameTime;
            }
            else if(gameTime.TotalGameTime.TotalMilliseconds - lastPlayTime.TotalMilliseconds > theme.Duration.TotalMilliseconds)
            {
                themePlaying = false;
            }


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
                    _spriteBatch.DrawString(font, "Level: " + curLevel.ToString(), new Vector2(screenWidth*0.052f, screenHeight*0.0926f), Color.White);
                    _spriteBatch.DrawString(font, "SCORE: " + currentScore.ToString(), new Vector2(screenWidth * 0.052f, screenHeight*0.12f), Color.White);
                    _spriteBatch.DrawString(font, "LINES CLEARED: " + totalLinesCleared.ToString(), new Vector2(screenWidth * 0.052f, screenHeight * 0.15f), Color.White);
                    //Render falling shape
                    for(int i = 0; i < 4; i++)
                    {
                        _spriteBatch.Draw(cellTextures[(int)currentShape.type], new Rectangle((int)gameBoard.board[currentShape.cells[i, 0], currentShape.cells[i,1]].location.X, (int)gameBoard.board[currentShape.cells[i, 0], currentShape.cells[i, 1]].location.Y, (int)cell.X, (int)cell.Y), Color.White);
                    }
                    //Render existing Board
                    for(int i = 0; i < 10; i++)
                    {
                        for(int j = 0; j < 25; j++)
                        {
                            if(gameBoard.board[i,j].isFilled)
                                _spriteBatch.Draw(cellTextures[(int)gameBoard.board[i,j].currentCell], new Rectangle((int)gameBoard.board[i,j].location.X, (int)gameBoard.board[i,j].location.Y, (int)cell.X, (int)cell.Y), Color.White);
                        }
                    }

                    //Render Particle effects on line clear and set
                    cellBreaker.Draw(_spriteBatch);
                    shapeSetter.Draw(_spriteBatch);

                    //Render next shape in predictive box
                    _spriteBatch.Draw(shapeTextures[(int)nextType], new Rectangle(1100, 243, 180, 180), Color.White);
                    break;
            }

            
            _spriteBatch.End();
            myMenu.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}
