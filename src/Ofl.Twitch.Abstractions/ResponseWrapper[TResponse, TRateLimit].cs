using System;

namespace Ofl.Twitch
{
    public class ResponseWrapper<TResponse, TRateLimit>
        where TRateLimit : RateLimit
    {
        #region Constructor

        public ResponseWrapper(
            TRateLimit rateLimit,
            TResponse response
        )
        {
            // Validate parameters.
            RateLimit = rateLimit ?? throw new ArgumentNullException(nameof(rateLimit));
            Response = response ?? throw new ArgumentNullException(nameof(response));
        }

        #endregion

        #region Instance, read-only state

        public TRateLimit RateLimit { get; }

        public TResponse Response { get; }

        #endregion
    }
}
