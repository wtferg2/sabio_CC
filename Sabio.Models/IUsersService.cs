using System.Collections.Generic;

namespace Sabio.Models
{
    public interface IUsersService
    {
        int Add(UsersAddRequest model, int userId);
        void Delete(int Id);
        User Get(int Id);
        List<User> GetAll();
        void Update(UsersUpdateRequest model, int userId);
    }
}