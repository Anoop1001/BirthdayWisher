using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NCrontab;
using Siemens.Audiology.BirthdayWisher.Models;
using Siemens.Audiology.Notification;
using Siemens.Audiology.Notification.Contract;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Siemens.Audiology.BirthdayWisher.HostedServices
{
    public class BirthdayHostedService : BackgroundService
    {
        private readonly CrontabSchedule _schedule;
        private readonly string Schedule = "0 0 0 * * *";
        private readonly IMailer _mailer;
        private DateTime _nextRun;
        public BirthdayHostedService(IOptions<BirthdaySchedulerOptions> options, IMailer mailer)
        {
            Schedule = options.Value.CronExpression ?? Schedule;
            _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
            _mailer = mailer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.Now;
                var nextrun = _schedule.GetNextOccurrence(now);
                if (now >= _nextRun)
                {
                    await BirthdayProcessor();
                    _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                }
                await Task.Delay(5000, stoppingToken);
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        private async Task BirthdayProcessor()
        {
            try
            {
                await _mailer.SendEmailAsync(new EmailData
                {
                    To = new List<string> { "anoophn10@gmail.com" },
                    Body = "Hi Anoop, This is a test mail",
                    Subject = "Happy Birthday"
                });
            }
            catch (Exception ex)
            {

            }

        }
    }

    public enum Platform
    {
        iOS = 1,
        Android = 2
    }
}
