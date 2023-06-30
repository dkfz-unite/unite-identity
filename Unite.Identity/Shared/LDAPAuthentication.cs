using System;
using System.DirectoryServices.Protocols;
using System.Net;

namespace Unite.Identity.Shared
{
    public class LDAPAuthentication
    {
        // Connection
        private LdapConnection? Connection;
        private string Server;
        private int? Port;
        private string ServiceUserRNA;
        private string ServiceUserPassword;

        // Search params
        private string TargetOU;
        private SearchScope SearchScope;

        public LDAPAuthentication(
            string server,
            string serviceUserRNA,
            string serviceUserPassword,
            string targetOU,
            int? port = null)
        {
            this.TargetOU = targetOU;
            this.SearchScope = SearchScope.Subtree;

            this.Server = server;
            this.Port = port;
            this.ServiceUserRNA = serviceUserRNA;
            this.ServiceUserPassword = serviceUserPassword;
        }

        private void BindServiceConnection()
        {
            var ldapDirId = new LdapDirectoryIdentifier(this.Server);
            if (this.Port != null)
            {
                ldapDirId = new LdapDirectoryIdentifier(this.Server, (int)this.Port);
            }

            var serviceUserCredential = new NetworkCredential(this.ServiceUserRNA, this.ServiceUserPassword);

            this.Connection = new LdapConnection(ldapDirId, serviceUserCredential, AuthType.Basic);

            try
            {
                this.Connection.Bind();
            }
            catch (Exception ex)
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    Console.WriteLine($"BindServiceConnection: {ex}");
                }
            }
        }

        public bool UserCredentialsValid(string userId, string userPassword)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                Console.WriteLine($"UserCredentialsValid-userId: {userId}");
                if (string.IsNullOrEmpty(userPassword))
                {
                    Console.WriteLine("UserCredentialsValid: userPassword empty");
                }
            }

            var userEntry = this.ReadUserEntry(userId);

            var userCredentialsValid = false;
            if (userEntry == null)
            {
                return userCredentialsValid;
            }

            // Relative Distinguished Name
            var targetRNA = userEntry.DistinguishedName;
            var targetCredential = new NetworkCredential(targetRNA, userPassword);

            try
            {
                this.Connection.Bind(targetCredential);

                userCredentialsValid = true;
            }
            catch
            {
                // Credential wrong
            }

            return userCredentialsValid;
        }

        public SearchResultEntry ReadUserEntry(string userLogin)
        {
            bool loginIsEmail = userLogin.Contains("@");
            string searchEntryMember = loginIsEmail ? "mail" : "cn";
            string query = $"({searchEntryMember}={userLogin})";
            string[] attributeList = new string[] { "DistinguishedName", "Mail" };

            var searchRequest = new SearchRequest(this.TargetOU, query, this.SearchScope, attributeList);

            if (this.Connection == null)
            {
                this.BindServiceConnection();
            }

            DirectoryResponse response = null;
            SearchResponse searchResponse = null;
            try
            {
                response = this.Connection.SendRequest(searchRequest);
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    Console.WriteLine($"UserCredentialsValid-SearchRequest-ErrorMessage: {response.ErrorMessage}");
                    Console.WriteLine($"UserCredentialsValid-SearchRequest-ResultCode: {response.ResultCode}");
                }
                searchResponse = (SearchResponse)response;
            }
            catch (Exception ex)
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    Console.WriteLine($"UserCredentialsValid-SearchRequest: {ex}");
                }
            }


            if (searchResponse.Entries.Count < 1)
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    Console.WriteLine($"Found no entries for AD-User");
                }
                return null;
            }

            return searchResponse.Entries[0];
        }
    }
}
