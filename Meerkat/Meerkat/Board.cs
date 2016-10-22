using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeerkatAI
{
    class Board
    {
        private static readonly log4net.ILog log =
    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int[,] myBoard;
        private int player;
        private int p1threes;
        private int p1fours;
        private int p2threes;
        private int p2fours;
        private int TOP = 0;
        private int BOTTOM = 5;
        private int NUM_OF_ROWS = 6;
        private int NUM_OF_COLUMNS = 7;

        public Board(string board, int player)
        {
            try
            {
                myBoard = JsonConvert.DeserializeObject<int[,]>(board);
                log.Debug(myBoard.ToString());
                log.Debug("Parsed board successfully");
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            this.player = player;

            
        }

        public int[] ValidMoves()
        {
            var list = new List<int>();
            for (int i = 0; i < NUM_OF_COLUMNS; i++)
            {
                if (myBoard[0, i] == 0)
                {
                    list.Add(i);
                }
            }
            return list.ToArray();
        }

        public int Heuristic()
        {
            var score = 0;
            p1threes = 0;
            p1fours = 0;
            p2threes = 0;
            p2fours = 0;
            for (int x = 0; x < NUM_OF_COLUMNS; x++)
            {
                for (int y = 0; y < NUM_OF_ROWS; y++)
                {
                    if (myBoard[y, x] != 0)
                    {
                        //Found a piece!
                        HorizontalGoal(x, y - 1, 1);
                        HorizontalGoal(x, y - 1, 2);
                        //VerticalGoal(x, y);
                    }
                }
            }

            score += (p1threes - p2threes);
            score += (p1fours - p2fours)*10;
            //score needs to negative if player 2
            if(player==2)
                score *= -1;
            return score;
        }

        public int NumPieces()
        {
            return -1;
        }

        public bool IsGoal()
        {
            myBoard.ToString();
            for (int i = 0; i < NUM_OF_ROWS; i++)
            {
                for (int j = 0; j < NUM_OF_COLUMNS; j++)
                {
                    
                }
            }
            return true;
        }

        public bool IsEmpty()
        {
            for (int i = 0; i < NUM_OF_COLUMNS; i++)
            {
                if (myBoard[BOTTOM, i] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public Board Move(int column)
        {

            return null;
        }

        private bool DiagonalGoal()
        {
            
            return false;
        }

        private void HorizontalGoal(int x, int y, int whichPlayer)
        {
            int xcoord = x;
            int pieceCount = 1;
            //left
            while (xcoord>0)
            {
                
                xcoord--;
                if (myBoard[y, xcoord] == whichPlayer)
                {
                    pieceCount++;
                }
                else{
                    break;
                }
            }
            //right
            while (xcoord<7)
            {
                xcoord++;
                if (myBoard[y, xcoord] == whichPlayer)
                {
                    pieceCount++;
                }
                else
                {
                    break;
                }
            }

            if (pieceCount == 3)
            {
                if(whichPlayer==1)
                    p1threes++;
                if (whichPlayer == 2)
                    p2threes++;
            }
            if (pieceCount == 4)
            {
                if (whichPlayer == 1)
                    p1fours++;
                if (whichPlayer == 2)
                    p2fours++;
            }
        }

        private bool VerticalGoal()
        {

            return false;
        }
    }
}
