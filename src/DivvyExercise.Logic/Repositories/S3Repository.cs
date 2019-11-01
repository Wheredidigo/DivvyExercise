using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Amazon.S3;

namespace DivvyExercise.Logic.Repositories
{
    public class S3Repository : IS3Repository
    {
        private readonly IAmazonS3 _amazonS3;
        public S3Repository(IAmazonS3 amazonS3)
        {
            _amazonS3 = amazonS3 ?? throw new ArgumentNullException(nameof(amazonS3));
        }

        public IEnumerable<string> GetData(string bucket, string key)
        {
            using(var dataStream = _amazonS3.GetObjectStreamAsync(bucket, key, null).GetAwaiter().GetResult())
            {
                if (key.ToLower().EndsWith(".gz"))
                {
                    using (var unzipStream = new GZipStream(dataStream, CompressionMode.Decompress))
                    {
                        using (var reader = new StreamReader(unzipStream, Encoding.UTF8, false, 4096, true))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                yield return line;
                            }
                        }
                    }
                }
                else
                {
                    using (var reader = new StreamReader(dataStream, Encoding.UTF8, false, 4096, true))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            yield return line;
                        }
                    }
                }
            }
        }
    }
}