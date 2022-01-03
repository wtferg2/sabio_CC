using System.Collections.Generic;

namespace Sabio.Models
{
    public interface IFriendsService
    {
        int Add(FriendsAddRequest model, int userId);
        void Delete(int Id);
        Friend Get(int Id);
        List<Friend> GetAll();
        void Update(FriendsUpdateRequest model, int userId);
        Paged<Friend> Pagination(int pageIndex, int pageSize);
    }
}