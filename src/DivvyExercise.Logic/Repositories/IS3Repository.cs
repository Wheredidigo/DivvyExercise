using System.Collections.Generic;

namespace DivvyExercise.Logic.Repositories
{
    public interface IS3Repository
    {
        IEnumerable<string> GetData(string bucket, string key);
    }
}