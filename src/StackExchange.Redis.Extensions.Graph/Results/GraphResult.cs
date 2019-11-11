using System.Collections.Generic;

namespace StackExchange.Redis.Extensions.Graph
{
    public sealed class GraphResult
    {
        private readonly Dictionary<string, RedisValue>[] _results;

        public GraphResultStats Stats { get; private set; }

        public GraphResultHeader Header { get; private set; }

        internal GraphResult(RedisResult result, GraphCache cache)
        {
            var _results = (RedisResult[])result;
            Stats = new GraphResultStats(_results);

            if (_results.Length > 1)
            {
                Header = new GraphResultHeader(_results);

                var rows = (RedisResult[])_results[1];
                var firstRow = (RedisResult[])rows[2];
                var rowNode = new Node((RedisResult[])firstRow[0], cache);
                var rowRel = new Edge((RedisResult[])firstRow[1]);
                var rowScalar = new GraphResultScalar((RedisResult[])firstRow[2]);
            }
        }

        public IReadOnlyList<Dictionary<string, RedisValue>> GetResults() => _results;

        public Dictionary<string, RedisValue> this[int index]
            => index >= _results.Length ? null : _results[index];

    }

    // _results = new Dictionary<string, RedisValue>[results.Length - 1];
    // for (int i = 1; i < results.Length; i++)
    // {
    //     var raw = (RedisResult[])results[i];
    //     var cur = new Dictionary<string, RedisValue>();
    //     for (int j = 0; j < raw.Length;)
    //     {
    //         var key = (string)raw[j++];
    //         var val = raw[j++];
    //         if (val.Type != ResultType.MultiBulk)
    //             cur.Add(key, (RedisValue)val);
    //     }
    //     _results[i - 1] = cur;
    // }
    public readonly struct Row
    {
        private readonly Dictionary<string, RedisValue> _fields;

        internal Row(Dictionary<string, RedisValue> fields)
        {
            _fields = fields;
        }

        public bool ContainsKey(string key) => _fields.ContainsKey(key);
        public RedisValue this[string key] => _fields.TryGetValue(key, out var result) ? result : RedisValue.Null;

        public string GetString(string key) => _fields.TryGetValue(key, out var result) ? (string)result : default;
        public long GetInt64(string key) => _fields.TryGetValue(key, out var result) ? (long)result : default;
        public double GetDouble(string key) => _fields.TryGetValue(key, out var result) ? (double)result : default;
    }
}