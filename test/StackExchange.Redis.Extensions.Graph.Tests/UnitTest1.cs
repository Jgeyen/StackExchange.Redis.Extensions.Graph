using System;
using NUnit.Framework;
using StackExchange.Redis.Extensions.Graph;
namespace StackExchange.Redis.Extensions.Graph.Tests
{
    public class Tests
    {
        IDatabase db;
        [SetUp]
        public void Setup()
        {
            db = ConnectionMultiplexer.Connect("localhost").GetDatabase();
        }

        [Test]
        public void CreateNodeReturnsCorrectNodeCreatedResults()
        {
            var result = db.Query("jasonfun", "CREATE (:person{name:'roi',age:32})");

            Assert.AreEqual(result.Stats.NodesCreated, 1);
            Assert.AreEqual(result.Stats.PropertiesSet, 2);
        }
        [Test]
        public void CreateNodeAndRelationshipReturnsCorrectResults()
        {
            var result = db.Query("jasonfun", "CREATE (:plant {name: 'Tree'})-[:GROWS {season: 'Autumn'}]->(:fruit {name: 'Apple'})");

            Assert.AreEqual(result.Stats.NodesCreated, 2);
            Assert.AreEqual(result.Stats.PropertiesSet, 3);
            Assert.AreEqual(result.Stats.RelationshipsCreated, 1);
        }

        [Test]
        public void CreateNodeWithParamsReturnsCorrectResults()
        {
            var name = "amit";
            var age = 30;
            var result = db.Query("jasonfun", "CREATE (:person{{name:'{0}',age:{1}}})", name, age);

            Assert.AreEqual(result.Stats.NodesCreated, 1);
            Assert.AreEqual(result.Stats.PropertiesSet, 2);
        }
        [Test]
        public void QueryReturnsCorrectResults()
        {
            var result = db.Query("jasonfun", "MATCH (a:person)-[r:knows]->(b:person) RETURN a, r, b.name");

            Assert.AreEqual(result.Header[0].columnType, ColumnType.COLUMN_NODE);
            Assert.AreEqual(result.Header[1].columnType, ColumnType.COLUMN_RELATION);
            Assert.AreEqual(result.Header[2].columnType, ColumnType.COLUMN_SCALAR);
            Assert.AreEqual(result.Header[0].Name, "a");
            Assert.AreEqual(result.Header[1].Name, "r");
            Assert.AreEqual(result.Header[2].Name, "b.name");

        }
    }
}