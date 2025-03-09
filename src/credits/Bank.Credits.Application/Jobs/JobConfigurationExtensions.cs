using Bank.Credits.Application.Jobs.Configurations;
using Bank.Credits.Application.Jobs.IssuingCredits;
using Bank.Credits.Application.Jobs.IssuingCredits.Configurations;
using Bank.Credits.Application.Jobs.Payments.Configurations;
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
            services.ConfigureOptions<IssuingCreditsHandlerOptionsConfigure>();

            services.ConfigureOptions<PaymentsPlannerOptionsConfigure>();
            services.ConfigureOptions<PaymentsHandlerOptionsConfigure>();

            var jobsOptions = configuration.GetRequiredSection("Jobs").Get<JobsOptions>();
            if (jobsOptions == null)
            {
                throw new NullReferenceException("Section JobsConfiguration was not found in application settings");
            }

            services.AddQuartz(q =>
            {
                int startDelaySeconds = 10;

                q.AddJob<IssuingCreditsPlanner>(j => j
                    .StoreDurably()
                    .WithIdentity(nameof(IssuingCreditsPlanner)));
                q.AddTrigger(t => t
                    .ForJob(nameof(IssuingCreditsPlanner))
                    .WithIdentity($"{nameof(IssuingCreditsPlanner)}Trigger", $"{nameof(IssuingCreditsPlanner)}Group")
                    .StartAt(DateTime.UtcNow.AddSeconds(startDelaySeconds))
                    .WithSimpleSchedule(x => x
                            .WithInterval(jobsOptions.IssuingCredits.Planner.Interval)
                            .RepeatForever()));

                startDelaySeconds += 10;

                q.AddJob<IssuingCreditsHandler>(j => j
                    .StoreDurably()
                    .WithIdentity(nameof(IssuingCreditsHandler)));
                q.AddTrigger(t => t
                    .ForJob(nameof(IssuingCreditsHandler))
                    .WithIdentity($"{nameof(IssuingCreditsHandler)}Trigger", $"{nameof(IssuingCreditsHandler)}Group")
                    .StartAt(DateTime.UtcNow.AddSeconds(startDelaySeconds))
                    .WithSimpleSchedule(x => x
                            .WithInterval(jobsOptions.IssuingCredits.Handler.Interval)
                            .RepeatForever()));

            });
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }
    }
}
