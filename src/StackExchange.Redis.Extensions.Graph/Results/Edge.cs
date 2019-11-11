using System.Linq;

namespace StackExchange.Redis.Extensions.Graph
{
    public sealed class Edge
    {
        public int Id { get; private set; }

        public int RelationshipTypeId { get; private set; }
        public int SourceNodeId { get; private set; }
        public int DestinationNodeId { get; private set; }
        public GraphResultNodeProperty[] Properties { get; private set; }


        internal Edge(RedisResult[] results)
        {
            Id = (int)results[0];
            RelationshipTypeId = (int)results[1];
            SourceNodeId = (int)results[2];
            DestinationNodeId = (int)results[3];
            var properties = (RedisResult[])results[4];

            Properties = properties.Select(prop => new GraphResultNodeProperty((RedisResult[])prop)).ToArray();
        }
    }
}