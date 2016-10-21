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
        static void Main(string[] args)
        {
            //Calls timeOutHandler after 9.5 seconds
            Timer time = new Timer();
            time.Elapsed += new ElapsedEventHandler(timeOutHandler);
            time.Interval = (10*1000)-500;
            time.Enabled = true; //start timer

            Console.Read(); //exit on input
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
