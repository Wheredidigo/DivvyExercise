using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DivvyExercise.Logic.Models;
using DivvyExercise.Logic.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DivvyExercise.Logic.Services
{
    public class DynamoDbDataUploader : IDataUploader
    {
        private readonly ILogger<DynamoDbDataUploader> _logger;
        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly AppSettings _appSettings;
        public DynamoDbDataUploader(ILogger<DynamoDbDataUploader> logger, IAmazonDynamoDB dynamoDb, IOptions<AppSettings> appSettings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dynamoDb = dynamoDb ?? throw new ArgumentNullException(nameof(dynamoDb));
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }
        public async Task UploadData(IEnumerable<Node> data)
        {
            try
            {
                var table = Table.LoadTable(_dynamoDb, _appSettings.TargetDynamoDbTable);

                foreach (var node in data)
                {
                    var doc = Document.FromJson(JsonConvert.SerializeObject(node));
                    await table.PutItemAsync(doc);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                throw;
            }
        }
    }
}