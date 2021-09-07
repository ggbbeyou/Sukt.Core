using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sukt.AuthServerHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseSerilog((webHost, configuration) =>
                    {
                        //�õ������ļ�
                        var serilog = webHost.Configuration.GetSection("Serilog");
                        //��С����
                        var minimumLevel = serilog["MinimumLevel:Default"];
                        //��־�¼�����
                        var logEventLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), minimumLevel);
                        configuration.ReadFrom.
                        Configuration(webHost.Configuration.GetSection("Serilog")).Enrich.FromLogContext().WriteTo.Console(logEventLevel);
                        configuration.WriteTo.Map(le => MapData(le),
                (key, log) => log.Async(o => o.File(Path.Combine("logs", @$"{key.time:yyyy-MM-dd}\{key.level.ToString().ToLower()}.txt"), logEventLevel)));

                        (DateTime time, LogEventLevel level) MapData(LogEvent logEvent)
                        {
                            return (new DateTime(logEvent.Timestamp.Year, logEvent.Timestamp.Month, logEvent.Timestamp.Day, logEvent.Timestamp.Hour, logEvent.Timestamp.Minute, logEvent.Timestamp.Second), logEvent.Level);
                        }
                    })//ע��Serilog��־�м��//����������log��
                    ;
                });
    }
}
