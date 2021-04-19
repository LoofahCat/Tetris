﻿using Microsoft.Xna.Framework;
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
    public class GameBoard
    {
        
        public Tile[,] board;
        public int screenWidth { get; set; }
        public int screenHeight { get; set; }
        public Vector2 cell { get; set; }

        public GameBoard(int w, int h, Vector2 c)
        {
            screenWidth = w;
            screenHeight = h;
            cell = c;
        }

        public void initializeBoardValues()
        {
            board = new Tile[10, 25];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    board[i, j] = new Tile();
                    board[i, j].isFilled = false;
                    board[i, j].location = new Vector2((screenWidth * 0.354f) + (i * cell.X), (screenHeight * 0.225f) + ((j-4) * cell.Y));
                }
            }
        }

        private bool checkLine(int lineNum)
        {
            bool completeLine = true;
            for (int i = 0; i < lineNum; i++)
            {
                if (board[i, lineNum].isFilled == false)
                {
                    completeLine = false;
                    break;
                }
            }
            return completeLine;
        }

        public int detectLine()//find lines and return total score after clearing.
        {
            int numLinesRemoved = 0;
            for(int i = 4; i < 25; i++)
            {
                if (checkLine(i))
                {
                    removeLine(i);
                    numLinesRemoved++;
                }
            }
            return numLinesRemoved;
        }

        private void removeLine(int lineNum)
        {

        }

        public void saveShape(Shape shape)
        {
            for(int i = 0; i < 4; i++)
            {
                board[shape.cells[i, 0], shape.cells[i, 1]].isFilled = true;
                board[shape.cells[i, 0], shape.cells[i, 1]].currentCell = shape.type;
            }
        }

        public bool isLoss()
        {
            bool lost = false;
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    if (board[i, j].isFilled)
                    {
                        return true;
                    }
                }
            }
            return lost;
        }
    }

    public class Tile {
        public bool isFilled;
        public Vector2 location;
        public Game1.SHAPE_TYPE currentCell;
        public Tile()
        {
            isFilled = false;
            location = new Vector2();
            currentCell = Game1.SHAPE_TYPE.I;
        }
    }
}
