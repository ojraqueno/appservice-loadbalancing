// Set up configuration values that control the behavior of the application
using System.Text.Json;

const int secondsInterval = 2;
const double secondsDuration = 300;
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
    Response? response;
    try
    {
        var rawResponse = await httpClient.GetAsync("/ping");
        var content = await rawResponse.Content.ReadAsStringAsync();
        response = JsonSerializer.Deserialize<Response>(content);
    }
    catch (Exception)
    {
        continue;
    }
    
    if (response == null || string.IsNullOrWhiteSpace(response.instanceId)) throw new InvalidOperationException("Invalid response.");

    if (!instanceIdCount.ContainsKey(response.instanceId)) instanceIdCount.Add(response.instanceId, 1);
    else instanceIdCount[response.instanceId]++;

    WriteInstanceCounts(instanceIdCount);

    if (DateTime.Now > stopTime)
    {
        break;
    }
}

Console.WriteLine("End");

void WriteInstanceCounts(Dictionary<string, int> instanceIdCount)
{
    Console.Clear();
    Console.WriteLine("Instance Id                                                      : Response Count From Instance");
    foreach (var kvp in instanceIdCount)
    {
        Console.WriteLine($"{kvp.Key} : {kvp.Value}");
    }
}

public class Response
{
    public string instanceId { get; set; }
}