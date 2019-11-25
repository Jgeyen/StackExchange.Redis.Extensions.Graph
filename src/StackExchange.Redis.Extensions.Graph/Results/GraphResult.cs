using System.Collections.Generic;

namespace StackExchange.Redis.Extensions.Graph {
    public sealed class GraphResult {
        private readonly RedisResult[] _results;

        public GraphResultStats Stats { get; private set; }

        public GraphResultHeader Header { get; private set; }

        internal GraphResult(RedisResult result) {
            var results = (RedisResult[])result;
            Stats = new GraphResultStats(results);

            if (results.Length > 1) {
                Header = new GraphResultHeader(results);
                _results = (RedisResult[])results[1];
            }
        }

        public RedisResult[] GetResults() => _results;

        public RedisResult[] this[int index]
            => index >= _results.Length ? null : (RedisResult[])_results[index];

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
    public readonly struct Row {
        private readonly Dictionary<string, RedisValue> _fields;

        internal Row(Dictionary<string, RedisValue> fields) {
            _fields = fields;
        }

        public bool ContainsKey(string key) => _fields.ContainsKey(key);
        public RedisValue this[string key] => _fields.TryGetValue(key, out var result) ? result : RedisValue.Null;

        public string GetString(string key) => _fields.TryGetValue(key, out var result) ? (string)result : default;
        public long GetInt64(string key) => _fields.TryGetValue(key, out var result) ? (long)result : default;
        public double GetDouble(string key) => _fields.TryGetValue(key, out var result) ? (double)result : default;
    }
}