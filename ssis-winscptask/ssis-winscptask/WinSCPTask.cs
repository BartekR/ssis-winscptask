using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Dts.Runtime;

namespace ssis_winscptask
{
    [DtsTask(
        Description = "WinSCP Task - obsługa SFTP, FTP/TLS, SCP za pomocą biblioteki WinSCP",
        DisplayName = "WinSCP Task"
    )]
    public class WinScpTask : Task
    {
        public override DTSExecResult Execute(Connections connections, VariableDispenser variableDispenser, IDTSComponentEvents componentEvents, IDTSLogging log, object transaction)
        {
            return base.Execute(connections, variableDispenser, componentEvents, log, transaction);
        }
    }
}
