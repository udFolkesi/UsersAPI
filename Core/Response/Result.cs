using Core.Enums;
using Core.Response.Abstractions;

namespace Core.Response
{
    public class Result: IResult
    {
        public bool Success { get; private set; }
        public string? Info { get; private set; }

        public Result(bool success, string? info)
        {
            Success = success;
            Info = info;
        }
    }
}