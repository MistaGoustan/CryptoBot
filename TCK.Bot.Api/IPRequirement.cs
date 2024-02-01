using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace TCK.Bot.Api
{
    public class IPRequirement : IAuthorizationRequirement
    {
        public String Whitelist { get; }

        public IPRequirement(String whitelist)
        {
            Whitelist = whitelist;
        }
    }

    public class IPAddressHandler : AuthorizationHandler<IPRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IPRequirement requirement)
        {
            var httpContext = (DefaultHttpContext)context.Resource ?? throw new Exception($"Cannot cast to {nameof(DefaultHttpContext)}");
            var remoteIp = httpContext.Connection.RemoteIpAddress ?? throw new Exception($"No ip address found.");

            var bytes = remoteIp.GetAddressBytes();

            foreach (var address in ConvertIpsToRawList(requirement.Whitelist))
            {
                if (address.SequenceEqual(bytes))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }

        private Byte[][] ConvertIpsToRawList(String trustedIps)
        {
            var ips = trustedIps.Split(';');
            var rawTrustedIps = new Byte[ips.Length][];

            for (var i = 0; i < ips.Length; i++)
            {
                rawTrustedIps[i] = IPAddress.Parse(ips[i]).GetAddressBytes();
            }

            return rawTrustedIps;
        }
    }
}
