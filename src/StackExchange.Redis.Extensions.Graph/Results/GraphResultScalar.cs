using System.Linq;

namespace StackExchange.Redis.Extensions.Graph
{
    public class GraphResultScalar
    {
        public PropertyType Type { get; private set; }
        public RedisResult Value { get; private set; }

        internal GraphResultScalar(RedisResult[] result)
        {
            var startIndex = result.Length > 2 ? 1:0;

            Type = (PropertyType)((int)result[startIndex]);
            Value = result[startIndex+1];
        }
    }
}