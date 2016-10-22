using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MeerkatAI
{
    class MeerkatAI
    {
        private static readonly log4net.ILog log =
    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Option('b', null, Required = true, HelpText = "Board input")]
        public string BoardString { get; set; }

        [Option('p', null, Required = true, HelpText = "Player input")]
        public string Player { get; set; }

        [Option('t', null, Required = true, HelpText = "Board input")]
        public int Time { get; set; }

        // Infinity can be represented as a very large number
        const int INFINITY = 999999;

        // Fixed depth of 5
        const int DEPTH = 5;

        private int bestMove;

        public MeerkatAI()
        {
            bestMove = -1;
            log.Debug("Instantiated MeerkatAI");
        }

        static void Main(string[] args)
        {
            log.Info("MeerkatAI started");

            //var options = new MeerkatOptions();
            CommandLine.Parser parser = new Parser();

            // Instantiate our AI
            // Command line arguments will be passed in as properties through parser.ParseArguments
            MeerkatAI meerkatPlayer = new MeerkatAI();

            if(!parser.ParseArguments(args, meerkatPlayer))
            {
                log.Error("Command line arguments could not be parsed");
            }

            //Calls timeOutHandler half a second before the time expires
            Timer time = new Timer();
            // The callback function in the timer is a closure that contains the player
            time.Elapsed += new ElapsedEventHandler(makeTimeOutHandler(meerkatPlayer));
            time.Interval = meerkatPlayer.Time - 500;
            time.Enabled = true; //start timer
            log.Info("Timer started with " + (meerkatPlayer.Time - 500));

            // Select a move
            try
            {
                meerkatPlayer.move();
            } catch (Exception e)
            {
                log.Error(e);
            }
        }

        // Main game logic
        // Selects a move on the current board
        private void move()
        {
            Board board = new Board(this.BoardString, this.Player == "player-one" ? 1: 2);
            
            // Special case
            // If we are the first player and this is the first board, choose column 3
            if(board.IsEmpty())
            {
                Environment.Exit(3);
            }

            // Choose a random available move
            this.bestMove = chooseRandomMove(board);

            // Initialize best value to negative infinity
            int bestValue = -INFINITY;

            // Look at each possible move
            var validMoves = board.ValidMoves();
            foreach(int move in validMoves)
            {
                // Min max evaluation
                var newBoard = board.Move(move);
                int value = minMax(newBoard, DEPTH, true); // Should we always start as max, or is that dependent on which player is up?
                if (value > bestValue) {
                    bestValue = value;
                    this.bestMove = move;
                }
            }

            log.Info("Meerkat chose move: " + this.bestMove);
            Environment.Exit(this.bestMove);
        }

        // Random player useful for testing.
        // Selects a random valid move given a board.
        // If no valid moves are available, returns 0
        private static int chooseRandomMove(Board board)
        {
            // Get the valid moves from the board
            int[] moves = board.ValidMoves();

            if (moves != null && moves.Length > 0)
            {
                Random rand = new Random();
                return rand.Next(0, moves.Length);
            }
            else
            {
                log.Warn("No valid moves!");
                return 0;
            }
        }

        // Recursive minimax evaluation of a board
        // Inspired by a pseudocode example of min-max on Wikipedia: https://en.wikipedia.org/wiki/Minimax
        private static int minMax(Board board, int depth, bool isMax)
        {
            // If the remaining depth is 0, evaluate this board
            if (depth == 0) {
                return board.Heuristic();
            }

            // If this is the maximzing player, initialize the best value to negative infinity.
            // If this is the minimizing player, initialize the best value to infinity.
            int bestValue = isMax ? -INFINITY: INFINITY;
            int[] validMoves = board.ValidMoves();

            // Look at each possible move
            foreach(int move in validMoves)
            {
                // Get the board created by this move
                var newBoard = board.Move(move);
                // Recursively perform minimax on this board
                int value = minMax(newBoard, depth - 1, !isMax);
                // Update the best value if this board is better / worse
                bestValue = isMax ? Math.Max(value, bestValue) : Math.Min(value, bestValue);
            }

            return bestValue;
        }


        // Delegate type for timer callback function
        private delegate void timeOutHandler(object sender, ElapsedEventArgs e);

        // Generator function returns a closure containing the player to execute on timeout
        private static timeOutHandler makeTimeOutHandler(MeerkatAI meerkatPlayer)
        {
            return delegate (object sender, ElapsedEventArgs e)
            {
                if (meerkatPlayer.bestMove != -1)
                {
                    log.Info("Timer interrupted player: Chose " + meerkatPlayer.bestMove);
                    Environment.Exit(meerkatPlayer.bestMove);
                }


                int nextMove = 0;

                if(meerkatPlayer != null)
                {
                    var board = new Board(meerkatPlayer.BoardString, meerkatPlayer.Player == "player-one" ? 1 : 2);
                    nextMove = chooseRandomMove(board);
                }

                log.Info("Time expired: Chose " + nextMove);
                Environment.Exit(nextMove);
            };
        }

        
    }

}
