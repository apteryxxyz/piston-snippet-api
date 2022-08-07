using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text;
namespace Backend.Testing;

public class Tests
{
    private JsonNode _snippet = default!;
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://localhost:3000/api/");
    }

    // Method to test the creation of snippets
    [Test, Order(1)]
    public async Task Create()
    {
        // Format request content
        var data = new
        {
            language = "python",
            version = "3.10.0",
            code = "print('hello world')",
        };
        var requestBody = JsonSerializer.Serialize(data);
        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        // Make request
        var response = await _client.PutAsync("snippets", content);
        var responseBody = await response.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<JsonNode>(responseBody);

        // Asserts
        if (json == null || json["id"] == null) Assert.Fail();
        else
        {
            var key = (string)json["key"]!;
            _snippet = json;
            Assert.Pass();
        }
    }

    // Method to test the getting of a snippet
    [Test, Order(2)]
    public async Task Get()
    {
        // Ensure a snippet was created before this test
        if (_snippet == null) Assert.Fail("Must run Create test before Get");
        else
        {
            // Add key to headers
            var key = (string)_snippet["key"]!;
            _client.DefaultRequestHeaders.Add("key", key);

            // Make request
            var id = (string)_snippet["id"]!;
            var response = await _client.GetAsync($"snippets/{id}");
            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonNode>(responseBody);

            // Asserts
            if (json == null || json["id"] == null) Assert.Fail();
            else
            {
                Assert.Multiple(() =>
                {
                    Assert.That((string)json["id"]!, Is.EqualTo(id));
                    Assert.That(json["key"]!, Is.EqualTo(null));
                });
            }
        }
    }

    // Method to test updating a snippet
    [Test, Order(2)]
    public async Task Patch()
    {
        // Ensure a snippet was created before this test
        if (_snippet == null) Assert.Fail("Must run Create test before Patch");
        else
        {
            // Format the request content/body
            var data = new
            {
                code = "print('Hello World')",
            };
            var requestBody = JsonSerializer.Serialize(data);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            // Add key to headers
            var key = (string)_snippet["key"]!;
            _client.DefaultRequestHeaders.Add("key", key);

            // Make request
            var id = (string)_snippet["id"]!;
            var response = await _client.PatchAsync($"snippets/{id}", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonNode>(responseBody);

            // Asserts
            if (json == null || json["id"] == null) Assert.Fail();
            else
            {
                Assert.Multiple(() =>
                {
                    Assert.That((string)json["id"]!, Is.EqualTo(id));
                    Assert.That(json["key"]!, Is.EqualTo(null));
                    Assert.That((string)json["code"]!, Is.EqualTo(data.code));
                });
                _snippet["code"] = data.code;
            }
        }
    }

    // Method to test executing a snippet
    [Test, Order(3)]
    public async Task Execute()
    {
        // Ensure a snippet was created before this test
        if (_snippet == null) Assert.Fail("Must run Create test before Execute");
        else
        {
            // Add key to headers
            var key = (string)_snippet["key"]!;
            _client.DefaultRequestHeaders.Add("key", key);

            // Make request
            var id = (string)_snippet["id"]!;
            var response = await _client.GetAsync($"snippets/{id}/execute");
            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonNode>(responseBody);

            // Asserts
            if (json == null || json["output"] == null) Assert.Fail();
            else
            {
                Assert.Multiple(() =>
                {
                    var language = (string)_snippet["language"]!;
                    var output = "Hello World\n";
                    Assert.That((string)json["language"]!, Is.EqualTo(language));
                    Assert.That((string)json["output"]!, Is.EqualTo(output));
                });
            }
        }
    }

    // Method to test deleting a snippet
    [Test, Order(4)]
    public async Task Delete()
    {
        // Ensure a snippet was created before this test
        if (_snippet == null) Assert.Fail("Must run Create test before Execute");
        else
        {
            // Add key to headers
            var key = (string)_snippet["key"]!;
            _client.DefaultRequestHeaders.Add("key", key);

            // Make request
            var id = (string)_snippet["id"]!;
            var response = await _client.DeleteAsync($"snippets/{id}");
            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonNode>(responseBody);

            if (json == null || json["id"] == null) Assert.Fail();
            else Assert.Pass();
        }
    }
}