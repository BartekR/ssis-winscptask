using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Wrap = Microsoft.SqlServer.Dts.Runtime.Wrapper;
using System.Data.SqlClient;
using WinSCP;

using Microsoft.SqlServer.Dts.Runtime;

namespace BartekR.WinSCP.CustomTask
{
    public class SQLServerWrapper
    {
        private ConnectionManager cm;
        private object txn;

        private DbConnection connection;

        public SQLServerWrapper(ConnectionManager cm, object txn)
        {
            this.cm = cm;
            this.txn = txn;

            this.StartSession();
        }

        public void StartSession()
        {
            //object connection = cmS.AcquireConnection(transaction);
            // http://agilebi.com/jwelch/2011/08/17/oledb-connections-in-script-tasks/
            // https://blogs.msdn.microsoft.com/mattm/2008/08/22/accessing-oledb-connection-managers-in-a-script/
            if (this.cm.CreationName == "OLEDB")
            {
                Wrap.IDTSConnectionManagerDatabaseParameters100 cm100 = this.cm.InnerObject as Wrap.IDTSConnectionManagerDatabaseParameters100;
                //this.connection = cm100.GetConnectionForSchema() as OleDbConnection;
                this.connection = cm100.GetConnectionForSchema() as DbConnection;
            }
            else
            {
                //this.connection = (SqlConnection)this.cm.AcquireConnection(this.txn)
                //SqlConnection
                this.connection = this.cm.AcquireConnection(this.txn) as DbConnection;
            }

            if (this.connection.State == ConnectionState.Closed)
            {
                this.connection.Open();
            }
        }

        public void CloseSession()
        {
            if(this.connection.State == ConnectionState.Open)
            {
                this.connection.Close();
            }
            this.cm.ReleaseConnection(this.txn);
        }

        public void getData()
        {
            DbCommand cmd = this.connection.CreateCommand();
            cmd.CommandText = "SELECT name FROM sys.databases";
            using (var reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    string db = reader.GetString(0);
                }
            }

        }

        public LocalFileInfo getLocalFiles()
        {
            LocalFileInfo localFiles = new LocalFileInfo();
            return localFiles;
        }

        public void SaveServerFileNamesToDatabase(IEnumerable<RemoteFileInfo> remoteFiles)
        {
            foreach (RemoteFileInfo fileInfo in remoteFiles)
            {
                DbCommand cmd = this.connection.CreateCommand();
                cmd.CommandType = CommandType.Text;

                //TODO: table name parametrization
                cmd.CommandText = @"
                INSERT INTO dbo.ServerFiles (RemoteFilePath, RemoteDirectoryName, LocalFileName)
                VALUES (?, ?, ?);";

                var paramRemoteFilePath = cmd.CreateParameter();
                paramRemoteFilePath.ParameterName = "RemoteFilePath";
                paramRemoteFilePath.Value = fileInfo.FullName;

                var paramRemoteDirectoryName = cmd.CreateParameter();
                paramRemoteDirectoryName.ParameterName = "RemoteDirectoryName";
                // GetDirectoryName returns path with backslash
                paramRemoteDirectoryName.Value = System.IO.Path.GetDirectoryName(fileInfo.FullName).Replace("\\", "/");

                var paramLocalFileName = cmd.CreateParameter();
                paramLocalFileName.ParameterName = "LocalFileName";
                paramLocalFileName.Value = fileInfo.Name.Replace("/", "_");

                cmd.Parameters.Add(paramRemoteFilePath);
                cmd.Parameters.Add(paramRemoteDirectoryName);
                cmd.Parameters.Add(paramLocalFileName);

                cmd.ExecuteNonQuery();
            }
        }

        //TODO: parametrization, SQL Injection prevention
        public void ClearTable(string downloadedFilesTable, bool truncateDownloadedFilesTable)
        {
            if(truncateDownloadedFilesTable == true)
            {
                DbCommand cmd = this.connection.CreateCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = @"TRUNCATE TABLE " + downloadedFilesTable;

                cmd.ExecuteNonQuery();
            }
            
        }
    }
}
