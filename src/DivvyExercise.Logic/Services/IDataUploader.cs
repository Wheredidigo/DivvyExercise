using System.Collections.Generic;
using System.Threading.Tasks;
using DivvyExercise.Logic.Models;

namespace DivvyExercise.Logic.Services
{
    public interface IDataUploader
    {
        Task UploadData(IEnumerable<Node> data);
    }
}