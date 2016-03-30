using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.SqlServer.Dts.Runtime;
using WinSCP;

namespace BartekR.WinSCP.CustomTask
{
    [DtsConnection(ConnectionType = "WINSCP", DisplayName = "WinSCP Connection Manager", Description = "Connection manager for SFTP/FTPS/FTP using WinSCPNet.dll library", ConnectionContact = "BartekR, bartekr.net")]
    public class WinSCPConnectionManager : ConnectionManagerBase
    {
        #region Configuration variables

        // all descriptions are from official WinSCPNet documentation.

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

        [Category("WinSCP Session Options")]
        [Description("Fingerprint of SSH server host key (or several alternative fingerprints separated by semicolon). It makes WinSCP automatically accept host key with the fingerprint. Mandatory for SFTP/SCP protocol. You can leave the property null, if you set GiveUpSecurityAndAcceptAnySshHostKey.")]
        public string SshHostKeyFingerprint { get; set; }

        //[Category("WinSCP Session Options")]
        //[Description("Server response timeout. Defaults to 15 seconds.")]
        //public TimeSpan Timeout { get; set; }

        [Category("WinSCP Session")]
        [Description("Path to winscp.exe. The default is null, meaning that winscp.exe is looked for in the same directory as this assembly  or in an installation folder. The property as to be set before calling Open.")]
        public string ExecutablePath { get; set; }
        #endregion

        Session s = new Session();

        public WinSCPConnectionManager()
        {
            HostName = "localhost";
            UserName = "tester";
            Password = "password";
            Protocol = Protocol.Sftp;
            SshHostKeyFingerprint = "ssh-rsa 2048 52:4b:46:87:88:a2:90:b5:75:ff:49:ff:57:22:72:42";

            ExecutablePath = @"C:\tools\winscp-sdk\winscp.exe";
        }

        public override object AcquireConnection(object txn)
        {
            SessionOptions so = new SessionOptions
            {
                HostName = this.HostName,
                UserName = this.UserName,
                Password = this.Password,
                SshHostKeyFingerprint = this.SshHostKeyFingerprint
            };

            this.s.ExecutablePath = this.ExecutablePath;

            this.s.Open(so);

            return base.AcquireConnection(txn);
        }

        public override void ReleaseConnection(object connection)
        {
            this.s.Close();
            //base.ReleaseConnection(connection);
        }

        /// <summary>
        /// Simple validation. Runtime only!
        /// </summary>
        /// <param name="infoEvents"></param>
        /// <returns></returns>
        public override DTSExecResult Validate(IDTSInfoEvents infoEvents)
        {
            if (string.IsNullOrEmpty(this.HostName) || string.IsNullOrEmpty(this.UserName) || string.IsNullOrEmpty(this.Password))
            {
                infoEvents.FireError(0, "WinSCP Connection Manager", "Hostname, username and password are mandatory.", string.Empty, 0);
                return DTSExecResult.Failure;
            }
            else
            {
                return DTSExecResult.Success;
            }
        }

    }
}
