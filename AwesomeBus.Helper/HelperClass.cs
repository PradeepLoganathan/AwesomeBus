using System;

namespace AwesomeBus.Helper
{
    public static class HelperClass
    {
        static object syncLock = new object();

        public static void WriteToConsole(string message, ConsoleColor consoleColor = ConsoleColor.White)
        {
            lock(syncLock)
            {
                Console.ForegroundColor = consoleColor;
                Console.WriteLine($"{DateTime.Now} : {message}");

                Console.ResetColor();
            }
            
        }
    }
}
