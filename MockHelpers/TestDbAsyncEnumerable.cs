using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace MockDbContextTests
{
	/// <summary>
	/// Original code found here: https://msdn.microsoft.com/en-us/data/dn314431.aspx
	/// </summary>
	internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
	{
		public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
			: base(enumerable)
		{ }

		public TestDbAsyncEnumerable(Expression expression)
			: base(expression)
		{ }

		public IDbAsyncEnumerator<T> GetAsyncEnumerator()
		{
			return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
		}

		IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
		{
			return GetAsyncEnumerator();
		}

		IQueryProvider IQueryable.Provider
		{
			get { return new TestDbAsyncQueryProvider<T>(this); }
		}
	}
}