namespace Ofl.Twitch
{
    public interface IClientIdProvider
    {
        string ClientId { get; }

        string ClientSecret { get; }
    }
}