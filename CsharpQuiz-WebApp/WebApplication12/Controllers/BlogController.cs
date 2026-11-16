using Microsoft.AspNetCore.Mvc;
using WebApplication12.Models;

namespace WebApplication12.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            var posts = new List<BlogPost>
            {
                new BlogPost { Title = "Вступ до ASP.NET Core", Content = "ASP.NET Core — це кросплатформний фреймворк високої продуктивності з відкритим вихідним кодом для створення сучасних хмарних веб-додатків. Всі нові можливості роблять його дуже популярним серед розробників по всьому світу." },
                new BlogPost { Title = "Використання Tag Helpers", Content = "Tag Helpers дозволяють серверному коду брати участь у створенні та рендерингу HTML-елементів у файлах Razor. Вони є чудовою альтернативою стандартним HTML-хелперам." },
                new BlogPost { Title = "Паттерн MVC", Content = "Model-View-Controller розділяє програму на три основні компоненти: Модель (дані та бізнес-логіка), Представлення (інтерфейс користувача) та Контролер (управління потоком виконання)." }
            };

            return View(posts);
        }
    }
}