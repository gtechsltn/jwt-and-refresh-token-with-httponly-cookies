using System.Net;

namespace Api.Testing.Dtos
{
    public class ApiResult
    {
        public bool Success { get; set; }
        public HttpStatusCode Status { get; set; }
        public int StatusCode => (int)Status;
        public string[] Errors { get; set; }

        public static implicit operator ApiResult(bool value)
        {
            return new()
            {
                Success = value,
                Status = value ? HttpStatusCode.OK : HttpStatusCode.BadRequest
            };
        }
    }

    public class ApiResult<T> : ApiResult
    {
        public ApiResult()
        {
        }

        public ApiResult(string[] errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            Success = false;
            Status = statusCode;
            Errors = errors;
        }

        public T Data { get; set; }

        public static implicit operator ApiResult<T>(T value)
        {
            return new()
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = value,
            };
        }
    }
}