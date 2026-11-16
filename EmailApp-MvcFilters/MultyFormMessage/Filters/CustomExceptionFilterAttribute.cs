using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MultyFormMessage.Filters
{
    public class CustomExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName ?? "Action";
            string exceptionMessage = context.Exception.Message;

            context.Result = new ContentResult
            {
                Content = $"Виникла помилка під час виконання [{actionName}]: {exceptionMessage}"
            };
            context.ExceptionHandled = true;
        }
    }
}