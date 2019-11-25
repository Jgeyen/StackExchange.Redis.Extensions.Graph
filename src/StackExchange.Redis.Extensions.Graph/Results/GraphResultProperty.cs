namespace StackExchange.Redis.Extensions.Graph {
    public enum PropertyType {
        PROPERTY_UNKNOWN = 0,
        PROPERTY_NULL = 1,
        PROPERTY_STRING = 2,
        PROPERTY_INTEGER = 3,
        PROPERTY_BOOLEAN = 4,
        PROPERTY_DOUBLE = 5,
    }

    public sealed class GraphResultNodeProperty : GraphResultScalar {
        public int KeyId { get; private set; }

        internal GraphResultNodeProperty(RedisResult result) : base(result) {
            KeyId = (int)((RedisResult[])result)[0];
        }
    }
}