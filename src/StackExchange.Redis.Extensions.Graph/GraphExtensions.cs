using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StackExchange.Redis;

namespace StackExchange.Redis.Extensions.Graph {
    public static class GraphExtensions {
        public static GraphResult Query(this IDatabase db, string graphid, string query, params object[] args) {
            new GraphCache(graphid, db);

            var preparedQuery = string.Format(query, args);
            return Query(db, graphid, preparedQuery);
        }

        public static GraphResult Query(this IDatabase db, string graphid, string query) {
            new GraphCache(graphid, db);

            var result = db.Execute("GRAPH.QUERY", (RedisKey)graphid, query, "--compact");
            return new GraphResult(result);
        }
    }


}