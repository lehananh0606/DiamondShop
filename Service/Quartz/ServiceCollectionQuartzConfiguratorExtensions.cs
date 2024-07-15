using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Quartz
{
    public static class ServiceCollectionQuartzConfiguratorExtensions
    {
        public static void AddJobAndTrigger<T>(
        this IServiceCollectionQuartzConfigurator quartz, IConfiguration config
        ) where T : IJob
        {
            string jobName = typeof(T).Name;
            var configKey = $"Quartz:{jobName}";
            var cronSchedule = config[configKey];
            if (string.IsNullOrEmpty(cronSchedule))
            {
                throw new Exception($"No Quartz.Net Cron schedule found for job in configuration at {configKey}");
            }
            var jobKey = new JobKey(jobName);
            quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));
            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "--trigger")
                .WithCronSchedule(cronSchedule)
            );
        }
    }


}
