using System.Linq;

namespace StackExchange.Redis.Extensions.Graph
{
    public sealed class Node
    {
        public int NodeId { get; private set; }

        public string[] Labels { get; private set; }
        public GraphResultNodeProperty[] Properties { get; private set; }

        public Node(RedisResult result)
        {
            var cell = (RedisResult[])result;

            NodeId = (int)cell[0];

            var labelIds = (RedisResult[])cell[1];
            Labels = labelIds.Select(id => GraphCache.Label((int)id)).ToArray();

            var properties = (RedisResult[])cell[2];
            Properties = properties.Select(prop => new GraphResultNodeProperty(prop)).ToArray();
        }
    }
}