using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Ofl.Twitch.V5
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

        public Task<string> GetClientIdAsync(CancellationToken cancellationToken)
        {
            // Return the configuration.
            return Task.FromResult(_clientIdConfigurationOptions.Value.ClientId);
        }

        #endregion
    }
}
