﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Intellias.CQRS.Core.Config
{
    /// <summary>
    /// Extensions methods for CQRS config.
    /// </summary>
    public static class CqrsSettings
    {
        /// <summary>
        /// Initializes static members of the <see cref="CqrsSettings"/> class.
        /// </summary>
        static CqrsSettings()
        {
            JsonConvert.DefaultSettings = JsonConfig;
        }

        /// <summary>
        /// Configures JSON serializer globally
        /// Settings will automatically be used by JsonConvert.SerializeObject/DeserializeObject.
        /// </summary>
        public static Func<JsonSerializerSettings> JsonConfig =>
            () => new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DateParseHandling = DateParseHandling.DateTimeOffset,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
    }
}
