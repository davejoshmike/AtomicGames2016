﻿using Newtonsoft.Json;
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
        public Board(string board, int player)
        {
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
            return false;
        }

        public Board Move(int column)
        {
            return null;
        }

        private bool DiagonalGoal()
        {
            return false;
        }
    }
}
