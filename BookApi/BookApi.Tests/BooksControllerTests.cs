using BookApi.Controllers;
using BookApi.Models;
using BookApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BookApi.Tests
{
    public class BooksControllerTests
    {
        private readonly IBookRepository _repository;
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            _repository = new InMemoryBookRepository();
            _controller = new BooksController(_repository);
        }


        [Fact]
        public void GetAll_ReturnsListOfAllBooks_WhenBooksExist()
        {

            _repository.Add(new Book { Title = "Book 1", Author = "Author 1", Year = 2020 });
            _repository.Add(new Book { Title = "Book 2", Author = "Author 2", Year = 2021 });

            var result = _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var books = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
            Assert.Equal(2, books.Count());
        }

        [Fact]
        public void GetAll_ReturnsEmptyList_WhenNoBooksExist()
        {
            var result = _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var books = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
            Assert.Empty(books);
        }


        [Fact]
        public void GetById_ReturnsBook_WhenIdExists()
        {

            var book = new Book { Title = "Test Book", Author = "Test Author", Year = 2022 };
            _repository.Add(book);

            var result = _controller.GetById(book.Id);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBook = Assert.IsType<Book>(okResult.Value);
            Assert.Equal(book.Id, returnedBook.Id);
            Assert.Equal("Test Book", returnedBook.Title);
        }

        [Fact]
        public void GetById_ReturnsNotFound_WhenIdDoesNotExist()
        {
            var result = _controller.GetById(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }


        [Fact]
        public void Add_SuccessfullyAddsNewBook()
        {

            var newBook = new Book { Title = "New Book", Author = "New Author", Year = 2023 };

            var result = _controller.Add(newBook);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedBook = Assert.IsType<Book>(createdResult.Value);
            Assert.Equal("New Book", returnedBook.Title);
            Assert.Single(_repository.GetAll());
        }

        [Fact]
        public void Add_ReturnsBadRequest_WhenTitleIsEmpty()
        {
            var invalidBook = new Book { Title = "", Author = "Author", Year = 2023 };

            var result = _controller.Add(invalidBook);

            Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Empty(_repository.GetAll());
        }


        [Fact]
        public void Update_SuccessfullyUpdatesExistingBook()
        {
            var book = new Book { Title = "Old Title", Author = "Author", Year = 2020 };
            _repository.Add(book);

            var updatedBook = new Book { Title = "Updated Title", Author = "Author", Year = 2020 };

            var result = _controller.Update(book.Id, updatedBook);

            Assert.IsType<NoContentResult>(result);
            var checkBook = _repository.GetById(book.Id);
            Assert.Equal("Updated Title", checkBook?.Title);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenBookDoesNotExist()
        {

            var updatedBook = new Book { Title = "Some Title", Author = "Author", Year = 2020 };

            var result = _controller.Update(999, updatedBook);

            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public void Delete_SuccessfullyDeletesExistingBook()
        {

            var book = new Book { Title = "Book to delete", Author = "Author", Year = 2020 };
            _repository.Add(book);


            var result = _controller.Delete(book.Id);


            Assert.IsType<NoContentResult>(result);
            Assert.Empty(_repository.GetAll());
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenBookDoesNotExist()
        {

            var result = _controller.Delete(999);


            Assert.IsType<NotFoundResult>(result);
        }
    }
}