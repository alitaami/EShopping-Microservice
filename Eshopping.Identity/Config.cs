// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace Eshopping.Identity
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
                new ApiScope("CatalogApi"),
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {       
                // Lists of microservices can go here.
                new ApiResource(/* Audience */ "Catalog", "Catalog-Api")
                {
                    Scopes = {"CatalogApi" }
                }
            };
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName = "Catalog api client",
                    ClientId = "ClientApiClient",
                    ClientSecrets = {new Secret("1db79381-2d1a-4b3d-b88c-e7116fa7aaa3".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"CatalogApi" }

                }
            };
    }
}