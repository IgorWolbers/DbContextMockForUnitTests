using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

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
				new MyEntity(){Id = 1, Name = "Entity A"},
				new MyEntity(){Id = 2, Name = "Entity B"},
				new MyEntity(){Id = 3, Name = "Entity C"},
				new MyEntity(){Id = 4, Name = "Entity D"}
			};
			// call extension method
			var dbSet = data.GenerateMockDbSet();
			// create dbContext - this could be any derived class from DbContext
			var dbContext = NSubstitute.Substitute.For<DbContext>();
			// assign my mock data set
			dbContext.Set<MyEntity>().Returns(dbSet);

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
				new MyEntity(){Id = 1, Name = "Entity A"},
				new MyEntity(){Id = 2, Name = "Entity B"},
				new MyEntity(){Id = 3, Name = "Entity C"},
				new MyEntity(){Id = 4, Name = "Entity D"}
			};
			
			// call extension method for asynchronous capable dbset
			var dbSet = data.GenerateMockDbSetForAsync();
			// create dbContext - this could be any derived class from DbContext
			var dbContext = NSubstitute.Substitute.For<DbContext>();
			// assign my mock data set
			dbContext.Set<MyEntity>().Returns(dbSet);

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
	}
}
