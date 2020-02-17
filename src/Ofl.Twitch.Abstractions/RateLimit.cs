using System;

namespace Ofl.Twitch
{
    public class RateLimit
    {
        #region Constructor

        public RateLimit(
            int limit,
            int remaining,
            DateTimeOffset reset
        )
        {
            // Validate parameters.
            Limit = limit < 0
                ? throw new ArgumentOutOfRangeException(nameof(limit), limit,
                    $"The {nameof(limit)} parameter must be a non-negative value.")
                : limit;
            Remaining = remaining < 0
                ? throw new ArgumentOutOfRangeException(nameof(remaining), remaining,
                    $"The {nameof(remaining)} parameter must be a non-negative value.")
                : remaining;
            Reset = reset;
        }

        #endregion

        #region Instance, read-only state

        public int Limit { get; }

        public int Remaining { get; }

        public DateTimeOffset Reset { get; }

        #endregion
    }
}
