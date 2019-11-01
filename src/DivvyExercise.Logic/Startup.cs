using System.IO;
using Amazon.DynamoDBv2;
using Amazon.S3;
using DivvyExercise.Logic.Models;
using DivvyExercise.Logic.Repositories;
using DivvyExercise.Logic.Services;
using DivvyExercise.Logic.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace DivvyExercise.Logic
{
    public class Startup
    {
        public static ServiceProvider GetServiceProvider(bool isLocal = false)
        {
            var services = new ServiceCollection();
            ConfigureCommonServices(services);
            if (isLocal)
            {
                ConfigureLocalServices(services);
            }
            else
            {
                ConfigureServices(services);
            }
            return services.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAmazonS3>(provider => new AmazonS3Client());
            services.AddSingleton<IAmazonDynamoDB>(provider => new AmazonDynamoDBClient());
        }

        private static void ConfigureLocalServices(IServiceCollection services)
        {
            
            services.AddSingleton<IAmazonS3>(provider => new AmazonS3Client(AwsCredential.GetCredentials()));
            services.AddSingleton<IAmazonDynamoDB>(provider => new AmazonDynamoDBClient(AwsCredential.GetCredentials()));
        }

        private static void ConfigureCommonServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();
            services.AddOptions();
            services.Configure<AppSettings>(config.GetSection("AppSettings"));
            services.AddLogging(x => x.AddNLog());

            services.AddSingleton<IS3Repository, S3Repository>();
            services.AddSingleton<IDataConverter, DataConverter>();
            services.AddSingleton<IDataUploader, DynamoDbDataUploader>();

            services.AddTransient<App>();
        }
    }
}
