using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Dts.Runtime;
using WinSCP;

namespace BartekR.WinSCP.CustomTask
{
    public class WinSCPWrapper
    {
        public int MaxFiles { get; set; }

        private ConnectionManager cm;
        private object txn;
        private Session session;

        public WinSCPWrapper(ConnectionManager cm, object txn)
        {
            this.cm = cm;
            this.txn = txn;

            this.StartSession();
        }

        public void StartSession()
        {
            this.session = (Session)this.cm.AcquireConnection(this.txn);
        }

        public void CloseSession()
        {
            this.cm.ReleaseConnection(this.session);
        }

        public IEnumerable<RemoteFileInfo> SearchDirectory(string path, string mask, int logLevel = 0)
        {
            if(this.session == null)
            {
                this.StartSession();
            }
            //Recursivelly enumerate files (EnumerationOptions.AllDirectories)
            return session.EnumerateRemoteFiles(path, mask, EnumerationOptions.AllDirectories);
        }

        public void GetFiles(string remotePath, string localPath, bool remove, TransferOptions transferOptions)
        {
            TransferOperationResult transferResult;
            //transferResult = session.GetFiles("/home/user/*", "d:\\download\\", false, transferOptions);
            transferResult = session.GetFiles(remotePath, localPath, remove, transferOptions);
        }

    }
}
