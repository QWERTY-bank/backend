﻿using Bank.Common.Resilience;

namespace Bank.Credits.Application.Requests.Configurations
{
    public class TokenServiceOptions
    {
        public string Secret { get; set; } = null!;
        public string BaseUrl { get; set; } = null!;

        public string LoginPath { get; set; } = null!;

        public ResilienceConfiguration Resilience { get; set; } = ResilienceConfiguration.Null;
    }
}
