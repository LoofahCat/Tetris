using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class GameBoard
    {
        public bool[,] board;

        GameBoard()
        {
            board = new bool[10, 21];
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 21; j++)
                {
                    board[i,j] = false;
                }
            }
        }

        private bool checkLine(int lineNum)
        {
            bool completeLine = true;
            for (int i = 0; i < lineNum; i++)
            {
                if (board[i, lineNum] == false)
                {
                    completeLine = false;
                    break;
                }
            }
            return completeLine;
        }

        public void detectLine()
        {
            for(int i = 0; i < 21; i++)
            {
                if (checkLine(i))
                {
                    removeLine(i);
                }
            }

        }

        private void removeLine(int lineNum)
        {

        }
    }
}
