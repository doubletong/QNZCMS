using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace QNZCMS
{
    public class Program
    {
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()  
            .SetBasePath(Directory.GetCurrentDirectory())  
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)  
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange:true)  
            .Build();  
        
        public static void Main(string[] args)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");            
            var columnOptions = new ColumnOptions  
            {  
                AdditionalColumns = new Collection<SqlColumn>  
                {  
                    new SqlColumn("UserName", SqlDbType.NVarChar)  
                }  
            };  
            Log.Logger = new LoggerConfiguration()  
                .Enrich.FromLogContext()  
                .WriteTo.MSSqlServer(connectionString, sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs" }  
                    , null, null, LogEventLevel.Information, null, columnOptions: columnOptions, null, null)  
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)//To capture Information and error only
                .CreateLogger();  
            
            CreateHostBuilder(args).Build().Run();
            
           
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseUrls("http://localhost:5003/");
                }).UseSerilog();//serilog;  

       
    }
}
