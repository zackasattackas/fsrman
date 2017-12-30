using System;
using System.Security;

namespace fsrman
{
    internal static class Helpers
    {


        public static SecureString GetSecurePassword()
        {
            Console.Write("Enter password: ");
            var pwd = new SecureString();
            ConsoleKeyInfo currKey;
            while ((currKey = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (currKey.Key == ConsoleKey.Backspace)
                {
                    pwd.RemoveAt(pwd.Length - 1);
                    Console.Write("\b \b");
                }
                else
                {
                    pwd.AppendChar(currKey.KeyChar);
                    Console.Write('*');
                }                
            }

            pwd.MakeReadOnly();

            ConsoleClearLine(Console.CursorTop);
            return pwd;
        }

        public static void ConsoleClearLine(int cursorTop)
        {
            Console.SetCursorPosition(0, cursorTop);
            Console.WriteLine(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, cursorTop);
        }
    }
}
