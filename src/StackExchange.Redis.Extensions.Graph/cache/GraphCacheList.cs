using System.Collections.Concurrent;
using System.Linq;

namespace StackExchange.Redis.Extensions.Graph
{
    public sealed class GraphCacheList
    {
        private ConcurrentDictionary<int, string> _cache = new ConcurrentDictionary<int, string>();
        private string _graphId;
        private IDatabase _db;
        private string _procedure;

        public GraphCacheList(string graphId, IDatabase db, string procedure)
        {
            _graphId = graphId;
            _db = db;
            _procedure = procedure;
        }
        public string GetCachedData(int index)
        {

            if (!_cache.TryGetValue(index, out string label))
            {
                var results = (RedisResult[])_db.Execute("GRAPH.QUERY", (RedisKey)_graphId, _procedure);

                ((RedisResult[])results[1]).Select((r, i) => _cache.TryAdd(i, (string)r)).Count();

                label = _cache[index];
            }
            return label;
        }
    }
}