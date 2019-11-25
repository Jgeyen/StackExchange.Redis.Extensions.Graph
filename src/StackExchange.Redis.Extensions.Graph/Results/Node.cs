using System.Collections.Generic;
using System.Linq;

namespace StackExchange.Redis.Extensions.Graph {
    public sealed class Node {
        public int NodeId { get; private set; }

        public string[] Labels { get; private set; }
        private Dictionary<string, object> _properties;

        public Node(RedisResult result) {
            var cell = (RedisResult[])result;

            NodeId = (int)cell[0];

            var labelIds = (RedisResult[])cell[1];
            Labels = labelIds.Select(id => GraphCache.Label((int)id)).ToArray();

            var properties = (RedisResult[])cell[2];

            _properties = properties.ToDictionary(c => GraphCache.PropertyName((int)((RedisResult[])c)[0]), c => (object)((RedisResult[])c)[2].ToString());

        }

        public T Property<T>(string key) {
            return (T)_properties[key];
        }
    }
}