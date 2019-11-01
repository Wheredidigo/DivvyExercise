using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace DivvyExercise.Logic.Models
{
    public class Node
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "children")]
        public IEnumerable<string> Children { get; set; } = new Collection<string>();
    }
}