using System.Collections.Generic;
using System.Linq;

namespace StackExchange.Redis.Extensions.Graph {
    public sealed class Edge {
        public int Id { get; private set; }
        public int RelationshipTypeId { get; private set; }
        public string RelationshipType { get; private set; }
        public int SourceNodeId { get; private set; }
        public int DestinationNodeId { get; private set; }
        private Dictionary<string, object> _properties;


        public Edge(RedisResult results) {
            var cell = (RedisResult[])results;
            Id = (int)cell[0];

            RelationshipTypeId = (int)cell[1];
            RelationshipType = GraphCache.RelationshipType(RelationshipTypeId);

            SourceNodeId = (int)cell[2];
            DestinationNodeId = (int)cell[3];
            var properties = (RedisResult[])cell[4];

            _properties = properties.ToDictionary(c => GraphCache.PropertyName((int)((RedisResult[])c)[0]), c => (object)((RedisResult[])c)[2].ToString());

        }
        public T Property<T>(string key) {
            return (T)_properties[key];
        }
    }
}