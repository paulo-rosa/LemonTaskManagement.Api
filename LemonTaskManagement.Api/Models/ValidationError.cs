using Newtonsoft.Json;

namespace LemonTaskManagement.Api.Models;

public class ValidationError(string field = null, string message = null)
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Field { get; } = field != string.Empty ? field : null;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; } = message;
}
