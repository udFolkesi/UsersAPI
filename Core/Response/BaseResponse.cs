using Core.Enums;
using Core.Response.Abstractions;

namespace Core.Response
{
    public class BaseResponse: IBaseResponse
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public static BaseResponse Success() => new() { IsSuccess = true };
        public static BaseResponse Fail(string message) => new() { IsSuccess = false, ErrorMessage = message };
    }
}