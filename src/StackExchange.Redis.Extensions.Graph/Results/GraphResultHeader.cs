namespace StackExchange.Redis.Extensions.Graph
{
    public enum ColumnType
    {
        COLUMN_UNKNOWN = 0,
        COLUMN_SCALAR = 1,
        COLUMN_NODE = 2,
        COLUMN_RELATION = 3,
    }

    public sealed class GraphResultHeader
    {
        private (ColumnType columnType, string Name)[] headerList;

        public (ColumnType columnType, string Name) this[int index] => headerList[index];

        internal GraphResultHeader(RedisResult[] results)
        {
            var headers = (RedisResult[])results[0];
            headerList = new (ColumnType columnType, string Name)[headers.Length];

            for (int i = 0; i < headers.Length; i++)
            {
                var header = (RedisResult[])headers[i];

                var columnType = (ColumnType)((int)header[0]);
                headerList[i] = (columnType, (string)header[1]);

            }
        }
    }
}