using System.Linq;

namespace StackExchange.Redis.Extensions.Graph
{
    public sealed class Node
    {
        public int NodeId { get; private set; }

        public string[] Labels { get; private set; }
        public GraphResultNodeProperty[] Properties { get; private set; }

        internal Node(RedisResult[] results)
        {
            NodeId = (int)results[0];

            var labelIds = (RedisResult[])results[1];
            Labels = labelIds.Select(id => GraphCache.Label((int)id)).ToArray();

            var properties = (RedisResult[])results[2];
            Properties = properties.Select(prop => new GraphResultNodeProperty((RedisResult[])prop)).ToArray();
        }
    }
}