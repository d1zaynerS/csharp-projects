using Microsoft.AspNetCore.Mvc;

namespace WebApplication12.Controllers
{
    public class MatrixController : Controller
    {
        [HttpGet]
        public IActionResult Index(int size = 3)
        {
            ViewBag.Size = size;
            return View();
        }

        [HttpPost]
        public IActionResult Calculate(IFormCollection form)
        {
            int size = int.Parse(form["size"]);
            string operation = form["operation"];

            int[,] a = new int[size, size];
            int[,] b = new int[size, size];
            int[,] result = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    a[i, j] = int.Parse(form[$"a_{i}_{j}"]);
                    b[i, j] = int.Parse(form[$"b_{i}_{j}"]);
                }
            }

            if (operation == "Add")
            {
                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                        result[i, j] = a[i, j] + b[i, j];
            }
            else if (operation == "Multiply")
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        result[i, j] = 0;
                        for (int k = 0; k < size; k++)
                        {
                            result[i, j] += a[i, k] * b[k, j];
                        }
                    }
                }
            }

            ViewBag.Size = size;
            ViewBag.Operation = operation == "Add" ? "Додавання" : "Множення";
            ViewBag.Result = result;

            return View("Result");
        }
    }
}