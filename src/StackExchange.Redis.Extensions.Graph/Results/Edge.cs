using System.Linq;

namespace StackExchange.Redis.Extensions.Graph
{
    public sealed class Edge
    {
        public int Id { get; private set; }

        public int RelationshipTypeId { get; private set; }
        public string RelationshipType {get; private set;}
        public int SourceNodeId { get; private set; }
        public int DestinationNodeId { get; private set; }
        public GraphResultNodeProperty[] Properties { get; private set; }


        public Edge(RedisResult results)
        {
            var cell = (RedisResult[])results;
            Id = (int)cell[0];

            RelationshipTypeId = (int)cell[1];
            RelationshipType = GraphCache.RelationshipType(RelationshipTypeId);

            SourceNodeId = (int)cell[2];
            DestinationNodeId = (int)cell[3];
            var properties = (RedisResult[])cell[4];

            //TODO: Convert to key value pair
            Properties = properties.Select(prop => new GraphResultNodeProperty(prop)).ToArray();
        }
    }
}