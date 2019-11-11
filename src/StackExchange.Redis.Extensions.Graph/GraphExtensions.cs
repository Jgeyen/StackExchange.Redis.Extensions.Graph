using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StackExchange.Redis;

namespace StackExchange.Redis.Extensions.Graph
{



    public static class GraphExtensions
    {
        public static GraphResult Query(this IDatabase db, string graphid, GraphCache cache, string query, params object[] args)
        {
            var preparedQuery = string.Format(query, args);
            return Query(db, graphid, preparedQuery, cache);
        }

        public static GraphResult Query(this IDatabase db, string graphid, string query, GraphCache cache)
        {
            var result = db.Execute("GRAPH.QUERY", (RedisKey)graphid, query, "--compact");
            return new GraphResult(result, cache);
        }
    }

    
}