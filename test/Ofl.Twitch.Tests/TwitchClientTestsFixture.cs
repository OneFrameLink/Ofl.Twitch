using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ofl.Twitch.Tests
{
    public class TwitchClientTestsFixture : IDisposable
    {
        #region Constructor

        public TwitchClientTestsFixture()
        {
            // Assign values.
            _serviceProvider = CreateServiceProvider();
        }

        #endregion

        #region Read-only state.

        private static readonly IConfigurationRoot ConfigurationRoot = new ConfigurationBuilder()
            // For local debugging.
            .AddJsonFile("appsettings.local.json", true)
            // For Appveyor.
            .AddEnvironmentVariables()
            .Build();

        private readonly ServiceProvider _serviceProvider;

        #endregion

        #region Helpers

        private static ServiceProvider CreateServiceProvider()
        {
            // Create a collection.
            var sc = new ServiceCollection();

            // Add the google apis.
            sc.AddTwitchClient(ConfigurationRoot.GetSection("Twitch"));

            // Build and return.
            return sc.BuildServiceProvider();
        }

        public ITwitchClient CreateTwitchClient() => _serviceProvider.GetRequiredService<ITwitchClient>();

        #endregion

        #region IDisposable implementation.

        public void Dispose()
        {
            // Call the overload and
            // suppress finalization.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            // Dispose of unmanaged resources.

            // If not disposing, get out.
            if (!disposing) return;

            // Dispose of IDisposable implementations.
            using var _ = _serviceProvider;
        }

        ~TwitchClientTestsFixture() => Dispose(false);

        #endregion
    }
}
