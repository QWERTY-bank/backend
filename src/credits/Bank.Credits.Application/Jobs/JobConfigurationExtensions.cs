using Bank.Common.Api.Configurations.Swagger;
using Bank.Credits.Application.Jobs.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Bank.Credits.Application.Jobs
{
    public static class JobConfigurationExtensions
    {
        public static void AddJobs(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<IssuingCreditsPlannerOptionsConfigure>();

            var jobsSection = configuration.GetRequiredSection("Jobs").Get<JobsOptions>();

            services.AddQuartz(q =>
            {
                //q.AddJob<CreditHandler>(j => j
                //    .StoreDurably()
                //    .WithIdentity("CreditHandler"));
                //q.AddTrigger(t => t
                //    .ForJob("CreditHandler")
                //    .WithIdentity("CreditHandler-trigger")
                //    .WithCronSchedule("0 0 0/1 1/1 * ? *"));
            });
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }
    }
}
