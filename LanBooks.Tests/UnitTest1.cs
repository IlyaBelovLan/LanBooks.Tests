namespace LanBooks.Tests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using SwaggerClient;

    /// <summary>
    /// �������� ����� ��� Web API LanBooks.
    /// </summary>
    public class Tests
    {
        /// <summary>
        /// ������ ��� ������ � Web API.
        /// </summary>
        private Client _client;

        /// <summary>
        /// �������������� <see cref="Client"/>.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _client = new Client("https://localhost:44376/");
        }

        /// <summary>
        /// ��������� ��������� ����� �� id.
        /// </summary>
        [Test]
        public void GetBookByIdTest()
        {
            //Arrange
            string id = "viixY3oB-iHo-N2gru-i";

            string testJson =
                "{\"id\":\"viixY3oB-iHo-N2gru-i\",\"title\":\"������� �� ��������\",\"author\":\"������ ������\",\"publicationDate\":\"2018-01-01T00:00:00\",\"isbn\":\"978-2-1234-5680-3\",\"lccn\":\"\",\"language\":\"ru\",\"publishingHouse\":\"���\",\"tags\":[\"������\",\"�������\"]}";

            Book testBook = Book.FromJson(testJson);

            //Act
            var res = _client.GetBookByIdAsync(id);

            var book = res.Result;

            //Assert
            Assert.AreEqual(testBook, book, "��������� ����� �� id");
            Console.WriteLine("��������� �����: " + testBook.ToJson());
            Console.WriteLine("���������� �����: " + book.ToJson());
        }

        /// <summary>
        /// ��������� ��������� ������ ����
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
        /// ��������� ������ � ��������.
        /// </summary>
        [Test]
        public void GetBooksByFilters()
        {
            int number = 1;
            int size = 100;

            //Arrange
            //������ ������ � ������� � �������� ��������
            GetBooksByFiltersQuery query = new GetBooksByFiltersQuery
            {
                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "������ ������ � ������� � �������� ��������: ");

            //------

            //Arrange
            //������ � ��������� ��������
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    Title = "�� ��������"
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "������ � ��������� ��������: ");

            //------

            //Arrange
            //������ � ��������� ������
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    Author = "������"
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "������ � ��������� ������: ");

            //-------

            //Arrange
            //������ � ��������� ��������� ���� ����������
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
            TestQuery(query, "������ � ��������� ������: ");

            //-------

            //Arrange
            //������ � ��������� ������������
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    PublishingHouse = "���"
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "������ � ��������� ������������: ");

            //-------

            //Arrange
            //������ � ��������� �����
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
            TestQuery(query, "������ � ��������� �����: ");

            //------

            //Arrange
            //������ �� ������� isbn
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
            TestQuery(query, "������ �� ������� isbn: ");

            //------

            //Arrange
            //������ � ������
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    Tags = new List<string>() { "C++", "����" }
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "������ � ������: ");

            //------

            //Arrange
            //������ � ����������� �� ��������
            query = new GetBooksByFiltersQuery
            {
                PageNumber = number,
                PageSize = size,
                SortBy = SortByFields.Title
            };

            //Act
            TestQuery(query, "������ � ����������� �� ��������: ");

            //------

            //Arrange
            //������ � ����������� �� ������
            query = new GetBooksByFiltersQuery
            {
                PageNumber = number,
                PageSize = size,
                SortBy = SortByFields.Author
            };

            //Act
            TestQuery(query, "������ � ����������� �� ������: ");

            //------

            //Arrange
            //������ � ����������� �� ���� ����������
            query = new GetBooksByFiltersQuery
            {
                PageNumber = number,
                PageSize = size,
                SortBy = SortByFields.PublicationDate
            };

            //Act
            TestQuery(query, "������ � ����������� �� ���� ����������: ");

            //------

            //Arrange
            //������ � ��������� � �������
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    Title = "�� ��������",
                    Author = "�������"
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "������ ��������� � �������: ");

            //Arrange
            //������ � ���������, ������� � �������� ����������
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    Title = "�� ��������",
                    Author = "������",
                    PublicationDateGte = new DateTime(2011, 01, 01),
                    PublicationDateLte = new DateTime(2018, 12, 31)
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "������ � ���������, ������� � �������� ����������: ");

            //------

            //Arrange
            //������ � ���������, �������, �������� ���������� � �������������
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    Title = "�� ��������",
                    Author = "������",
                    PublicationDateGte = new DateTime(2011, 01, 01),
                    PublicationDateLte = new DateTime(2018, 12, 31),
                    PublishingHouse = "���"
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "������ � ���������, �������, �������� ���������� � �������������: ");

            //------

            //Arrange
            //������ � ��������� � ������� ISBN
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    Title = "�� ��������",
                    IsbnList = new List<string>() { "978-5-699-12014-7", "978-5-699-44894-4" }
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "������ � ��������� � ������� ISBN: ");

            //------

            //Arrange
            //������ � ��������� � ������� �����
            query = new GetBooksByFiltersQuery
            {
                SearchFilters = new BooksSearchFilters
                {
                    Title = "�� ��������",
                    Tags = new List<string>() { "C++" }
                },

                PageNumber = number,
                PageSize = size
            };

            //Act
            TestQuery(query, "������ � ��������� � ������� �����: ");

            //------

            //Arrange
            //������ � ����������� �� ����������� �� ���� ����������
            query = new GetBooksByFiltersQuery
            {
                PageNumber = number,
                PageSize = size,
                SortBy = SortByFields.PublicationDate,
                Order = SortOrder.Ascending
            };

            //Act
            TestQuery(query, "������ � ����������� �� ����������� �� ���� ����������: ");

            //------

            //Arrange
            //������ � ����������� �� �������� �� ���� ����������
            query = new GetBooksByFiltersQuery
            {
                PageNumber = number,
                PageSize = size,
                SortBy = SortByFields.PublicationDate,
                Order = SortOrder.Descending
            };

            //Act
            TestQuery(query, "������ � ����������� �� �������� �� ���� ����������: ");
        }

        /// <summary>
        /// ���������� ������ � ��������.
        /// </summary>
        /// <param name="query">������ � ��������.</param>
        /// <param name="testTitle">��������� �����.</param>
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
        /// ������� ������ ����.
        /// </summary>
        /// <param name="books">������ ����.</param>
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