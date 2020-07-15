using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterBotDotNet
{
    class Schedule
    {
        public DateTime currentTime = DateTime.Now;
        public static void PrintHowLongToWait()
        {
            
            Console.WriteLine("You will post a tweet at a later date");
            Console.WriteLine("How many days do you want to wait?");
            int userAddDays = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("How many hours do you want to wait?");
            int userAddHours = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("How many minutes do you want to wait?");
            int userAddMinutes = Convert.ToInt32(Console.ReadLine());
        }
    }
}
