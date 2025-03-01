using Bank.Common.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Bank.Common.Auth
{
    internal class PolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;

        private static readonly object _sync = new();

        public PolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
            _options = options.Value;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);

            if (policy is null)
            {
                lock (_sync)
                {
                    var policyTask = base.GetPolicyAsync(policyName);
                    policyTask.Wait();

                    policy = policyTask.Result;
                    if (policy is null)
                    {
                        AuthorizationPolicyBuilder policyBuilder = new();

                        var model = JsonSerializer.Deserialize<PolicyModel>(policyName);
                        if (model is not null)
                        {
                            policyBuilder = policyBuilder.AddRequirements(model.GetRequirements().ToArray());
                        }

                        policy = policyBuilder.Build();

                        _options.AddPolicy(policyName, policy);
                    }
                }
            }

            return policy;
        }
    }
}
