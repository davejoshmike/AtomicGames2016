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
        private int TOP = 0;
        private int BOTTOM = 5;

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
            //Defense has a higher heuristic
            return -1;
        }

        public int NumPieces()
        {
            return -1;
        }

        public bool IsGoal()
        {
            return false;
        }

        public bool IsEmpty()
        {
            for (int i = 0; i < 7; i++)
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
            
            return new Board(this.myBoard, this.player == 1 ? 1 : 2);
        }

        private bool DiagonalGoal()
        {
            
            return false;
        }
    }
}
