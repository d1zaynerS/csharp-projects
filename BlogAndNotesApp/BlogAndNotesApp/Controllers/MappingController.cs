using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BlogAndNotesApp.Models;
using BlogAndNotesApp.Helpers;

namespace BlogAndNotesApp.Controllers
{
    public class MappingController : Controller
    {
        private readonly IMapper _mapper;

        public MappingController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var sourceUser = new UserSource { Id = 101, Name = "Іван Петренко", Email = "ivan@example.com" };


            var result1 = sourceUser.ToDestination();


            var result2 = ReflectionMapper.Map<UserSource, UserDestination>(sourceUser);

            var result3 = _mapper.Map<UserDestination>(sourceUser);

            var list = new List<UserDestination> { result1, result2, result3 };
            return View(list);
        }
    }
}