using System.Web.Http;
using QFLibrary.Code.Models;

namespace QFLibrary.Code.Controllers
{
    public class UsersController : ApiController
    {
        public User Get(string id)
        {
            return new User();
        }
    }
}