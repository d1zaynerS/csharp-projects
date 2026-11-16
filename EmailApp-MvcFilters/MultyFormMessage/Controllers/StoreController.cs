using Microsoft.AspNetCore.Mvc;
using MultyFormMessage.Filters;

namespace MultyFormMessage.Controllers
{
    public class StoreController : Controller
    {
        [CurrencyResourceFilter]
        public IActionResult SelectCurrency(string currency)
        {
            return Content($"Вибрану валюту ({currency}) збережено у файли куки!");
        }
    }
}