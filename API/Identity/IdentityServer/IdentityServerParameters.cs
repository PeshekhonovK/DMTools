using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Options;

namespace IdentityServer
{
    public class IdentityServerParameters
    {
        private IdentityConfig Configuration { get; }
        
        public IdentityServerParameters(IOptions<IdentityConfig> configuration)
        {
            this.Configuration = configuration.Value;
        }
        
        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        // scopes define the API resources in your system
        public IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("DMToolsAPI", "DMTools API")
            };
        }

        // client want to access resources (aka scopes)
        public IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                // resource owner password grant client
                new Client
                {
                    ClientId = "DMToolsAPI",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowOfflineAccess = true,
                    ClientSecrets =
                    {
                        new Secret(this.Configuration.Secret.Sha256())
                    },
                    AllowedScopes = { "DMToolsAPI", IdentityServerConstants.StandardScopes.OfflineAccess },
                }
            };
        }
    }
}