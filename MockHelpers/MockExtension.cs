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
	public static class MockExtension
	{
		/// <summary>
		/// Creates a mocked generic DbSet of type passed in based on the data supplied
		/// </summary>
		/// <param name="queryableEnumerable">The data to emulate in the set</param>
		/// <returns>A mocked DbSet using NSubstitute</returns>
		public static DbSet<TEntity> GenerateMockDbSet<TEntity>(this IEnumerable<TEntity> queryableEnumerable) where TEntity : class
		{
			var queryable = queryableEnumerable as IQueryable<TEntity> ?? queryableEnumerable.AsQueryable();

			var mockSet = Substitute.For<DbSet<TEntity>, IQueryable<TEntity>>();
			var castMockSet = (IQueryable<TEntity>)mockSet;

			castMockSet.Provider.Returns(queryable.Provider);
			castMockSet.Expression.Returns(queryable.Expression);
			castMockSet.ElementType.Returns(queryable.ElementType);
			castMockSet.GetEnumerator().Returns(queryable.GetEnumerator());
			return mockSet;
		}

		/// <summary>
		/// Generates a mock/fake dbset that can be called using the async keyword
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="queryableEnumerable">The queryable.</param>
		public static DbSet<TEntity> GenerateMockDbSetForAsync<TEntity>(this IEnumerable<TEntity> queryableEnumerable) where TEntity : class
		{
			var queryable = queryableEnumerable as IQueryable<TEntity> ?? queryableEnumerable.AsQueryable();

			var mockSet = Substitute.For<DbSet<TEntity>, IQueryable<TEntity>, IDbAsyncEnumerable<TEntity>>();

			// async support
			var castMockSet = (IQueryable<TEntity>)mockSet;
			var castAsyncEnum = (IDbAsyncEnumerable<TEntity>)mockSet;
			castAsyncEnum.GetAsyncEnumerator().Returns(new TestDbAsyncEnumerator<TEntity>(queryable.GetEnumerator()));
			castMockSet.Provider.Returns(new TestDbAsyncQueryProvider<TEntity>(queryable.Provider));

			castMockSet.Expression.Returns(queryable.Expression);
			castMockSet.ElementType.Returns(queryable.ElementType);
			castMockSet.GetEnumerator().Returns(queryable.GetEnumerator());

			return mockSet;
		}
		/// <summary>
		/// Adds the IEnumerable parameter to the DbContext Set (of type DbSet) that can be used using asynchronous calls
		/// </summary>
		/// <param name="context">The context to add the IEnumerable parameter to.</param>
		/// <param name="queryableEnumerable">The enumerable object to add as a DbSet.</param>
		public static void AddToDbSetForAsync<TEntity>(this DbContext context, IEnumerable<TEntity> queryableEnumerable) where TEntity : class
		{
			var set = queryableEnumerable.GenerateMockDbSetForAsync();
			context.Set<TEntity>().Returns(set);
		}
		/// <summary>
		/// Adds the IEnumerable parameter to the DbContext Set (of type DbSet) (can not be used using asynchronous calls)
		/// </summary>
		/// <param name="context">The context to add the IEnumerable parameter to.</param>
		/// <param name="queryableEnumerable">The enumerable object to add as a DbSet.</param>
		public static void AddToDbSet<TEntity>(this DbContext context, IEnumerable<TEntity> queryableEnumerable) where TEntity : class
		{
			var set = queryableEnumerable.GenerateMockDbSetForAsync();
			context.Set<TEntity>().Returns(set);
		}
	}
}