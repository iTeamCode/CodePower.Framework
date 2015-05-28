
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace FellowshipOne.Dashboard.XUnitTest
{
    class Customer
    {
        public string CustomerID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class MongoDBTest
    {
        private readonly ITestOutputHelper _output;
        private readonly string _connectionString;
        private readonly string _dbName;

        public MongoDBTest(ITestOutputHelper output)
        {
            this._output = output;
            this._connectionString = "mongodb://localhost:27017";
            this._dbName = "customer";

            //string connectionStr = "mongodb://localhost:27017";
        }
        //[Fact]
        public void InsertData()
        {
            //01.Init database connect.
            MongoClient client = new MongoClient(_connectionString);
            MongoServer server = client.GetServer();        //need new way.
            MongoDatabase db = server.GetDatabase(_dbName);
            _output.WriteLine("Success connected to database:{0}", db.Name);

            //02.Insert data
            var employeeColl = db.GetCollection<BsonDocument>("employee");
            var result = employeeColl.Insert(new BsonDocument() { { "Name", "Alan" }, { "Age", 30 } });
            _output.WriteLine(result.ToString());


            //03.Query data
            var query = Query.EQ("Name", "Alan");

            MongoCursor<BsonDocument> cursor = employeeColl.Find(query);
            foreach (var employee in cursor)
            {
                _output.WriteLine("Find out employee Name is '{0}' , Age is '{1};'", employee["Name"], employee["Age"]);
            }

            //04.Update Query data
            //FindAndModifyArgs args = new FindAndModifyArgs()
            //{
            //    Query = query,
            //    Update = Update.Set("Age", 101),
            //    Upsert = true,
            //    SortBy = null,
            //    VersionReturned = FindAndModifyDocumentVersion.Original
            //};
            //FindAndModifyResult updateResult = employeeColl.FindAndModify(args);

            employeeColl.Update(query, Update.Set("Age", 6688), UpdateFlags.Multi);
            WriteConcernResult removeResult = employeeColl.Remove(query);
            //_output.WriteLine(updateResult.Response.ToString());
        }

        //[Fact]
        public async void InsertDataAsync()
        {            
            //string connectionStr = "mongodb://localhost:27017";
            //MongoClient client = new MongoClient(connectionStr);


            //MongoServer server = client.GetServer();
            ////MongoServer server = MongoDB.Driver.MongoServer.Create(connectionStr);
            //MongoDatabase mongoDatabase = server.GetDatabase("test");
            //MongoCollection<BsonDocument> collection = mongoDatabase.GetCollection<BsonDocument>("foo");

            //MongoCollection<BsonDocument> states = mongoDatabase.GetCollection<BsonDocument>("foo");
            //BsonDocument state = new BsonDocument { { "Name", "YYY" } };
            //states.Insert(state);
            
            //2@@@@@@@@@@@@@@@@@@@@
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("test");
            var collection = database.GetCollection<BsonDocument>("foo");

            await collection.InsertOneAsync(new BsonDocument("Name", "Jack"));

            var list = await collection.Find(new BsonDocument("Name", "Jack")).ToListAsync();

            foreach (var document in list)
            {
                _output.WriteLine(document["Name"].ToString());
            }
           
        }
    }
}
