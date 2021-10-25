// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace Identity.UI
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("devops.read", "Read acces on DevOps Api"),
                new ApiScope("hr.read", "Read acces on HumanRelations Api"),
                new ApiScope("manage", "Write access"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "kwops.cli",
                    ClientName = "KWops Command Line Interface",
                    ClientSecrets = 
                    { 
                        new Secret("SuperSecretClientSecret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "http://localhost:7890/" },
                    AllowOfflineAccess = true,
                    AllowedScopes = 
                    { 
                        "openid", "profile", "devops.read", "hr.read"
                    }
                }
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("devops", "DevOps API")
                {
                    Scopes = {"devops.read", "manage"},
                },
                new ApiResource("hr", "Human Relations API")
                {
                    Scopes = {"hr.read", "manage"}
                }
            };
    }
}