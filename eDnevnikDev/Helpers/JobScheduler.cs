using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;

namespace eDnevnikDev.Helpers
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<PromeniStatusJob>().Build();

            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithDailyTimeIntervalSchedule
            //      (s =>
            //         s.WithIntervalInHours(1)
            //        .OnEveryDay()
            //        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(14, 19))
            //      )
            //    .Build();

            ITrigger trigger = TriggerBuilder.Create()
    .WithIdentity("trigger1", "group1")
    .StartNow()
    .WithSimpleSchedule(x => x
        .WithIntervalInSeconds(3600)
        .RepeatForever())
    .Build();

            //scheduler.ScheduleJob(job, trigger);
        }
    }
}