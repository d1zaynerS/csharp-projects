using WebApplication12.Models;

namespace WebApplication12.Services
{
    public static class QuizService
    {
        public static List<Question> Questions { get; } = new List<Question>
        {
            new Question { Id = 1, Text = "Що таке C#?", Options = new List<string>{ "Мова програмування", "СУБД", "Операційна система", "Фреймворк" }, CorrectIndex = 0 },
            new Question { Id = 2, Text = "Що таке ASP.NET Core?", Options = new List<string>{ "База даних", "Веб-фреймворк", "Текстовий редактор", "Браузер" }, CorrectIndex = 1 },
            new Question { Id = 3, Text = "Який паттерн використовує ASP.NET MVC?", Options = new List<string>{ "Singleton", "Model-View-Controller", "Factory", "Observer" }, CorrectIndex = 1 },
            new Question { Id = 4, Text = "Що таке Tag Helper?", Options = new List<string>{ "Тег HTML", "Спеціальний C# атрибут для Razor", "Скрипт JS", "Стиль CSS" }, CorrectIndex = 1 },
            new Question { Id = 5, Text = "Який тип повертає розширення HtmlHelper?", Options = new List<string>{ "HtmlString", "Controller", "DbContext", "ViewData" }, CorrectIndex = 0 },
            new Question { Id = 6, Text = "Що повертає Action-метод контролера?", Options = new List<string>{ "JsonResult", "IActionResult", "ContentResult", "StatusCodeResult" }, CorrectIndex = 1 },
            new Question { Id = 7, Text = "Який атрибут обробляє POST-запити?", Options = new List<string>{ "[HttpGet]", "[HttpPost]", "[HttpPut]", "[HttpDelete]" }, CorrectIndex = 1 }
        };
    }
}