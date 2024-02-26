namespace TCK.Bot.Options
{
    public class BearerAuthenticationOptions
    {
        public string Audience { get; set; } = default!;
        public Double ExpireTimeInMinutes { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Key { get; set; } = default!;
    }
}
