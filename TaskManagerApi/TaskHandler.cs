using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerApi
{
    public class TaskHandler
    {
        public IResult Get(string taskName)
        {
            return Results.Ok("some task");
        }
    }
}