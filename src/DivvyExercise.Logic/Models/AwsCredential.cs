using System;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

namespace DivvyExercise.Logic.Models
{
    public class AwsCredential
    {
        internal static AWSCredentials GetCredentials(string profileName = "default")
        {
            var chain = new CredentialProfileStoreChain();
            if (chain.TryGetAWSCredentials(profileName, out var awsCredentials))
            {
                return awsCredentials;
            }
            throw new ArgumentOutOfRangeException(nameof(profileName), "The profile provided was not found.");
        }
    }
}