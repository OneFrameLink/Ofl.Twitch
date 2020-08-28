﻿using System;
using Microsoft.Extensions.Options;

namespace Ofl.Twitch
{
    public class ConfigurationClientIdProvider : IClientIdProvider
    {
        #region Constructor

        public ConfigurationClientIdProvider(IOptions<ClientIdConfiguration> clientIdConfigurationOptionsOptions)
        {
            // Validate parameters.
            _clientIdConfigurationOptions = clientIdConfigurationOptionsOptions ??
                throw new ArgumentNullException(nameof(clientIdConfigurationOptionsOptions));

            // Assign values.
            _clientIdConfigurationOptions = clientIdConfigurationOptionsOptions;
        }

        #endregion

        #region Instance, read-only state.

        private readonly IOptions<ClientIdConfiguration> _clientIdConfigurationOptions;

        #endregion

        #region Implementation of IClientIdProvider

        public string ClientId
        {
            get
            {
                // If the client is null, throw.
                string? clientId = _clientIdConfigurationOptions.Value.ClientId;

                // Validate.
                if (string.IsNullOrWhiteSpace(clientId))
                    throw new InvalidOperationException($"The {nameof(_clientIdConfigurationOptions.Value.ClientId)} configuration value was empty.");

                // Return the client ID.
                return clientId;
            }
        }

        public string ClientSecret
        {
            get
            {
                // If the client is null, throw.
                string? clientSecret = _clientIdConfigurationOptions.Value.ClientSecret;

                // Validate.
                if (string.IsNullOrWhiteSpace(clientSecret))
                    throw new InvalidOperationException($"The {nameof(_clientIdConfigurationOptions.Value.ClientSecret)} configuration value was empty.");

                // Return the client ID.
                return clientSecret;
            }
        }

        #endregion
    }
}
