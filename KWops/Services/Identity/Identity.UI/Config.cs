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
                    ClientSecrets = { new Secret("SuperSecretClientSecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "http://localhost:7890/" },
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "devops.read", "hr.read" }
                },
                new Client
                {
                    ClientId = "swagger.devops",
                    ClientName = "Swagger UI for DevOps Api",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RedirectUris = {"https://localhost:6001/swagger/oauth2-redirect.html"},
                    AllowedCorsOrigins = {"https://localhost:8001"},
                    AllowedScopes = {"devops.read", "manage"}
                },
                new Client
                {
                    ClientId = "swagger.hr",
                    ClientName = "Swagger UI for Human Relations Api",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RedirectUris = {"https://localhost:5001/swagger/oauth2-redirect.html"},
                    AllowedCorsOrigins = {"https://localhost:5001"},
                    AllowedScopes = {"hr.read", "manage"}
                },
                new Client
                {
                    ClientId = "kwops.mobile",
                    ClientName = "KWops mobile application",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = { new Secret("MobileClientSecret".Sha256()) },
                    RedirectUris = {"myapp://xamarincallback"},
                    AllowedScopes = {"openid", "devops.read", "manage"}
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