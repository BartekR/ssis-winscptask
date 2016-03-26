using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Dts.Runtime;
using System.ComponentModel;
using WinSCP;

namespace ssis_winscptask
{
    [DtsTask(
        Description = "WinSCP Task - SFTP, FTP/TLS, SCP handling using WinSCP .NET library",
        DisplayName = "WinSCP Task"
    )]
    public class WinScpTask : Task
    {
        #region Configuration variables

        [Category("WinSCP Session Options")]
        [Description("FTPS mode")]
        public FtpSecure FtpSecure { get; set; }

        [Category("WinSCP Session Options")]
        [Description("Name of the host to connect to. Mandatory property.")]
        public string HostName { get; set; }

        [Category("WinSCP Session Options")]
        [Description("Username for authentication. Mandatory property.")]
        public string UserName { get; set; }

        [Category("WinSCP Session Options")]
        [Description("Password for authentication.")]
        [PasswordPropertyText(true)]
        public string Password { get; set; }

        [Category("WinSCP Session Options")]
        [Description("Protocol to use for the session.")]
        public Protocol Protocol { get; set; }

        [Category("WinSCP Session Options")]
        [Description("Port number to connect to. Keep default 0 to use default port for the protocol.")]
        public int PortNumber { get; set; }

        //[Category("WinSCP Session Options")]
        //[Description("Server response timeout. Defaults to 15 seconds.")]
        //public TimeSpan Timeout { get; set; }

        #endregion

        public override void InitializeTask(Connections connections, VariableDispenser variableDispenser, IDTSInfoEvents events, IDTSLogging log, EventInfos eventInfos, LogEntryInfos logEntryInfos, ObjectReferenceTracker refTracker)
        {
            base.InitializeTask(connections, variableDispenser, events, log, eventInfos, logEntryInfos, refTracker);
        }

        public override DTSExecResult Validate(Connections connections, VariableDispenser variableDispenser, IDTSComponentEvents componentEvents, IDTSLogging log)
        {
            return base.Validate(connections, variableDispenser, componentEvents, log);
        }

        public override DTSExecResult Execute(Connections connections, VariableDispenser variableDispenser, IDTSComponentEvents componentEvents, IDTSLogging log, object transaction)
        {
            return base.Execute(connections, variableDispenser, componentEvents, log, transaction);
        }
    }
}
