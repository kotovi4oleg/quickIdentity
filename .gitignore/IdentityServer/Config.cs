// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("test_identity", "my custom identity", new List<string> {
                    "phone",
                    "sex"
                }), 
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("afcpayroll", "Acme Fireworks Co. payroll")
                {
                    UserClaims = { JwtClaimTypes.Name, JwtClaimTypes.Email },
                    ApiSecrets = new List<Secret>
                    {
                        new Secret("afcpayroll".Sha256())
                    }
                },
                new ApiResource("afcpayroll2", "Acme Fireworks Co. payroll")
                {
                    UserClaims = { JwtClaimTypes.Name, JwtClaimTypes.Email },
                    ApiSecrets = new List<Secret>
                    {
                        new Secret("afcpayroll2".Sha256())
                    }
                },
                new ApiResource("identityprovider", "identityprovider")
                {
                    UserClaims = { JwtClaimTypes.Name, JwtClaimTypes.Email },
                    ApiSecrets = new List<Secret>
                    {
                        new Secret("identityprovider".Sha256())
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "identityprovider", "afcpayroll" }
                },
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "afcpayroll", IdentityServerConstants.StandardScopes.OpenId, "test_identity" }
                },
                // OpenID Connect implicit flow client (MVC)
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AccessTokenLifetime = 60, //seconds
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:57002/home/signin-oidc", "http://localhost:5002/home/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:57002/signout-callback-oidc", "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,

                        //switching to hybrid flow access api + identity
                        "afcpayroll",
                        "afcpayroll2",
                        "identityprovider"
                    },
                    RefreshTokenUsage = TokenUsage.ReUse,
                    AllowOfflineAccess = true,
                    RequireConsent = false
                }
            };
        }
    }
}