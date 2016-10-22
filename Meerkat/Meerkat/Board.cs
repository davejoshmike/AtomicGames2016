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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int[,] myBoard;
        private int player;
        private int p1twos;
        private int p2twos;
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
            this.player = player;
            try
            {
                this.myBoard = JsonConvert.DeserializeObject<int[,]>(board);
                log.Debug(myBoard.ToString());
                log.Debug("Parsed board successfully");
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }
        // Copy constructor
        public Board(int[,] board, int player)
        {
            this.myBoard = board;
            this.player = player;
            this.player = player;

            
        }

        public int[] ValidMoves()
        {
            try
            {
                var list = new List<int>();
                for (int i = 0; i < 7; i++)
                {
                    if (myBoard[0, i] == 0)
                    {
                        list.Add(i);
                    }
                }
                return list.ToArray();
            } catch(Exception e)
            {
                // Log exception
                log.Error("Caught exception in ValidMoves(): ", e);
                return null;
            }
        }

        public int Heuristic()
        {
            var score = 0;
            p1threes = 0;
            p1fours = 0;
            p2threes = 0;
            p2fours = 0;
            p1twos = 0;
            p2twos = 0;
            for (int x = 0; x < NUM_OF_COLUMNS; x++)
            {
                for (int y = 0; y < NUM_OF_ROWS; y++)
                {
                    if (myBoard[y, x] != 0 && y > 0)
                    {
                        //Found a piece!
                        HorizontalGoal(x, y - 1, 1);
                        HorizontalGoal(x, y - 1, 2);
                        VerticalGoal(x, y - 1, 1);
                        VerticalGoal(x, y - 1, 2);
                    }
                }
            }
            Random rand = new Random();
            score += rand.Next(0, 50);

            score += (p1twos - p2twos) * 10;
            score += (p1threes - p2threes) * 100;
            score += (p1fours - p2fours)*1000;
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
            return Heuristic() > 499;
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
            int[,] newBoard = new int[6 , 7];
            // Copy array
            for(int x = 0; x < 7; x++)
            {
                for(int y = 0; y < 6; y++)
                {
                    newBoard[y, x] = this.myBoard[y, x];
                }
            }


            for (int i = 1; i < BOTTOM; i++)
            {
                    try
                    {
                        if (newBoard[i, column] != 0)
                        {
                            newBoard[i - 1, column] = this.player;
                        }   
                    }
                catch (IndexOutOfRangeException e)
                {
                    log.Error("Index: " + i);
                    log.Error("Caught IndexOutOfRangeException: " + e);
                }
            }
            
            return new Board(newBoard, this.player == 1 ? 2 : 1);
        }

        private bool DiagonalGoal()
        {
            
            return false;
        }

        private void HorizontalGoal(int x, int y, int whichPlayer)
        {
            try
            {
                int xcoord = x;
                int pieceCount = 1;
                //left
                while (xcoord > 0)
                {

                    xcoord--;
                    if (myBoard[y, xcoord] == whichPlayer)
                    {
                        pieceCount++;
                    }
                    else
                    {
                        break;
                    }
                }

                xcoord = x;

                //right
                while (xcoord < 6)
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
                if (pieceCount == 2)
                {
                    if (whichPlayer == 1)
                        p1twos++;
                    if (whichPlayer == 2)
                        p2twos++;
                }
                if (pieceCount == 3)
                {
                    if (whichPlayer == 1)
                        p1threes++;
                    if (whichPlayer == 2)
                        p2threes++;
                }
                if (pieceCount > 3)
                {
                    if (whichPlayer == 1)
                        p1fours++;
                    if (whichPlayer == 2)
                        p2fours++;
                }
            } catch (IndexOutOfRangeException e)
            {
                log.Error("Caught exception in HorizontalGoal: ", e);
            }
        }

        private void VerticalGoal(int x, int y, int whichPlayer)

        {

            int ycoord = y;

            int pieceCount = 0;

            //down

            while (ycoord > 6)

            {
                ycoord++;

                if (myBoard[ycoord, x] == whichPlayer)

                {

                    pieceCount++;

                }

                else

                {

                    break;

                }

            }

            if (pieceCount == 2)

            {

                if (whichPlayer == 1)

                    p1twos++;

                if (whichPlayer == 2)

                    p2twos++;

            }

            if (pieceCount == 3)

            {

                if (whichPlayer == 1)

                    p1threes++;

                if (whichPlayer == 2)

                    p2threes++;

            }

            if (pieceCount > 3)
            {

                if (whichPlayer == 1)

                    p1fours++;

                if (whichPlayer == 2)

                    p2fours++;
            }
        }
    }
}
