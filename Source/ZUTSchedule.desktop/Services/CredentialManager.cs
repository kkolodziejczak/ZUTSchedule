using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using ZUTSchedule.core;

namespace ZUTSchedule.desktop
{
    /// <summary>
    /// My spin on https://www.nuget.org/packages/Meziantou.Framework.Win32.CredentialManager
    /// Instead of using strings my modification uses SecureStrings
    /// </summary>
    public class CredentialManager : ICredentialManager
    {
        /// <summary>
        /// Reads Windows <see cref="Credential"/> for application called <paramref name="applicationName"/>
        /// </summary>
        /// <param name="applicationName">Application name to get <see cref="Credential"/> for</param>
        /// <returns><see cref="Credential"/> for <paramref name="applicationName"/></returns>
        public Credential ReadCredential(string applicationName)
        {
            IntPtr nCredPtr;
            bool read = CredRead(applicationName, Credential.CredentialType.Generic, 0, out nCredPtr);
            if (read)
            {
                using (CriticalCredentialHandle critCred = new CriticalCredentialHandle(nCredPtr))
                {
                    return ReadCredential(critCred.GetCredential());
                }
            }
            return null;
        }

        private unsafe Credential ReadCredential(CREDENTIAL credential)
        {
            string applicationName = Marshal.PtrToStringUni(credential.TargetName);
            string userName = Marshal.PtrToStringUni(credential.UserName);
            SecureString secret = null;
            if (credential.CredentialBlob != IntPtr.Zero)
            {
                secret = new SecureString((char*)credential.CredentialBlob, (int)credential.CredentialBlobSize / 2);
            }

            return new Credential(credential.Type, applicationName, userName, secret);
        }

        /// <summary>
        /// Saves <see cref="Credential"/> for later use
        /// </summary>
        /// <param name="applicationName">Application name to save <see cref="Credential"/> for</param>
        /// <param name="userName">User name</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="CredWriteException"/>
        public bool SaveCredential(string applicationName, string userName, SecureString password)
        {
            // *2 to count for char size of 2 bytes.
            var passwordSizeInBytes = password.Length * 2;
            if (passwordSizeInBytes > 512)
                throw new ArgumentOutOfRangeException("password", "The password has exceeded 512 bytes.");

            CREDENTIAL credential = new CREDENTIAL();
            int lastError = -1;
            try
            {
                credential = new CREDENTIAL
                {
                    AttributeCount = 0,
                    Attributes = IntPtr.Zero,
                    Comment = IntPtr.Zero,
                    TargetAlias = IntPtr.Zero,
                    Type = Credential.CredentialType.Generic,
                    Persist = (UInt32)CredentialPersistence.LocalMachine,
                    CredentialBlobSize = (UInt32)passwordSizeInBytes,
                    TargetName = Marshal.StringToCoTaskMemUni(applicationName),
                    CredentialBlob = Marshal.SecureStringToGlobalAllocUnicode(password),
                    UserName = Marshal.StringToCoTaskMemUni(userName)
                };

                bool written = CredWrite(ref credential, 0);
                lastError = Marshal.GetLastWin32Error();
                if (written)
                    return true;
            }
            finally
            {
                Marshal.FreeCoTaskMem(credential.TargetName);
                Marshal.ZeroFreeGlobalAllocUnicode(credential.CredentialBlob);
                Marshal.FreeCoTaskMem(credential.UserName);
            }

            Logger.Log($"CredWrite failed with the error code {lastError}.", Logger.LogLevel.Error);
            throw new CredWriteException($"CredWrite failed with the error code {lastError}.");
        }

        /// <summary>
        /// Removes credentials for application with name <paramref name="applicationName"/>
        /// </summary>
        /// <param name="applicationName"></param>
        /// <exception cref="CredDeleteException"/>
        public void DeleteCredential(string applicationName)
        {
            var success = CredDelete(applicationName, Credential.CredentialType.Generic, 0);
            if (!success)
            {
                var lastError = Marshal.GetLastWin32Error();
                Logger.Log($"CreadDelete failed with the error code {lastError}.", Logger.LogLevel.Error);
                throw new CredDeleteException($"CreadDelete failed with the error code {lastError}.");
            }
        }

        [DllImport("Advapi32.dll", EntryPoint = "CredDeleteW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool CredDelete(string target, Credential.CredentialType type, int reservedFlag);

        [DllImport("Advapi32.dll", EntryPoint = "CredReadW", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool CredRead(string target, Credential.CredentialType type, int reservedFlag, out IntPtr credentialPtr);

        [DllImport("Advapi32.dll", EntryPoint = "CredWriteW", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool CredWrite([In] ref CREDENTIAL userCredential, [In] UInt32 flags);

        [DllImport("Advapi32.dll", EntryPoint = "CredFree", SetLastError = true)]
        static extern bool CredFree([In] IntPtr cred);

        private enum CredentialPersistence : uint
        {
            Session = 1,
            LocalMachine,
            Enterprise
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CREDENTIAL
        {
            public UInt32 Flags;
            public Credential.CredentialType Type;
            public IntPtr TargetName;
            public IntPtr Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public UInt32 CredentialBlobSize;
            public IntPtr CredentialBlob;
            public UInt32 Persist;
            public UInt32 AttributeCount;
            public IntPtr Attributes;
            public IntPtr TargetAlias;
            public IntPtr UserName;
        }

        sealed class CriticalCredentialHandle : CriticalHandleZeroOrMinusOneIsInvalid
        {
            public CriticalCredentialHandle(IntPtr preexistingHandle)
            {
                SetHandle(preexistingHandle);
            }

            public CREDENTIAL GetCredential()
            {
                if (!IsInvalid)
                {
                    CREDENTIAL credential = (CREDENTIAL)Marshal.PtrToStructure(handle, typeof(CREDENTIAL));
                    return credential;
                }

                throw new InvalidOperationException("Invalid CriticalHandle!");
            }

            protected override bool ReleaseHandle()
            {
                if (!IsInvalid)
                {
                    CredFree(handle);
                    SetHandleAsInvalid();
                    return true;
                }

                return false;
            }
        }

        #region Custom Exceptions

        public class CredWriteException : Exception
        {
            public CredWriteException()
            {
            }

            public CredWriteException(string message) : base(message)
            {
            }

            public CredWriteException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected CredWriteException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        public class CredDeleteException : Exception
        {
            public CredDeleteException()
            {
            }

            public CredDeleteException(string message) : base(message)
            {
            }

            public CredDeleteException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected CredDeleteException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        #endregion
    }


}
