namespace TCK.Bot.Options
{
    public class BearerAuthenticationOptions
    {
        public String Audience { get; set; } = default!;
        public Double ExpireTimeInMinutes { get; set; } = default!;
        public String Issuer { get; set; } = default!;
        public String Key { get; set; } = default!;
    }
}
