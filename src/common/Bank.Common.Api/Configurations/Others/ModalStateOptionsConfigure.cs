﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bank.Common.Api.Configurations.Others
{
    public class ModalStateOptionsConfigure : IConfigureOptions<ApiBehaviorOptions>
    {
        public void Configure(ApiBehaviorOptions options)
        {
            // Отключение автоматической проверки ModelState.IsValid
            options.SuppressModelStateInvalidFilter = true;
        }
    }
}
