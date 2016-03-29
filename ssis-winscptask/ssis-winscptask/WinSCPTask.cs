using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Dts.Runtime;
using System.ComponentModel;
using WinSCP;

namespace BartekR.WinSCP.CustomTask
{
    [DtsTask(
        Description = "WinSCP Task - SFTP, FTPS, FTP handling using WinSCP .NET library",
        DisplayName = "WinSCP Task"
    )]
    public class WinScpTask : Task
    {
       
        public WinScpTask()
        {
            
        }

        public string WinSCPConnectionManagerName { get; set; }
        public string SQLServerConnectionManagerName { get; set; }


        public override void InitializeTask(Connections connections, VariableDispenser variableDispenser, IDTSInfoEvents events, IDTSLogging log, EventInfos eventInfos, LogEntryInfos logEntryInfos, ObjectReferenceTracker refTracker)
        {
            base.InitializeTask(connections, variableDispenser, events, log, eventInfos, logEntryInfos, refTracker);
        }

        public override DTSExecResult Validate(Connections connections, VariableDispenser variableDispenser, IDTSComponentEvents componentEvents, IDTSLogging log)
        {
            try
            {
                ConnectionManager cmW = connections[this.WinSCPConnectionManagerName];
                //return DTSExecResult.Success;
            }
            catch (System.Exception e)
            {
                componentEvents.FireError(0, "WinSCPTask", "Invalid WinSCP connection manager. " + e.Message, "", 0);
                return DTSExecResult.Failure;
            }

            try
            {
                ConnectionManager cmS = connections[this.SQLServerConnectionManagerName];
                //return DTSExecResult.Success;
            }
            catch (System.Exception e)
            {
                componentEvents.FireError(0, "WinSCPTask", "Invalid SQL Server connection manager. " + e.Message, "", 0);
                return DTSExecResult.Failure;
            }

            return DTSExecResult.Success;

        }

        public override DTSExecResult Execute(Connections connections, VariableDispenser variableDispenser, IDTSComponentEvents componentEvents, IDTSLogging log, object transaction)
        {
            try
            {
                ConnectionManager cmW = connections[this.WinSCPConnectionManagerName];
                object connection = cmW.AcquireConnection(transaction);
                
            }
            catch (System.Exception e)
            {
                componentEvents.FireError(0, "WinSCPTask - WinSCPConnection", e.Message, "", 0);
                return DTSExecResult.Failure;
            }

            try
            {
                ConnectionManager cmS = connections[this.SQLServerConnectionManagerName];
                object connection = cmS.AcquireConnection(transaction);

            }
            catch (System.Exception e)
            {
                componentEvents.FireError(0, "WinSCPTask - SqlServerConnection", e.Message, "", 0);
                return DTSExecResult.Failure;
            }

            return DTSExecResult.Success;
            //return base.Execute(connections, variableDispenser, componentEvents, log, transaction);
        }
    }
}
