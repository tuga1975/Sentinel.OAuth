﻿namespace Sentinel.Sample.Managers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Sentinel.OAuth.Core.Constants.Identity;
    using Sentinel.OAuth.Core.Interfaces.Identity;
    using Sentinel.OAuth.Core.Interfaces.Managers;
    using Sentinel.OAuth.Models.Identity;

    public class SimpleUserManager : IUserManager
    {
        public async Task<ISentinelPrincipal> AuthenticateUserWithPasswordAsync(string username, string password)
        {
            // Just return an authenticated principal with the username as name if the username matches the password
            if (username == password)
            {
                return new SentinelPrincipal(new SentinelIdentity(AuthenticationType.OAuth, new SentinelClaim(ClaimTypes.Name, username)));
            }

            return SentinelPrincipal.Anonymous;
        }
    }
}