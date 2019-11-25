using NUnit.Framework;
namespace StackExchange.Redis.Extensions.Graph.Tests {
    public class Tests {
        IDatabase db;

        [SetUp]
        public void Setup() {
            db = ConnectionMultiplexer.Connect("localhost").GetDatabase();
        }

        [Test]
        public void CreateNodeReturnsCorrectNodeCreatedResults() {
            var result = db.Query("jasonfun", "CREATE (:person{name:'roi',age:32})");

            Assert.AreEqual(1, result.Stats.NodesCreated);
            Assert.AreEqual(2, result.Stats.PropertiesSet);
        }
        [Test]
        public void CreateNodeAndRelationshipReturnsCorrectResults() {
            var result = db.Query("jasonfun", "CREATE (:plant {name: 'Tree'})-[:GROWS {season: 'Autumn'}]->(:fruit {name: 'Apple'})");

            Assert.AreEqual(2, result.Stats.NodesCreated);
            Assert.AreEqual(3, result.Stats.PropertiesSet);
            Assert.AreEqual(1, result.Stats.RelationshipsCreated);
        }

        [Test]
        public void CreateNodeWithParamsReturnsCorrectResults() {
            var name = "amit";
            var age = 30;
            var result = db.Query("jasonfun", "CREATE (:person{{name:'{0}',age:{1}}})", name, age);

            Assert.AreEqual(1, result.Stats.NodesCreated);
            Assert.AreEqual(2, result.Stats.PropertiesSet);
        }
        [Test]
        public void QueryReturnsCorrectHeader() {
            var result = db.Query("jasonfun", "CREATE (:plant {name: 'Tree'})-[:GROWS {season: 'Autumn'}]->(:fruit {name: 'Apple'})");

            result = db.Query("jasonfun", "MATCH (a)-[e]->(b) RETURN a, e, b.name");

            Assert.AreEqual(ColumnType.COLUMN_NODE, result.Header[0].columnType);
            Assert.AreEqual(ColumnType.COLUMN_RELATION, result.Header[1].columnType);
            Assert.AreEqual(ColumnType.COLUMN_SCALAR, result.Header[2].columnType);
            Assert.AreEqual("a", result.Header[0].Name);
            Assert.AreEqual("e", result.Header[1].Name);
            Assert.AreEqual("b.name", result.Header[2].Name);
        }
        [Test]
        public void QueryReturnsCorrectNode() {
            var result = db.Query("jasonfun", "CREATE (:plant {name: 'Tree'})-[:GROWS {season: 'Autumn'}]->(:fruit {name: 'Apple'})");

            result = db.Query("jasonfun", "MATCH (a)-[e]->(b) RETURN a, e, b.name");

            var treeNode = new Node(result[0][0]);
            Assert.AreEqual("plant", treeNode.Labels[0]);
            Assert.AreEqual("Tree", treeNode.Property<string>("name"));

        }
        [Test]
        public void QueryReturnsCorrectEdge() {
            var result = db.Query("jasonfun", "CREATE (:plant {name: 'Tree'})-[:GROWS {season: 'Autumn'}]->(:fruit {name: 'Apple'})");

            result = db.Query("jasonfun", "MATCH (a)-[e]->(b) RETURN a, e, b.name");

            var relation = new Edge(result[0][1]);

            Assert.AreEqual("GROWS", relation.RelationshipType.ToUpper());
            Assert.AreEqual("Autumn", relation.Property<string>("season"));

        }
        [Test]
        public void QueryReturnsCorrectScalar() {
            var result = db.Query("jasonfun", "CREATE (:plant {name: 'Tree'})-[:GROWS {season: 'Autumn'}]->(:fruit {name: 'Apple'})");

            result = db.Query("jasonfun", "MATCH (a)-[e]->(b) RETURN a, e, b.name");

            var scalar = new GraphResultScalar(result[0][2]);
            Assert.AreEqual("Apple", scalar.Value.ToString());

        }
    }
}