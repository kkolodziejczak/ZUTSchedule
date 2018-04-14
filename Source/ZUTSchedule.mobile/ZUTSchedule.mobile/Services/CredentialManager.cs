using System.Security;
using ZUTSchedule.core;

namespace ZUTSchedule.mobile
{
    public class CredentialManager : ICredentialManager
    {

        public void DeleteCredential(string applicationName)
        {
            throw new System.NotImplementedException();
        }

        public Credential ReadCredential(string applicationName)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveCredential(string applicationName, string userName, SecureString password)
        {
            throw new System.NotImplementedException();
        }
    }
}