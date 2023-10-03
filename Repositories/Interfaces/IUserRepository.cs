using Web_App.Models;

namespace Web_App.Repositories.Interfaces
{
    public interface IUserRepository
    {
        bool UniqueUser(string username);
        User Register(string username, string password);
        User Authenticate(string username, string password);
    }
}
