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

        static void Main(string[] args)
        {
            String board = "";
            String player = "";
            int timeMillis = 0;

            var options = new MeerkatOptions();
            CommandLine.Parser parser = new Parser();
            if (parser.ParseArguments(args, options))
            {
                board = options.Board;
                player = options.Player;
                timeMillis = options.Time;
            }

            log.Info("Meerkat Started");
            log.Debug("Board: " + board);
            log.Debug("Player: " + player);
            log.Debug("Time: " + timeMillis);

            //Calls timeOutHandler after 9.5 seconds
            Timer time = new Timer();
            time.Elapsed += new ElapsedEventHandler(timeOutHandler);
            time.Interval = (10*1000)-500;
            time.Enabled = true; //start timer
            
            Console.Read(); //exit on input

            log.Info("Meerkat completed with result 0");
        }

        private static void timeOutHandler(object sender, ElapsedEventArgs e)
        {
            //TODO Implement logic that chooses a move at timeout
            Console.WriteLine("Hello World!");
            Random rand = new Random();

            switch (rand.Next(0,11))
            {
                case 0:
                case 1:
                case 2:
                    break;
                default:
                    break;
            }
        }
        
    }

    class MeerkatOptions
    {
        [Option('b', null, Required = true, HelpText = "Board input")]
        public string Board { get; set; }

        [Option('p', null, Required = true, HelpText = "Player input")]
        public string Player { get; set; }

        [Option('t', null, Required = true, HelpText = "Board input")]
        public int Time { get; set; }
    }
}
