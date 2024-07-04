namespace RiverBooks.Books.Tests.Endpoints;
#pragma warning disable CS0618 // Type or member is obsolete
public class BookList(Fixture fixture, ITestOutputHelper outputHelper) : TestClass<Fixture>(fixture, outputHelper)
#pragma warning restore CS0618 // Type or member is obsolete
{
  [Fact]
  public async Task ReturnsThreeBooksAsync()
  {
    var testResult = await Fixture.Client.GETAsync<List, ListBooksResponse>();

    testResult.Response.EnsureSuccessStatusCode();
    testResult.Result.Books.Count.Should().Be(3);
  }
}
