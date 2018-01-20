using System.Security;

namespace ZUTSchedule.core
{
    public interface ICredentialManager
    {
        Credential ReadCredential(string applicationName);

        bool SaveCredential(string applicationName, string userName, SecureString password);

        void DeleteCredential(string applicationName);
    }
}
