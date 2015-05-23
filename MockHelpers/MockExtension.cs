using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using NSubstitute;

namespace MockDbContextTests
{
	/// <summary>
	/// Extensions to generate DbSets
	/// </summary>
	/// <remarks>
	/// Original code taken from here
	/// https://msdn.microsoft.com/en-us/data/dn314431.aspx
	/// And then refactored into extensions
	/// </remarks>
	public static class MockExtension
	{
		/// <summary>
		/// Creates a mocked generic DbSet of type passed in based on the data supplied
		/// </summary>
		/// <param name="data">The data to emulate in the set</param>
		/// <returns></returns>
		public static DbSet<T> GenerateMockDbSet<T>(this IEnumerable<T> data) where T : class
		{
			return data.AsQueryable().GenerateMockDbSet();
		}

		/// <summary>
		/// Creates a mocked generic DbSet of type passed in based on the data supplied
		/// </summary>
		/// <param name="data">The data to emulate in the set</param>
		/// <returns></returns>
		public static DbSet<T> GenerateMockDbSet<T>(this IQueryable<T> data) where T : class
		{
			var mockSet = Substitute.For<DbSet<T>, IQueryable<T>>();
			var castMockSet = (IQueryable<T>)mockSet;

			castMockSet.Provider.Returns(data.Provider);
			castMockSet.Expression.Returns(data.Expression);
			castMockSet.ElementType.Returns(data.ElementType);
			castMockSet.GetEnumerator().Returns(data.GetEnumerator());
			return mockSet;
		}

		/// <summary>
		/// Creates a mocked generic DbSet of type passed in based on the data supplied
		/// </summary>
		/// <param name="data">The data to emulate in the set</param>
		/// <returns></returns>
		public static DbSet<T> GenerateMockDbSetForAsync<T>(this IEnumerable<T> data) where T : class
		{
			return data.AsQueryable().GenerateMockDbSetForAsync();
		}

		/// <summary>
		/// Generates a mock/fake dbset that can be called using the async keyword
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="queryable">The queryable.</param>
		public static DbSet<T> GenerateMockDbSetForAsync<T>(this IQueryable<T> queryable) where T : class
		{
			var mockSet = Substitute.For<DbSet<T>, IQueryable<T>, IDbAsyncEnumerable<T>>();

			// async support
			var castMockSet = (IQueryable<T>)mockSet;
			var castAsyncEnum = (IDbAsyncEnumerable<T>)mockSet;
			castAsyncEnum.GetAsyncEnumerator().Returns(new TestDbAsyncEnumerator<T>(queryable.GetEnumerator()));
			castMockSet.Provider.Returns(new TestDbAsyncQueryProvider<T>(queryable.Provider));

			castMockSet.Expression.Returns(queryable.Expression);
			castMockSet.ElementType.Returns(queryable.ElementType);
			castMockSet.GetEnumerator().Returns(queryable.GetEnumerator());

			return mockSet;
		}
	}
}