using System.Collections.Generic;

namespace LemonTaskManagement.Domain.Core.Models;

public class Response<T> : Response
{
    public T Data { get; set; }
}

public class Response
{
    public bool Success { get; set; } = true;

    public string Message { get; set; }

    public IEnumerable<Error> Errors { get; set; }
}

public class Error(string property = null, string message = null)
{
    public string Property { get; set; } = property;

    public string Message { get; set; } = message;
}
