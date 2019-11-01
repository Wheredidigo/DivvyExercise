using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using DivvyExercise.Logic;
using Microsoft.Extensions.DependencyInjection;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace DivvyExercise.Lambda
{
    public class Function
    {
        private static ServiceProvider ServiceProvider { get; }
        static Function()
        {
            ServiceProvider = Startup.GetServiceProvider();
        }
        
        public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            await ServiceProvider.GetService<App>().Run(evnt);
        }

        ~Function()
        {
            ServiceProvider.Dispose();
        }
    }
}