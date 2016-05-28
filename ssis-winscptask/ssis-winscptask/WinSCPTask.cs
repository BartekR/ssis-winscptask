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

        [Category("WinSCPTask")]
        public string WinSCPConnectionManagerName { get; set; }

        [Category("WinSCPTask")]
        public string SQLServerConnectionManagerName { get; set; }

        [Category("WinSCPTask")]
        [Description("List of directories on server to look for files")]
        public string DirectoryPath { get; set; }

        [Category("WinSCPTask")]
        [Description("Table name with directories on server to iterate over")]
        public string DirectoryTable { get; set; }

        [Category("WinSCPTask")]
        public string DirectoryMask { get; set; }

        [Category("WinSCPTask")]
        [Description("Files downloaded from server are described in 'DownloadedFilesTable' table; Should it be cleared during each import?")]
        public bool TruncateDownloadedFilesTable { get; set; }

        [Category("WinSCPTask")]
        public string ServerFilesTable { get; set; }

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
            WinSCPWrapper p;
            SQLServerWrapper s;
            IEnumerable<RemoteFileInfo> remoteFiles;

            // defaults for now
            ServerFilesTable = "dbo.ServerFiles";

            // connect to servers
            try
            {
                p = new WinSCPWrapper(connections[this.WinSCPConnectionManagerName], transaction);
            }
            catch (System.Exception e)
            {
                componentEvents.FireError(0, "WinSCPTask.Execute - WinSCPConnection", e.Message, "", 0);
                return DTSExecResult.Failure;
            }

            // compare remote files with local metadata; if using OLEDB - it can be tricky
            try
            {
                s = new SQLServerWrapper(connections[this.SQLServerConnectionManagerName], transaction);
            }
            catch (System.Exception e)
            {
                componentEvents.FireError(0, "WinSCPTask.Execute - SqlServerConnection", e.Message, "", 0);
                return DTSExecResult.Failure;
            }

            // get remote files
            remoteFiles = p.SearchDirectory(DirectoryPath, null, 0);
            
            // clear DownloadedFiles table (optional)
            s.ClearTable(ServerFilesTable, TruncateDownloadedFilesTable);

            // save file names to database
            s.SaveServerFileNamesToDatabase(remoteFiles);

            // check for files to download (compare with existing metadata)

            // download files

            // close connections
            p.CloseSession();

            return DTSExecResult.Success;
        }
    }
}
