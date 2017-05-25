using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Ofl.Twitch.V5
{
    public class ConfigurationClientIdProvider : IClientIdProvider
    {
        #region Constructor

        public ConfigurationClientIdProvider(IOptions<ClientIdConfiguration> clientIdConfiguration)
        {
            // Validate parameters.
            if (string.IsNullOrWhiteSpace(clientIdConfiguration?.Value?.ClientId)) throw new ArgumentNullException(nameof(clientIdConfiguration));

            // Assign values.
            _clientIdConfiguration = clientIdConfiguration.Value;
        }

        #endregion

        #region Instance, read-only state.

        private readonly ClientIdConfiguration _clientIdConfiguration;

        #endregion

        #region Implementation of IClientIdProvider

        public Task<string> GetClientIdAsync(CancellationToken cancellationToken)
        {
            // Return the configuration.
            return Task.FromResult(_clientIdConfiguration.ClientId);
        }

        #endregion
    }
}
