DbContext Data Mocking for Unit Testing
=================
This project demonstrates how a couple of extension methods in addition to the code posted 
in an article/how to named [Testing with Your Own Test Doubles (EF6 onwards)](https://msdn.microsoft.com/en-us/data/dn314431.aspx) make unit testing code that makes use of 
the Entity Framework very easy. 
This github project focuses on data retrieval, not adding or manipulating data although this might be added in the future.

This project also makes use of [NSubstitute](https://github.com/nsubstitute/NSubstitute) as the substitution/mock framework. Really any framework could be used,
for an example of Moq see the original code referenced in the above link.

## Structure
There are 4 library files that can be found in folder `MockHelpers`. Copy and include all 4 files to your Test project.
* If you are using NSubstitute these can be copied directly from this repository without changes necessary.
* If you are using Moq or another mocking framework you will have to change the extension methods found in class `MockExtension` in file `MockExtension.cs` as these are NSubstitute specific.

## Unit tests
A demonstration can be found in class `DbSetTests` in the root folder in file `DbSetTests.cs`. There are 2 unit tests.
* Test SynchronousTest1 demonstrates a synchronous call to a DbSet contained on a DbContext.
* Test AsynchronousTest1 demonstrates an asynchronous call to a DbSet contained on a DbContext.
Again, both tests make use of NSubstitute but you could plugin your preferred Mocking framework with minimal effort.

Both tests attach the data directly to the generic Set'1 method but really any type of DbSet property or method could be subsittuted.

## Extension Methods
These extension methods do all the work for you.

```
// these 2 methods are the easiest to use and are extension methods on DbContext
AddToDbSetForAsync
AddToDbSet

// These 2 methods are to generate a mocked/substituted DbSet<TEntity> instance from any IEnumerable<TEntity>
GenerateMockDbSet
GenerateMockDbSetForAsync
```

## Examples
### Example 1

```
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
	
	// create dbContext - this could be any derived class from DbContext
	var dbContext = NSubstitute.Substitute.For<DbContext>();

	// assign my mock data set using the extension method
	dbContext.AddToDbSetForAsync(data);

	// act - use await and call ToListAsync. Typically this would be executed from inside some other Type that is being tested
	var result = await dbContext.Set<MyEntity>().Where(x => x.Id > 2).OrderBy(x => x.Id).ToListAsync();

	// assert
	Assert.AreEqual(2, result.Count);
	CollectionAssert.AreEqual(new int[] { 3, 4 }, result.Select(x => x.Id).ToArray());
}
```