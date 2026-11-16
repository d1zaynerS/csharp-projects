using Microsoft.AspNetCore.Mvc;
using WebApplication12.Services;

namespace WebApplication12.Controllers
{
    public class QuizController : Controller
    {
        [HttpGet]
        public IActionResult Index(int qIndex = 0, int score = 0)
        {
            if (qIndex >= QuizService.Questions.Count)
            {
                ViewBag.Score = score;
                ViewBag.Total = QuizService.Questions.Count;
                return View("Result");
            }

            ViewBag.Question = QuizService.Questions[qIndex];
            ViewBag.QuestionIndex = qIndex;
            ViewBag.Score = score;

            return View();
        }

        [HttpPost]
        public IActionResult Answer(int qIndex, int selectedOption, int score)
        {
            if (QuizService.Questions[qIndex].CorrectIndex == selectedOption)
            {
                score++;
            }

            return RedirectToAction("Index", new { qIndex = qIndex + 1, score = score });
        }
    }
}