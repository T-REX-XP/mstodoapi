using System;
using MSTodoApi.Infrastructure;
using MSTodoApi.Infrastructure.Utils;
using Newtonsoft.Json;

namespace MSTodoApi.IntegrationTests
{
    public static class TestDataHelper
    {
        static readonly IDatetimeUtils  DatetimeUtils = new DatetimeUtils();
        public static string TaskSubject = "Test Task";
        public static string EventSubject = "Test Event";
        
        private static readonly JsonSerializerSettings  SerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        
        public static string TaskModelFor(DateTime dueDateTime)
        {
            var task = new 
            {
                Subject = $"{TaskSubject}({dueDateTime})",
                StartDateTime = new {
                    DateTime = DatetimeUtils.FormatLongUtc(dueDateTime),
                    TimeZone = "UTC"
                },
                DueDateTime = new {
                    DateTime = DatetimeUtils.FormatLongUtc(dueDateTime),
                    TimeZone = "UTC"
                }
            };
            
            var data = JsonConvert.SerializeObject(task, SerializerSettings);

            return data;
        }

        public static string EventModelFor(DateTime dueDateTime)
        {
            var evt = new
            {
                Subject = $"{EventSubject}(S: {DatetimeUtils.FormatLongUtc(dueDateTime)} E: {DatetimeUtils.FormatLongUtc(DatetimeUtils.GetEndOfDay(dueDateTime))})",
                Body = new
                {
                    ContentType = "HTML",
                    Content = $"Test Content ({dueDateTime})"
                },
                Start = new
                {
                    DateTime = DatetimeUtils.FormatLongUtc(dueDateTime),
                    TimeZone = "UTC"
                },
                End = new
                {
                    DateTime = DatetimeUtils.FormatLongUtc(DatetimeUtils.GetEndOfDay(dueDateTime)),
                    TimeZone = "UTC"
                },
                Attendees = new[]
                {
                    new
                    {
                        EmailAddress = new
                        {
                            Address = "mscase23072017@outlook.com",
                            Name = "Name Surname"
                        },
                        Type = "Required"
                    }
                }
            };
            
            var data = JsonConvert.SerializeObject(evt, SerializerSettings);

            return data;
        }
    }
}