using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MultyFormMessage.Filters
{
    public class CurrencyResourceFilterAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            string currency = context.HttpContext.Request.Query["currency"].ToString();
            if (!string.IsNullOrEmpty(currency))
            {
                context.HttpContext.Response.Cookies.Append("SelectedCurrency", currency);
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}