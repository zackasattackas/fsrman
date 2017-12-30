using System;
using System.Net;

namespace fsrman
{
    internal class FsrmanOptions
    {
        public bool ListOnly { get; }
        public bool SendTestEmail { get; }
        public bool Modify { get; }
        public bool IsRemoteServer { get; }
        public string ComputerName { get; }
        public NetworkCredential Credentials { get; }

        public FsrmanOptions(CmdLineApp app)
        {
            app.Arguments.TryGetValue("-l", out var listOnly);
            app.Arguments.TryGetValue("-t", out var testEmail);
            app.Arguments.TryGetValue("-m", out var modify);
            app.Arguments.TryGetValue("-c", out var computerName);
            app.Arguments.TryGetValue("-u", out var username);
            app.Arguments.TryGetValue("-p", out var password);

            this.ListOnly = bool.Parse(listOnly ?? bool.FalseString);
            this.SendTestEmail = bool.TryParse(testEmail, out _) || testEmail != null;
            this.Modify = bool.Parse(modify ?? bool.FalseString);
            this.IsRemoteServer = computerName !=  null && !computerName.Equals(Environment.MachineName, StringComparison.CurrentCultureIgnoreCase);
            this.ComputerName = computerName ?? Environment.MachineName;

            string domain = null;

            if (username == null)
            {
                return;
            }

            if (username.Contains("\\"))
            {
                var split = username.Split('\\');
                domain = split[0];
                username = split[1];
            }

            this.Credentials = new NetworkCredential(username, password?.ToSecureString() ?? Helpers.GetSecurePassword(), domain);
        }
    }
}
