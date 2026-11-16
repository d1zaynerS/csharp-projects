using Microsoft.AspNetCore.Mvc.Filters;
using MultyFormMessage.Models;
using System;

namespace MultyFormMessage.Filters
{
    public class TimestampActionFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("item", out var argument) && argument is TodoItem item)
            {
                if (item.Id == 0)
                {
                    item.CreatedAt = DateTime.Now;
                }
                item.LastUpdatedAt = DateTime.Now;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}