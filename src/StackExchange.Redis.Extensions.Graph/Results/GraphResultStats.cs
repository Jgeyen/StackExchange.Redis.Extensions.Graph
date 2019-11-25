using System;
using System.Text.RegularExpressions;

namespace StackExchange.Redis.Extensions.Graph {
    ///"Labels added: (integer)"
    ///"Nodes created: (integer)"
    ///"Properties set: (integer)"
    ///"Nodes deleted: (integer)"
    ///"Relationships deleted: (integer)"
    ///"Relationships created: (integer)"
    ///"Query internal execution time: (float) milliseconds"
    public sealed class GraphResultStats {
        public TimeSpan ExecutionTime { get; private set; }

        public int LabelsAdded { get; private set; }
        public int NodesCreated { get; private set; }
        public int NodesDeleted { get; private set; }
        public int PropertiesSet { get; private set; }
        public int RelationshipsCreated { get; private set; }
        public int RelationshipsDeleted { get; private set; }

        internal GraphResultStats(RedisResult[] results) {
            var stats = (RedisResult[])results[results.Length - 1];

            foreach (RedisResult stat in stats) {
                var raw = (string)stat;

                switch (raw.Substring(0, raw.LastIndexOf(':'))) {
                    case "Labels added":
                        LabelsAdded = int.Parse(ExtractNumber(raw));
                        break;
                    case "Nodes created":
                        NodesCreated = int.Parse(ExtractNumber(raw));
                        break;
                    case "Nodes deleted":
                        NodesDeleted = int.Parse(ExtractNumber(raw));
                        break;
                    case "Properties set":
                        PropertiesSet = int.Parse(ExtractNumber(raw));
                        break;
                    case "Relationships created":
                        RelationshipsCreated = int.Parse(ExtractNumber(raw));
                        break;
                    case "Relationships deleted":
                        RelationshipsDeleted = int.Parse(ExtractNumber(raw));
                        break;
                    case "Query internal execution time":
                        ExecutionTime = TimeSpan.FromMilliseconds(double.Parse(ExtractNumber(raw)));
                        break;

                }
            }
        }
        private string ExtractNumber(string result) {
            var value = Regex.Match(result, @"([0-9]*[.])?[0-9]+").Value;
            return value; //todo: assess situation where it doesn't find number?
        }
    }
}