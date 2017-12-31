using System;
using MsftFsrm;
#if DEBUG
using System.Diagnostics;
#endif

namespace fsrman
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                CmdLineApp.PrintCopyright();

                var app = new CmdLineApp(CmdLineApp.NormalizeArgs(args));

                if (app.IsShowingHelp)
                {
                    if (app.FirstArg == "-?")
                    {
                        CmdLineApp.PrintHelp();
                        return;
                    }

                    CmdLineApp.PrintOptionDetails(app.FirstArg);
                    return;
                }
                
                var cliOptions = new FsrmanOptions(app);

                if (!FsrmUtil.IsServiceRunning(cliOptions.ComputerName, cliOptions.Credentials))
                {
                    throw new ApplicationException($"The File Server Resource Manager service on computer {cliOptions.ComputerName} is either not installed or is not running");
                }

                var currentSettings = new FsrmSettings(cliOptions.ComputerName, cliOptions.Credentials);

                Console.WriteLine(string.Join(", ", currentSettings.ReportClassificationFormat));

                #region -l List

                if (cliOptions.ListOnly)
                {
                    const string fmt = "{0,-10} : {1}";
                    
                    Console.WriteLine(fmt, "Server", currentSettings.ComputerName);
                    Console.WriteLine(fmt, "AdminEmail", currentSettings.AdminEmailAddress);
                    Console.WriteLine(fmt, "FromEmail", currentSettings.FromEmailAddress);
                    Console.WriteLine(fmt, "SmtpServer", currentSettings.SmtpServer);
                }

                #endregion

                #region -t Test Email

                else if (cliOptions.SendTestEmail)
                {
                    if (app.Arguments.TryGetValue("-t", out var toEmail) && bool.TryParse(toEmail, out _))
                    {                        
                        toEmail = currentSettings.AdminEmailAddress;
                    }
                    if (string.IsNullOrEmpty(toEmail))
                    {
                        throw new ArgumentNullException(nameof(toEmail), "A \"To\" e-mail address was not provided, and no administrator recipient is currently configured.");
                    }

                    currentSettings.SendTestEmail(toEmail);

                    Console.WriteLine($"A test notification has been sent to {toEmail}.");
                }

                #endregion
        
                #region -m Modify

                else if (cliOptions.Modify)
                {
                    app.Arguments.TryGetValue("-a", out var adminEmail);
                    app.Arguments.TryGetValue("-f", out var fromEmail);
                    app.Arguments.TryGetValue("-s", out var smtpServer);

                    currentSettings.AdminEmailAddress = adminEmail ?? currentSettings.AdminEmailAddress;
                    currentSettings.FromEmailAddress = fromEmail ?? currentSettings.FromEmailAddress;
                    currentSettings.SmtpServer = smtpServer ?? currentSettings.SmtpServer;

                    currentSettings.SaveChanges();
                }

                #endregion

                Console.WriteLine("\r\nThe operation completed successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e.Message}.");
            }

#if DEBUG
            PauseConsole();
            void PauseConsole()
            {
                if (!Debugger.IsAttached)
                {
                    return;
                }

                Console.Write("\r\nPress any key to exit...");
                Console.ReadKey();
            }
#endif
        }


    }
}
