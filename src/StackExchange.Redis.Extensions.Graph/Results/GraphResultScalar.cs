using System.Linq;

namespace StackExchange.Redis.Extensions.Graph {
    public class GraphResultScalar {
        public PropertyType Type { get; private set; }
        public RedisResult Value { get; private set; }

        public GraphResultScalar(RedisResult result) {
            var cell = (RedisResult[])result;
            var startIndex = cell.Length > 2 ? 1 : 0;

            Type = (PropertyType)((int)cell[startIndex]);
            Value = cell[startIndex + 1];
        }
    }
}