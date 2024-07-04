namespace RiverBooks.Books.Tests.Endpoints;
#pragma warning disable CS0618 // Type or member is obsolete
public class BookGetById(Fixture fixture, ITestOutputHelper outputHelper) : TestClass<Fixture>(fixture, outputHelper)
#pragma warning restore CS0618 // Type or member is obsolete
{
 [Theory]
 [InlineData("A89F6CD7-3D3D-4D3D-8D3D-3D3D3D3D3D3D", "The Fellowship of the Ring")]
 [InlineData("B89F6CD7-3D3D-4D3D-8D3D-3D3D3D3D3D3D", "The Two Towers")]
 [InlineData("C89F6CD7-3D3D-4D3D-8D3D-3D3D3D3D3D3D", "The Return of the King")]
 public async Task ReturnsExpectedBookGivenIdAsync(string validId, string expectedTitle)
 {
   Guid id = Guid.Parse(validId);
    var request = new GetBookByIdRequest { Id = id };
    var testResult = await Fixture
      .Client.GETAsync<GetById, GetBookByIdRequest, BookDto>(request);
    testResult.Response.EnsureSuccessStatusCode();
    testResult.Result.Title.Should().Be(expectedTitle);
 }
}
