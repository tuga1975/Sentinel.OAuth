﻿namespace Sentinel.OAuth.TokenManagers.RedisTokenRepository.Models
{
    using Newtonsoft.Json;
    using Sentinel.OAuth.Core.Interfaces.Models;
    using Sentinel.OAuth.Core.Models.OAuth;
    using StackExchange.Redis;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RedisAccessToken : AccessToken
    {

        /// <summary>
        /// Initializes a new instance of the
        /// Sentinel.OAuth.TokenManagers.RedisTokenRepository.Models.RedisAccessToken class.
        /// </summary>
        public RedisAccessToken()
        {
        }

        /// <summary>Initializes a new instance of the Sentinel.OAuth.TokenManagers.RedisTokenRepository.Models.RedisAccessToken class.</summary>
        /// <param name="accessToken">The access token.</param>
        public RedisAccessToken(IAccessToken accessToken)
            : base(accessToken)
        {
            this.Created = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// Sentinel.OAuth.TokenManagers.RedisTokenRepository.Models.RedisAccessToken class.
        /// </summary>
        /// <param name="hashEntries">The hash entries.</param>
        public RedisAccessToken(HashEntry[] hashEntries)
        {
            var clientId = hashEntries.FirstOrDefault(x => x.Name == "ClientId");
            var redirectUri = hashEntries.FirstOrDefault(x => x.Name == "RedirectUri");
            var subject = hashEntries.FirstOrDefault(x => x.Name == "Subject");
            var token = hashEntries.FirstOrDefault(x => x.Name == "Token");
            var ticket = hashEntries.FirstOrDefault(x => x.Name == "Ticket");
            var validTo = hashEntries.FirstOrDefault(x => x.Name == "ValidTo");
            var created = hashEntries.FirstOrDefault(x => x.Name == "Created");

            this.ClientId = clientId.Value.HasValue ? clientId.Value.ToString() : string.Empty;
            this.RedirectUri = redirectUri.Value.HasValue ? redirectUri.Value.ToString() : string.Empty;
            this.Subject = subject.Value.HasValue ? subject.Value.ToString() : string.Empty;
            this.Token = token.Value.HasValue ? token.Value.ToString() : string.Empty;
            this.Ticket = ticket.Value.HasValue ? ticket.Value.ToString() : string.Empty;
            this.ValidTo = validTo.Value.HasValue ? JsonConvert.DeserializeObject<DateTimeOffset>(validTo.Value.ToString()) : DateTimeOffset.MinValue;
            this.Created = created.Value.HasValue ? JsonConvert.DeserializeObject<DateTimeOffset>(created.Value.ToString()) : DateTimeOffset.MinValue;
        }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTimeOffset Created { get; set; }

        /// <summary>Check if this object is valid.</summary>
        /// <returns><c>true</c> if valid, <c>false</c> if not.</returns>
        public override bool IsValid()
        {
            return base.IsValid() && this.Created != DateTimeOffset.MinValue;
        }

        /// <summary>Converts this object to a list of hash entries.</summary>
        /// <returns>This object as a Redis hash.</returns>
        public HashEntry[] ToHashEntries()
        {
            var entries = new List<HashEntry>();

            entries.Add(new HashEntry("ClientId", this.ClientId));
            entries.Add(new HashEntry("RedirectUri", this.RedirectUri ?? string.Empty));
            entries.Add(new HashEntry("Subject", this.Subject));
            entries.Add(new HashEntry("Scope", JsonConvert.SerializeObject(this.Scope ?? new string[0])));
            entries.Add(new HashEntry("Token", this.Token));
            entries.Add(new HashEntry("Ticket", this.Ticket));
            entries.Add(new HashEntry("ValidTo", JsonConvert.SerializeObject(this.ValidTo)));
            entries.Add(new HashEntry("Created", JsonConvert.SerializeObject(this.Created)));

            return entries.ToArray();
        }
    }
}