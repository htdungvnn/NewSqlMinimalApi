using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NewSqlMinimalApi;
using Xunit;

public class ProductApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProducts_ReturnsOkAndList()
    {
        var response = await _client.GetAsync("/products");
        response.EnsureSuccessStatusCode();

        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.NotNull(products);
    }

    [Fact]
    public async Task PostProduct_CreatesProduct()
    {
        var newProduct = new Product { Name = "Test", Price = 1.23M };
        var response = await _client.PostAsJsonAsync("/products", newProduct);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdProduct = await response.Content.ReadFromJsonAsync<Product>();
        Assert.NotNull(createdProduct);
        Assert.Equal("Test", createdProduct.Name);
        Assert.Equal(1.23M, createdProduct.Price);
    }

    [Fact]
    public async Task PutProduct_UpdatesProduct()
    {
        // Create product first
        var newProduct = new Product { Name = "Original", Price = 10 };
        var postResponse = await _client.PostAsJsonAsync("/products", newProduct);
        var created = await postResponse.Content.ReadFromJsonAsync<Product>();

        // Update it
        created.Name = "Updated";
        created.Price = 20;

        var putResponse = await _client.PutAsJsonAsync($"/products/{created.Id}", created);
        Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);

        // Verify update
        var getResponse = await _client.GetAsync($"/products/{created.Id}");
        var updatedProduct = await getResponse.Content.ReadFromJsonAsync<Product>();
        Assert.Equal("Updated", updatedProduct.Name);
        Assert.Equal(20, updatedProduct.Price);
    }

    [Fact]
    public async Task DeleteProduct_RemovesProduct()
    {
        // Create product first
        var newProduct = new Product { Name = "ToDelete", Price = 5 };
        var postResponse = await _client.PostAsJsonAsync("/products", newProduct);
        var created = await postResponse.Content.ReadFromJsonAsync<Product>();

        // Delete it
        var deleteResponse = await _client.DeleteAsync($"/products/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Confirm deletion
        var getResponse = await _client.GetAsync($"/products/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}