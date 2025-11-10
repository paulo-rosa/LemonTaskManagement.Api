using Newtonsoft.Json;

namespace LemonTaskManagement.Api.Models;

public class ApiResponse<T>(int statusCode = -1, string message = "", T result = default, ApiError apiError = null)
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int StatusCode { get; set; } = statusCode;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; set; } = message;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ApiError Exception { get; set; } = apiError;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public T Result { get; set; } = result;
}
