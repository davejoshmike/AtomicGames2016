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
            log.Info("Meerkat Started");

            //Calls timeOutHandler after 9.5 seconds
            Timer time = new Timer();
            time.Elapsed += new ElapsedEventHandler(timeOutHandler);
            time.Interval = (10*1000)-500;
            time.Enabled = true; //start timer
            log.Debug("Received arguments:");
            foreach (var arg in args) {
                log.Debug(arg);
            }
            
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
}
