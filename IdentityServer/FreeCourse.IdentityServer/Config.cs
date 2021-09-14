// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace FreeCourse.IdentityServer
{
    public static class Config
    {

        //Auth Server uygulamasının sorumlu olduğu resource’leri yani API’leri ifade eder.
        public static IEnumerable<ApiResource> ApiResources =>
                   new ApiResource[]
                   {
                       new ApiResource("resource_catalog"){Scopes={"catalog_full_permission"}},
                       new ApiResource("resource_photostock"){Scopes={"photostock_full_permission"}},
                       new ApiResource("resource_basket"){Scopes={"basket_full_permission"}},
                       new ApiResource("resource_discount"){Scopes={"discount_full_permission"}},
                       new ApiResource("resource_order"){Scopes={"order_full_permission"}},
                       new ApiResource("resource_fakepayment"){Scopes={"fakepayment_full_permission"}},
                       new ApiResource("resource_gateway"){Scopes={"gateway_full_permission"}},
                       new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
                   };

        //
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource(){Name="roles",DisplayName="Roles",Description="Kullanıcı Rolleri",UserClaims=new string[]{ "role"} },
                   };

        //Kullanılacak olan izinler tanımlanır.
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_full_permission","Catalog API için full erişim"),
                new ApiScope("photostock_full_permission","Photo Stock API için full erişim"),
                new ApiScope("basket_full_permission","Basket API için full erişim"),
                new ApiScope("discount_full_permission","Discount API için full erişim"),
                new ApiScope("order_full_permission","Order API için full erişim"),
                new ApiScope("fakepayment_full_permission","Payment API için full erişim"),
                new ApiScope("gateway_full_permission","Payment API için full erişim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        //Api'leri kullancak olan clientlar tanımlanır.
        //Yukarıda tanımlanan yetkiler(scope) ile ilişkilendirilir.
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "WebMvcClient",
                    ClientName = "Asp.Net Core Mvc",
                    AllowedGrantTypes = GrantTypes.ClientCredentials, //Client Creadetials(Machine to Machine) kullanılacağı set edilmiş.
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "catalog_full_permission", "photostock_full_permission", "gateway_full_permission", IdentityServerConstants.LocalApi.ScopeName } //Yetkisi olduğu scope lar tanımlanmış.
                },
                new Client
                {
                    ClientId = "WebMvcClientForUser",
                    ClientName = "Asp.Net Core Mvc",
                    AllowOfflineAccess=true,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "basket_full_permission",
                        "order_full_permission",
                        "gateway_full_permission",
                        "discount_full_permission",
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.LocalApi.ScopeName,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "roles"},
                    AccessTokenLifetime=1*60*60,//1 saat
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,// 60 gün
                    RefreshTokenUsage=TokenUsage.ReUse
                },
                new Client
                {
                    ClientId = "Token Exchange Client",
                    ClientName = "TokenExchangeClient",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes =new []{ "urn:ietf:params:outh:grant-type:token-exchange" }, //Client Creadetials(Machine to Machine) kullanılacağı set edilmiş.
                    AllowedScopes = { "discount_full_permission", "fakepayment_full_permission", IdentityServerConstants.StandardScopes.OpenId } //Yetkisi olduğu scope lar tanımlanmış.
                }
            };
    }
}