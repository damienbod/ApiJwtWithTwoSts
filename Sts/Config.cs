using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace StsServerIdentity
{
    public class Config
    {
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("scope_used_for_api_in_protected_zone",  "Scope for the scope_used_for_api_in_protected_zone"),
                new ApiScope("scope_a",  "Scope for a")
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("ProtectedApiResourceA")
                {
                    DisplayName = "API protected",
                    ApiSecrets =
                    {
                        new Secret("api_resource_in_protected_zone_secret".Sha256())
                    },
                    Scopes = { "scope_used_for_api_in_protected_zone", "scope_a" },
                    UserClaims = { "role", "admin", "user" }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "CC_STS_A",
                    ClientName = "CC_STS_A",
                    ClientSecrets = new List<Secret> { new Secret { Value = "cc_secret".Sha256() } },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = new List<string> { "scope_used_for_api_in_protected_zone", "scope_a" }
                },
                new Client
                {
                    ClientName = "codeflowpkceclient",
                    ClientId = "codeflowpkceclient",
                    ClientSecrets = {new Secret("codeflow_pkce_client_secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = true,
                    AllowOfflineAccess = true,
                    AlwaysSendClientClaims = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris = {
                        "https://localhost:44360/signin-oidc"
                    },
                    PostLogoutRedirectUris = {
                        "https://localhost:44360/signout-callback-oidc"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    }
                }
            };
        }
    }
}