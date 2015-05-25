using System.Collections.Generic;
using System.Web.Http;
using QFLibrary.Code.Models;

namespace QFLibrary.Code.Controllers
{
    public class BooksController : ApiController
    {
        public IEnumerable<Book> Get()
        {
            return new[] { new Book() };
        }
    }
}