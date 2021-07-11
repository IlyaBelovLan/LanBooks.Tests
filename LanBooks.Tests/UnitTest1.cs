namespace LanBooks.Tests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using SwaggerClient;

    /// <summary>
    /// Содержит тесты для Web API LanBooks.
    /// </summary>
    public class Tests
    {
        /// <summary>
        /// Клиент для работы с Web API.
        /// </summary>
        private Client _client;

        /// <summary>
        /// Инициализирует <see cref="Client"/>.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _client = new Client("https://localhost:44376/");
        }

        /// <summary>
        /// Тестирует получение книги по id.
        /// </summary>
        [Test]
        public void GetBookByIdTest()
        {
            //Arrange
            string id = "viixY3oB-iHo-N2gru-i";

            string testJson =
                "{\"id\":\"viixY3oB-iHo-N2gru-i\",\"title\":\"Хинкали на примерах\",\"author\":\"Добрый грузин\",\"publicationDate\":\"2018-01-01T00:00:00\",\"isbn\":\"978-2-1234-5680-3\",\"lccn\":\"\",\"language\":\"ru\",\"publishingHouse\":\"МИФ\",\"tags\":[\"Вкусно\",\"Полезно\"]}";

            Book testBook = Book.FromJson(testJson);

            //Act
            var res = _client.GetBookByIdAsync(id);

            var book = res.Result;

            //Assert
            Assert.AreEqual(testBook, book, "Получение книги по id");
            Console.WriteLine("Ожидаемая книга: " + testBook.ToJson());
            Console.WriteLine("Полученная книга: " + book.ToJson());
        }

        /// <summary>
        /// Тестирует получение списка книг
        /// </summary>
        [Test]
        public void GetBookListTest()
        {
            //Arrange
            GetBookListQuery query = new GetBookListQuery
            {
                PageNumber = 1,
                PageSize = 1000
            };

            //Act
            var resultTask = _client.GetBookListAsync(query);

            var result = (SwaggerResponse<List<Book>>)resultTask.Result;

            var books = result.Result;

            //Assert
            foreach (var book in books)
            {
                Console.WriteLine(book.ToJson());
            }
        }

        /// <summary>
        /// Тестирует запрос с фильтром.
        /// </summary>
        [Test]
        public void GetBooksByFilters()
        {
            int number = 1;
            int size = 100;

            //Arrange
            //Запрос только с номером и размером страницы
            GetBooksByFiltersQuery query = new GetBooksByFiltersQuery
            {
                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "Запрос только с номером и размером страницы: ");

            //------

            //Arrange
            //Запрос с указанием названия
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    Title = "на примерах"
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "Запрос с указанием названия: ");

            //------

            //Arrange
            //Запрос с указанием автора
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    Author = "грузин"
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "Запрос с указанием автора: ");

            //-------

            //Arrange
            //Запрос с указанием диапазона даты публикации
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    PublicationDateGte = new DateTime(2011, 01, 01),
                    PublicationDateLte = new DateTime(2011, 12, 31)
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "Запрос с указанием автора: ");

            //-------

            //Arrange
            //Запрос с указанием издательства
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    PublishingHouse = "МИФ"
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "Запрос с указанием издательства: ");

            //-------

            //Arrange
            //Запрос с указанием языка
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    Language = "en"
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "Запрос с указанием языка: ");

            //------

            //Arrange
            //Запрос со списком isbn
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    IsbnList = new List<string>() { "978-5-699-12014-7", "978-5-699-44894-4" }
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "Запрос со списком isbn: ");

            //------

            //Arrange
            //Запрос с тегами
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    Tags = new List<string>() { "C++", "Дзен" }
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "Запрос с тегами: ");

            //------

            //Arrange
            //Запрос с соритровкой по названию
            query = new GetBooksByFiltersQuery
            {
                PageNumber = number,
                PageSize = size,
                SortBy = SortByFields.Title
            };

            //Act
            TestQuery(query, "Запрос с соритровкой по названию: ");

            //------

            //Arrange
            //Запрос с соритровкой по автору
            query = new GetBooksByFiltersQuery
            {
                PageNumber = number,
                PageSize = size,
                SortBy = SortByFields.Author
            };

            //Act
            TestQuery(query, "Запрос с соритровкой по автору: ");

            //------

            //Arrange
            //Запрос с соритровкой по дате публикации
            query = new GetBooksByFiltersQuery
            {
                PageNumber = number,
                PageSize = size,
                SortBy = SortByFields.PublicationDate
            };

            //Act
            TestQuery(query, "Запрос с соритровкой по дате публикации: ");

            //------

            //Arrange
            //Запрос с названием и автором
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    Title = "на примерах",
                    Author = "Кольцов"
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "Запрос названием и автором: ");

            //Arrange
            //Запрос с названием, автором и периодом публикации
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    Title = "на примерах",
                    Author = "грузин",
                    PublicationDateGte = new DateTime(2011, 01, 01),
                    PublicationDateLte = new DateTime(2018, 12, 31)
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "Запрос с названием, автором и периодом публикации: ");

            //------

            //Arrange
            //Запрос с названием, автором, периодом публикации и издательством
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    Title = "на примерах",
                    Author = "грузин",
                    PublicationDateGte = new DateTime(2011, 01, 01),
                    PublicationDateLte = new DateTime(2018, 12, 31),
                    PublishingHouse = "МИФ"
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "Запрос с названием, автором, периодом публикации и издательством: ");

            //------

            //Arrange
            //Запрос с названием и списком ISBN
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    Title = "на примерах",
                    IsbnList = new List<string>() { "978-5-699-12014-7", "978-5-699-44894-4" }
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "Запрос с названием и списком ISBN: ");

            //------

            //Arrange
            //Запрос с названием и списком тегов
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    Title = "на примерах",
                    Tags = new List<string>() { "C++" }
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "Запрос с названием и списком тегов: ");

            //------

            //Arrange
            //Запрос с сортировкой по возрастанию по дате публикации
            query = new GetBooksByFiltersQuery
            {
                PageNumber = number,
                PageSize = size,
                SortBy = SortByFields.PublicationDate,
                Order = SortOrder.Ascending
            };

            //Act
            TestQuery(query, "Запрос с сортировкой по возрастанию по дате публикации: ");

            //------

            //Arrange
            //Запрос с сортировкой по убыванию по дате публикации
            query = new GetBooksByFiltersQuery
            {
                PageNumber = number,
                PageSize = size,
                SortBy = SortByFields.PublicationDate,
                Order = SortOrder.Descending
            };

            //Act
            TestQuery(query, "Запрос с сортировкой по убыванию по дате публикации: ");
        }

        /// <summary>
        /// Отправляет запрос с фильтром.
        /// </summary>
        /// <param name="query">Запрос с фильтром.</param>
        /// <param name="testTitle">Заголовок теста.</param>
        public void TestQuery(GetBooksByFiltersQuery query, string testTitle)
        {
            //Act
            var resultTask = _client.GetBooksByFiltersAsync(query);

            var result = (SwaggerResponse<List<Book>>)resultTask.Result;

            var books = result.Result;

            Console.WriteLine(testTitle);

            //Assert
            PrintBookList(books);
        }
        
        /// <summary>
        /// Выводят список книг.
        /// </summary>
        /// <param name="books">Список книг.</param>
        public void PrintBookList(List<Book> books)
        {

            foreach (var book in books)
            {
                Console.WriteLine(book.ToJson());
            }

            Console.WriteLine();
        }
    }
}