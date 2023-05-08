// Set up configuration values that control the behavior of the application
const int secondsInterval = 2;
const double secondsDuration = 30;
const string apiUrl = "https://app-myapi.azurewebsites.net/";

var httpClient = new HttpClient { BaseAddress = new Uri(apiUrl) };
var instanceIdCount = new Dictionary<string, int>();
var timer = new PeriodicTimer(TimeSpan.FromSeconds(secondsInterval));
var stopTime = DateTime.Now.AddSeconds(secondsDuration);

Console.WriteLine("Instance Id : Response Count From Instance");
Console.WriteLine("waiting     : waiting");

// Periodically call the API and write out the response, noting the instance count
while (await timer.WaitForNextTickAsync())
{
    var rawResponse = await httpClient.GetAsync("/ping");
    var content = await rawResponse.Content.ReadAsStringAsync();
    var response = System.Text.Json.JsonSerializer.Deserialize<Response>(content);
    if (response == null || string.IsNullOrWhiteSpace(response.instanceId)) continue;

    if (!instanceIdCount.ContainsKey(response.instanceId)) instanceIdCount.Add(response.instanceId, 1);
    else instanceIdCount[response.instanceId]++;

    Console.Clear();
    Console.WriteLine("Instance Id                                                      : Response Count From Instance");
    foreach (var kvp in instanceIdCount)
    {
        Console.WriteLine($"{kvp.Key} : {kvp.Value}");
    }

    if (DateTime.Now > stopTime)
    {
        break;
    }
}

Console.WriteLine("End");

public class Response
{
    public string instanceId { get; set; }
}