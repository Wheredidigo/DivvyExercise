using System.Collections.Generic;
using DivvyExercise.Logic.Models;

namespace DivvyExercise.Logic.Services
{
    public interface IDataConverter
    {
        IEnumerable<Node> ConvertData(IEnumerable<string> data);
    }
}