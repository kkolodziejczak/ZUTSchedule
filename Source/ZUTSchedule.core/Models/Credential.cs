using System.Security;

namespace ZUTSchedule.core
{
    public class Credential
    {
        public enum CredentialType
        {
            Generic = 1,
            DomainPassword,
            DomainCertificate,
            DomainVisiblePassword,
            GenericCertificate,
            DomainExtended,
            Maximum,
            MaximumEx = Maximum + 1000,
        }

        public CredentialType Type { get; }

        public string ApplicationName { get; }

        public string UserName { get; }

        public SecureString Password { get; }

        public Credential(CredentialType credentialType, string applicationName, string userName, SecureString password)
        {
            ApplicationName = applicationName;
            UserName = userName;
            Password = password;
            Type = credentialType;
        }

        public override string ToString()
        {
            return $"CredentialType: {Type}, ApplicationName: {ApplicationName}, UserName: {UserName}";
        }
    }
}
