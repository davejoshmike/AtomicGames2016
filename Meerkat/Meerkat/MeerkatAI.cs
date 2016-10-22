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

        private int nextMove;

        public MeerkatAI()
        {
            nextMove = -1;
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
            meerkatPlayer.move();
        }

        // Main game logic
        // Selects a move on the current board
        private void move()
        {
            Board board = new Board(BoardString);

            int[] moves = board.ValidMoves();

            if (moves.Length > 0)
            {
                this.nextMove = moves[0];
            }
            else
            {
                log.Warn("No valid moves!");
                this.nextMove = 0;
            }
            log.Info("Meerkat chose move: " + this.nextMove);
            Environment.Exit(this.nextMove);
        }

        // Delegate type for timer callback function
        private delegate void timeOutHandler(object sender, ElapsedEventArgs e);

        // Generator function returns a closure containing the player to execute on timeout
        private static timeOutHandler makeTimeOutHandler(MeerkatAI meerkatPlayer)
        {
            return delegate (object sender, ElapsedEventArgs e)
            {
                if (meerkatPlayer.nextMove != -1)
                {
                    Environment.Exit(meerkatPlayer.nextMove);
                }

                //TODO Implement logic that chooses a move at timeout
                Console.WriteLine("Hello World!");
                Random rand = new Random();

                switch (rand.Next(0, 11))
                {
                    case 0:
                    case 1:
                    case 2:
                        break;
                    default:
                        break;
                }

                log.Info("Time expired.");
                Environment.Exit(0);
            };
        }

        
    }

}
