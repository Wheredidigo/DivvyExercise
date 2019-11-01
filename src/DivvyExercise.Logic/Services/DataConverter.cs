using System.Collections.Generic;
using System.Linq;
using DivvyExercise.Logic.Models;

namespace DivvyExercise.Logic.Services
{
    public class DataConverter : IDataConverter
    {
        public IEnumerable<Node> ConvertData(IEnumerable<string> data)
        {
            var dictionary = data.Select(x => x.Split(" ")).ToDictionary(y => y[0], y => y[y.Length - 1]);
            var childNodeLookup = dictionary.ToLookup(x => x.Value);
            var nodeList = dictionary.Keys.Union(dictionary.Values).Select(key => new Node { Name = key, Children = childNodeLookup[key].Select(x => x.Key).OrderBy(x => x) }).ToList();

            foreach (var rootNode in nodeList.Where(x => !nodeList.SelectMany(z => z.Children).Contains(x.Name)))
            {
                yield return rootNode;

                foreach (var child in GetTreeNodes(rootNode))
                {
                    yield return child;
                }
            }
            
            IEnumerable<Node> GetTreeNodes(Node root)
            {
                foreach (var item in root.Children)
                {
                    var node = nodeList.FirstOrDefault(x => x.Name.Equals(item));
                    yield return node;

                    if (node == null || !node.Children.Any()) continue;

                    foreach (var childNode in GetTreeNodes(node))
                    {
                        yield return childNode;
                    }
                }
            }
        }
    }
}