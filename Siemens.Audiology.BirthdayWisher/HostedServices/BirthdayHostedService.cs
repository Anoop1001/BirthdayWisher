using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NCrontab;
using Siemens.Audiology.BirthdayWisher.Business.Contract;
using Siemens.Audiology.BirthdayWisher.Models;
using Siemens.Audiology.BirthdayWisher.Utilities;
using Siemens.Audiology.Notification;
using Siemens.Audiology.Notification.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IBirthdayCalendarProcessor _birthdayCalendarProcessor;
        private readonly IEmailDataGenerator _emailDataGenerator;
        private readonly string _ccEmailAddress;
        public BirthdayHostedService(IOptions<BirthdaySchedulerOptions> options, IMailer mailer, IBirthdayCalendarProcessor birthdayCalendarProcessor, IEmailDataGenerator emailDataGenerator)
        {
            Schedule = options.Value.CronExpression ?? Schedule;
            _ccEmailAddress = options.Value.CcEmail;
            _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
            _mailer = mailer;
            _birthdayCalendarProcessor = birthdayCalendarProcessor;
            _emailDataGenerator = emailDataGenerator;
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
                var listOfBirthDays = await _birthdayCalendarProcessor.GetBirthDayDetailsForToday();
                var taskListToSendEmail = new List<Task>();
                var processedEmailData = new List<EmailData>();
                var emailDataList = _emailDataGenerator.GetEmailDataList(listOfBirthDays).ToList();
                emailDataList.ForEach(x =>
                {
                    x.Cc = _ccEmailAddress.Split(",");
                    processedEmailData.Add(x);
                });
                processedEmailData.ForEach(x =>
                {
                    var task = _mailer.SendEmailAsync(x);
                    taskListToSendEmail.Add(task);
                });
                await Task.WhenAll(taskListToSendEmail);
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
