using Newtonsoft.Json;

namespace LemonTaskManagement.Api.Models;

public class ApiError(string message = "Please correct the specified validation errors and try again.", IEnumerable<ValidationError> errors = null)
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; set; } = message;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Details { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IEnumerable<ValidationError> Errors { get; set; } = errors;
}
