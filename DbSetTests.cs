using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MockDbContextTests
{
	/// <summary>
	/// A demonstration of how to use the mocked sets
	/// </summary>
	[TestClass]
	public sealed class DbSetTests
	{
		[TestMethod]
		public void SynchronousTest1()
		{
			// arrange
		    var data = new List<MyEntity>()
		    {
		        new MyEntity() {Id = 1, Name = "Entity A", Relations = new List<MyRelatedPropertyEntity>() {new MyRelatedPropertyEntity {Id = 1, Name = "one"}}},
		        new MyEntity() {Id = 2, Name = "Entity B", Relations = new List<MyRelatedPropertyEntity>() {new MyRelatedPropertyEntity {Id = 54, Name = "two"}}},
		        new MyEntity() {Id = 3, Name = "Entity C", Relations = new List<MyRelatedPropertyEntity>() {new MyRelatedPropertyEntity {Id = 51, Name = "three"}}},
		        new MyEntity() {Id = 4, Name = "Entity D", Relations = new List<MyRelatedPropertyEntity>() {new MyRelatedPropertyEntity {Id = 321, Name = "four"}}}
		    };
			// create dbContext - this could be any derived class from DbContext
			var dbContext = NSubstitute.Substitute.For<DbContext>();
			// assign my mock data set using the extension method
			dbContext.AddToDbSet(data);
		    dbContext.Set<MyEntity>().Include("Relations");

			// act
			var result = dbContext.Set<MyEntity>().Where(x => x.Id > 2).OrderBy(x => x.Id).ToList();

			// assert
			Assert.AreEqual(2, result.Count);
			CollectionAssert.AreEqual(new int[]{3,4}, result.Select(x => x.Id).ToArray());
		}


        [TestMethod]
		public async Task AsynchronousTest1()
		{
			// arrange
		    var data = new List<MyEntity>()
		    {
		        new MyEntity() {Id = 1, Name = "Entity A", Relations = new List<MyRelatedPropertyEntity>() {new MyRelatedPropertyEntity {Id = 1, Name = "one"}}},
		        new MyEntity() {Id = 2, Name = "Entity B", Relations = new List<MyRelatedPropertyEntity>() {new MyRelatedPropertyEntity {Id = 54, Name = "two"}}},
		        new MyEntity() {Id = 3, Name = "Entity C", Relations = new List<MyRelatedPropertyEntity>() {new MyRelatedPropertyEntity {Id = 51, Name = "three"}}},
		        new MyEntity() {Id = 4, Name = "Entity D", Relations = new List<MyRelatedPropertyEntity>() {new MyRelatedPropertyEntity {Id = 321, Name = "four"}}}
		    };
			
			// create dbContext - this could be any derived class from DbContext
			var dbContext = NSubstitute.Substitute.For<DbContext>();
			// assign my mock data set using the extension method
			dbContext.AddToDbSetForAsync(data);
            // include
		    dbContext.Set<MyEntity>().MockInclude(x => x.Relations);

			// act - use await and call ToListAsync
			var result = await dbContext.Set<MyEntity>().Where(x => x.Id > 2).OrderBy(x => x.Id).ToListAsync();

			// assert
			Assert.AreEqual(2, result.Count);
			CollectionAssert.AreEqual(new int[] { 3, 4 }, result.Select(x => x.Id).ToArray());
		}
	}

	/// <summary>
	/// A generic entity class that could be linked back to a data store
	/// </summary>
	public sealed class MyEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
        public ICollection<MyRelatedPropertyEntity> Relations { get; set; }
	}

    /// <summary>
	/// A generic entity class that could be linked back to a data store
	/// </summary>
	public sealed class MyRelatedPropertyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
