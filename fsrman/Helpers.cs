using System;
using System.Linq;
using System.Net;
using System.Security;
using System.ServiceProcess;

namespace fsrman
{
    internal static class Helpers
    {
        public static bool IsFsrmInstalledAndRunning(string computerName, NetworkCredential credentials = null)
        {
            ServiceController service = null;
            if (computerName != Environment.MachineName)
            {
                if (credentials == null)
                {
                    service = ServiceController.GetServices(computerName).FirstOrDefault(s => s.ServiceName == "SrmSvc");
                }
                else
                {
                    using (new Impersonation(credentials.Domain, credentials.UserName, credentials.Password))
                    {
                        service = ServiceController.GetServices(computerName).FirstOrDefault(s => s.ServiceName == "SrmSvc");
                    }
                }
            }
            else
            {
                service = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == "SrmSvc");
            }            

            return service != null && service.Status == ServiceControllerStatus.Running;
        }

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
