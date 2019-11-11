namespace StackExchange.Redis.Extensions.Graph
{
    /**
     * A class to store a local cache in the client, for a specific graph.
     * Holds the labels, property names and relationship types
     */
    public class GraphCache
    {
        private static GraphCacheList labels;
        private static GraphCacheList propertyNames;
        private static GraphCacheList relationshipTypes;


        /**
         *
         * @param graphId - graph Id
         */
        public GraphCache(string graphId, IDatabase db)
        {
            labels = new GraphCacheList(graphId, db, "CALL db.labels()");
            propertyNames = new GraphCacheList(graphId, db, "CALL db.propertyKeys()");
            relationshipTypes = new GraphCacheList(graphId, db, "CALL db.relationshipTypes()");
        }

        /**
         * @param index - index of label
         * @return requested label
         */
        public static string Label(int index)
        {
            return labels.GetCachedData(index);
        }

        /**
         * @param index index of the relationship type
         * @return requested relationship type
         */
        public static string RelationshipType(int index)
        {
            return relationshipTypes.GetCachedData(index);
        }

        /**
         * @param index index of property name
         * @return requested property
         */
        public static string PropertyName(int index)
        {
            return propertyNames.GetCachedData(index);
        }
    }
}