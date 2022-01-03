using Sabio.Models.Domain;
using Sabio.Models.Requests.Addresses;
using System.Collections.Generic;

namespace Sabio.Services
{
    public interface IAddressesService
    {
        int Add(AddressAddRequest model, int userId);
        Address Get(int Id);
        List<Address> GetTop();
        void Update(AddressUpdateRequest model, int userId);
        void Delete(int Id);
        
    }
}