using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace fsrman
{
    internal class CmdLineApp
    {
        public string FirstArg { get; }
        public Dictionary<string, string> Arguments { get; }
        public bool IsShowingHelp { get; }

        public CmdLineApp(string[] args)
        {
            var arguments = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            foreach (var arg in args)
            {
                if (arg[0] != '-')
                {
                    continue;
                }
                var split = Regex.Split(arg, "^([^:]+):").Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                if (split.Length > 2)
                {
                    throw new ArgumentException($"Failed to parse argument: {arg}");
                }
                arguments.Add(split[0], split.Length == 1 ? bool.TrueString : split[1]);
            }

            this.Arguments = arguments;
            this.FirstArg = arguments.First().Key;
            this.IsShowingHelp = this.Arguments.ContainsKey("-?");
        }
        public static string[] NormalizeArgs(string[] args)
        {
            var copyArgs = args;
            for (var i = 0; i < copyArgs.Length; i++)
            {
                if (Regex.IsMatch(args[i], "^([-]|[/])([hH][eE][lL][pP])"))
                {
                    copyArgs[i] = "-?";
                }
                if (copyArgs[i][0] == '/')
                {
                    copyArgs[i] = '-' + copyArgs[i].Substring(1);
                }
            }
            return args;
        }
        public static void PrintCopyright()
        {
            Console.WriteLine("\r\nFsrman v1 - File Server Resource Manager Command Line Tool");
            Console.WriteLine("Zack Bolin (2017)\r\n");
        }
        public static void PrintHelp()
        {
            PrintUsage();
            Console.WriteLine("\r\nOPTIONS\r\n");

            PrintOption("-c", "The name or IP address of the remote computer to connect to.");
            PrintOption("-u", "The username of an account to connect to. For a domain account, format must be Domain\\Username.");
            PrintOption("-p", "The password of the user account. If omitted, a secure prompt will be used to retrieve the password.");
            PrintOption("-l", "List the current FSRM e-mail settings.");
            PrintOption("-t", "Sends a test email using the current configured FSRM settings.");
            PrintOption("-m", "Modify the FSRM e-mail settings.");
            PrintOption("-?", "Displays this help information. Use \"<command> -?\" for more information and command arguments."); //HELP
        }
        public static void PrintUsage()
        {
            Console.WriteLine("USAGE: fsrman [-c -u [-p]] [options]");
        }
        public static void PrintOption(string template, string description)
        {
            const string format = "    {0,-6} ";
            Console.Write(format, template);
            PrintLines(11, description.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            Console.WriteLine();
        }
        private static void PrintLines(int cursorLeft, params string[] lines)
        {
            foreach (var line in lines)
            {
                Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                Console.WriteLine(GetWrappedText(cursorLeft, line));
            }
        }
        public static void PrintOptionDetails(string option)
        {
            switch (option)
            {
                case "-c":
                    PrintOptionUsage("-c:<server>", "The name or IP address of the remote computer to connect to.");
                    break;
                case "-u":
                    PrintOptionUsage("-u:<username>", "The username of an account to connect to. For a domain account, format must be Domain\\Username.");
                    break;
                case "-p":
                    PrintOptionUsage("-p:<password>", "The password of the user account. If omitted, a secure prompt will be used to retrieve the password.");
                    break;
                case "-l":
                    PrintOptionUsage("-l", "Lists the current FSRM e-mail settings.");
                    break;
                case "-t":
                    PrintOptionUsage("-t[:<toEmail>]", "Sends a test email using the current configured FSRM settings.");
                    PrintOptionsLabel();
                    PrintOption("-t", "The e-mail address to send a test notification to. If omitted, the current administrator recipient is used.");
                    break;
                case "-m":
                    PrintOptionUsage("-m [-a:<adminEmail>] [-f:<fromEmail>] [-s:<smtpServer>]", "Modify the FSRM e-mail settings.");
                    PrintOptionsLabel();
                    PrintOption("-a", "The default adminstrator recipients for FSRM e-mail notifications.");
                    PrintOption("-f", "The default \"From\" e-mail address.");
                    PrintOption("-s", "The SMTP server name or IP address.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            void PrintOptionsLabel()
            {
                const string optionsLabel = "\r\nOPTIONS:\r\n";
                Console.WriteLine(optionsLabel);
            }
        }
        private static void PrintOptionUsage(string usage, string description)
        {
            const string usageFmt = "USAGE: fsrman {0}";

            var lines = description.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            PrintLines(0, lines);
            Console.WriteLine();

            lines = usage.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Console.Write(usageFmt, string.Empty);
            PrintLines(Console.CursorLeft, lines);
        }
        private static string GetWrappedText(int startPosition, string s)
        {
            if (startPosition + s.Length < Console.BufferWidth)
            {
                return s;
            }

            var builder = new StringBuilder();
            var chunk = s
                .Substring(0, Console.BufferWidth - startPosition)
                .Substring(sub => sub.Substring(0, sub.LastIndexOf(" ", StringComparison.Ordinal)));
            builder.AppendLine(chunk.TrimEnd());
            builder.Append(new string(' ', startPosition + s.GetLeadingWhitespace()));
            builder.Append(GetWrappedText(startPosition, s.Substring(chunk.Length).Trim()));
            return builder.ToString();
        }
    }
}
