using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.S3Events;
using Amazon.Lambda.SQSEvents;
using DivvyExercise.Logic.Repositories;
using DivvyExercise.Logic.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DivvyExercise.Logic
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly IS3Repository _s3Repository;
        private readonly IDataConverter _dataConverterService;
        private readonly IDataUploader _dataUploader;
        public App(ILogger<App> logger, IS3Repository s3Repository, IDataConverter dataConverterService, IDataUploader dataUploader)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _s3Repository = s3Repository ?? throw new ArgumentNullException(nameof(s3Repository));
            _dataConverterService = dataConverterService ?? throw new ArgumentNullException(nameof(dataConverterService));
            _dataUploader = dataUploader ?? throw new ArgumentNullException(nameof(dataUploader));
        }

        public async Task Run(SQSEvent sqsEvent)
        {
            Task taskResult = null;
            var tasks = new List<Task>();
            try
            {
                tasks.AddRange(sqsEvent.Records.Select(ProcessMessageAsync));
                await (taskResult = Task.WhenAll(tasks));
            }
            catch (Exception e)
            {
                if (taskResult == null) throw;
                foreach (var innerException in taskResult.Exception.InnerExceptions)
                {
                    _logger.LogError($"Inner Exception: {innerException.Message}");
                }
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                throw;
            }
            _logger.LogInformation("Hello World!!");
            await Task.CompletedTask;
        }

        private async Task ProcessMessageAsync(SQSEvent.SQSMessage message)
        {
            var (bucket, key) = GetS3DetailsFromMessage(message);
            await _dataUploader.UploadData(_dataConverterService.ConvertData(_s3Repository.GetData(bucket, key)));
        }

        private (string bucket, string key) GetS3DetailsFromMessage(SQSEvent.SQSMessage message)
        {
            var s3Event = JsonConvert.DeserializeObject<S3Event>(message.Body);
            var s3Notification = s3Event.Records?[0].S3;
            if (s3Notification == null)
            {
                throw new ArgumentNullException(nameof(s3Notification), "Missing S3 Notification");
            }
            return (s3Notification.Bucket.Name, s3Notification.Object.Key);
        }
    }
}