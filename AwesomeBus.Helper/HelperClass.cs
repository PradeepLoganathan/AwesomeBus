using System;

namespace AwesomeBus.Helper
{
    public static class HelperClass
    {
        static readonly object syncLock = new object();

        public static void WriteToConsole(string message, ConsoleColor consoleColor = ConsoleColor.White)
        {
            lock(syncLock)
            {
                Console.ForegroundColor = consoleColor;
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} : {message}");

                Console.ResetColor();
            }
            
        }
    }
}
