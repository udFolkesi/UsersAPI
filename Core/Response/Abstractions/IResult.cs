using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Response.Abstractions
{
    public interface IResult
    {
        bool Success { get; }
        string? Info { get; }
    }
}
