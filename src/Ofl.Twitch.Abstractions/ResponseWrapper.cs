namespace Ofl.Twitch
{
    public class ResponseWrapper<TResponse> : ResponseWrapper<TResponse, RateLimit>
    {
        #region Constructor

        public ResponseWrapper(
            RateLimit rateLimit,
            TResponse response
        ) : base(rateLimit, response)
        { }

        #endregion
    }
}
