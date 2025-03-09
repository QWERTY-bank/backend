using Bank.Credits.Application.Jobs.Configurations;
using Bank.Credits.Application.Jobs.IssuingCredits;
using Bank.Credits.Application.Jobs.IssuingCredits.Configurations;
using Bank.Credits.Application.Jobs.Payments;
using Bank.Credits.Application.Jobs.Payments.Configurations;
using Bank.Credits.Application.Jobs.Repayments;
using Bank.Credits.Application.Jobs.Repayments.Configurations;
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

            services.ConfigureOptions<RepaymentsPlannerOptionsConfigure>();
            services.ConfigureOptions<RepaymentsHandlerOptionsConfigure>();

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
            services.AddQuartz(q =>
            {
                int startDelaySeconds = 10;

                q.AddJob<PaymentsPlanner>(j => j
                    .StoreDurably()
                    .WithIdentity(nameof(PaymentsPlanner)));
                q.AddTrigger(t => t
                    .ForJob(nameof(PaymentsPlanner))
                    .WithIdentity($"{nameof(PaymentsPlanner)}Trigger", $"{nameof(PaymentsPlanner)}Group")
                    .StartAt(DateTime.UtcNow.AddSeconds(startDelaySeconds))
                    .WithSimpleSchedule(x => x
                            .WithInterval(jobsOptions.Payments.Planner.Interval)
                            .RepeatForever()));

                startDelaySeconds += 10;

                q.AddJob<PaymentsHandler>(j => j
                    .StoreDurably()
                    .WithIdentity(nameof(PaymentsHandler)));
                q.AddTrigger(t => t
                    .ForJob(nameof(PaymentsHandler))
                    .WithIdentity($"{nameof(PaymentsHandler)}Trigger", $"{nameof(PaymentsHandler)}Group")
                    .StartAt(DateTime.UtcNow.AddSeconds(startDelaySeconds))
                    .WithSimpleSchedule(x => x
                            .WithInterval(jobsOptions.Payments.Handler.Interval)
                            .RepeatForever()));

            });
            services.AddQuartz(q =>
            {
                int startDelaySeconds = 10;

                q.AddJob<RepaymentsPlanner>(j => j
                    .StoreDurably()
                    .WithIdentity(nameof(RepaymentsPlanner)));
                q.AddTrigger(t => t
                    .ForJob(nameof(RepaymentsPlanner))
                    .WithIdentity($"{nameof(RepaymentsPlanner)}Trigger", $"{nameof(RepaymentsPlanner)}Group")
                    .StartAt(DateTime.UtcNow.AddSeconds(startDelaySeconds))
                    .WithSimpleSchedule(x => x
                            .WithInterval(jobsOptions.Repayments.Planner.Interval)
                            .RepeatForever()));

                startDelaySeconds += 10;

                q.AddJob<RepaymentsHandler>(j => j
                    .StoreDurably()
                    .WithIdentity(nameof(RepaymentsHandler)));
                q.AddTrigger(t => t
                    .ForJob(nameof(RepaymentsHandler))
                    .WithIdentity($"{nameof(RepaymentsHandler)}Trigger", $"{nameof(RepaymentsHandler)}Group")
                    .StartAt(DateTime.UtcNow.AddSeconds(startDelaySeconds))
                    .WithSimpleSchedule(x => x
                            .WithInterval(jobsOptions.Repayments.Handler.Interval)
                            .RepeatForever()));

            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }
    }
}
