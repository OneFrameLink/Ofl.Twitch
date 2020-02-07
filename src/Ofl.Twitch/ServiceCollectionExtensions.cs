using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ofl.Twitch.V5;

namespace Ofl.Twitch
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTwitchClient(this IServiceCollection serviceCollection,
            IConfiguration clientIdConfiguration)
        {
            // Validate parameters.
            var sc = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            if (clientIdConfiguration == null) throw new ArgumentNullException(nameof(clientIdConfiguration));

            // Bind to client ID configuration and the provider.
            sc = sc.Configure<ClientIdConfiguration>(clientIdConfiguration.Bind);
            sc = sc.AddTransient<IClientIdProvider, ConfigurationClientIdProvider>();

			// Add the client handler.
			sc = sc.AddTransient<TwitchHttpClientHandler>();
			
            // Add the twitch client.
            sc.AddHttpClient<ITwitchClient, TwitchClient>()
                .ConfigurePrimaryHttpMessageHandler<TwitchHttpClientHandler>();

            // Return the service collection.
            return sc;
        }
    }
}
