DbContext Data Mocking for Unit Testing
=================
This project demonstrates how a couple of extension methods in addition to the code posted 
in an article/how to named [Testing with Your Own Test Doubles (EF6 onwards)](https://msdn.microsoft.com/en-us/data/dn314431.aspx) make unit testing code that makes use of 
the Entity Framework very easy. 
This github project focuses on data retrieval, not adding or manipulating data although this might be added in the future.

This project also makes use of [NSubstitute](https://github.com/nsubstitute/NSubstitute) as the substitution/mock framework. Really any framework could be used,
for an example of Moq see the original code referenced in the above link.

## Structure
There are 4 library files that can be found in folder: MockHelpers
* If you are using NSubstitute these can be copied directly from this repository without changes necessary. The main file you want to include in your unit test project(s) is `MockExtension.cs`
* If you are using Moq or another mocking framework you will have to change the extension methods found in class `MockExtension` in file `MockExtension.cs`.

### Unit tests
A demonstration can be found in class `DbSetTests` in the root folder in file `DbSetTests.cs`. There are 2 unit tests.
* Test SynchronousTest1 demonstrates a synchronous call to a DbSet contained on a DbContext.
* Test AsynchronousTest1 demonstrates an asynchronous call to a DbSet contained on a DbContext.
Again, both tests make use of NSubstitute but you could plugin your preferred Mocking framework with minimal effort.

Both tests attach the data directly to the generic Set'1 method but really any type of DbSet property or method could be subsittuted.

### Extension Methods
These extension methods do all the work for you.

```
// these 2 methods are the easiest to use and are extension methods on DbContext
AddToDbSetForAsync
AddToDbSet

// These 2 methods are to generate a mocked/substituted DbSet<TEntity> instance from any IEnumerable<TEntity>
GenerateMockDbSet
GenerateMockDbSetForAsync
```