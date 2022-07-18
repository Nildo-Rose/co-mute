using co_mute_master.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace co_mute_master.Security
{
    public class SecurityManager
    {
        public async void SignIn(HttpContext httpContext, Register user, string schema)
        {
            ClaimsIdentity claimIdentity = new ClaimsIdentity(GetUserClaims(user),
                CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimIdentity);
            await httpContext.SignInAsync(schema, claimsPrincipal);
        }

        public async void SignOut(HttpContext httpContext, string schema)
        {
            await httpContext.SignOutAsync(schema);
        }

        private static IEnumerable<Claim> GetUserClaims(Register user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email, user.Id.ToString())
            };

            foreach (var userRole in user.UserRoles)
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
            return claims;
        }

    }
}
