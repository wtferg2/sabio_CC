using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data.Providers;
using Sabio.Models.Domain;
using Sabio.Data;
using Sabio.Models.Requests.Addresses;

namespace Sabio.Services
{
    public class AddressesService : IAddressesService
    {
        IDataProvider _data = null;

        public AddressesService(IDataProvider data)
        {
            _data = data;
        }

        public void Delete(int Id)
        {

            string procName = "[dbo].[Sabio_Addresses_DeleteById]";

            Address address = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {

                paramCollection.AddWithValue("@Id", Id);


            }, delegate (IDataReader reader, short set) //single record mapper
            {
                // oneShape > secondShape
                // reader from DB tabular data stream >>> Address


                address = NewMethod(reader);

            }
             );
        }

        public void Update(AddressUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Sabio_Addresses_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", model.Id);
                col.AddWithValue("@LineOne", model.LineOne);
                col.AddWithValue("@SuiteNumber", model.SuiteNumber);
                col.AddWithValue("@City", model.City);
                col.AddWithValue("@State", model.State);
                col.AddWithValue("@PostalCode", model.PostalCode);
                col.AddWithValue("@IsActive", model.IsActive);
                col.AddWithValue("@Lat", model.Lat);
                col.AddWithValue("@Long", model.Long);

            }, returnParameters: null);
        }
        public int Add(AddressAddRequest model, int userId)
        {
            int id = 0;
            string procName = "[dbo].[Sabio_Addresses_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@LineOne", model.LineOne);
                col.AddWithValue("@SuiteNumber", model.SuiteNumber);
                col.AddWithValue("@City", model.City);
                col.AddWithValue("@State", model.State);
                col.AddWithValue("@PostalCode", model.PostalCode);
                col.AddWithValue("@IsActive", model.IsActive);
                col.AddWithValue("@Lat", model.Lat);
                col.AddWithValue("@Long", model.Long);

                // and one output
                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);
            },
            returnParameters: delegate (SqlParameterCollection returnCol)
            {
                object oId = returnCol["@Id"].Value;

                Int32.TryParse(oId.ToString(), out id);

            });
            return id;
        }

        public Address Get(int Id)
        {

            string procName = "[dbo].[Sabio_Addresses_SelectById]";

            Address address = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {

                paramCollection.AddWithValue("@Id", Id);


            }, delegate (IDataReader reader, short set) //single record mapper
            {
                // oneShape > secondShape
                // reader from DB tabular data stream >>> Address


                address = NewMethod(reader);

            }
             );

            return address;
        }

        public List<Address> GetTop()
        {
            List<Address> list = null;

            string procName = "[dbo].[Sabio_Addresses_SelectRandom50]";


            _data.ExecuteCmd(procName, inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set) //single record mapper
            {
                // oneShape > secondShape
                // reader from DB tabular data stream >>> Address

                Address address = NewMethod(reader);

                if (list == null)
                {
                    list = new List<Address>();
                }
                list.Add(address);
            });

            return list;
        }

        private static Address NewMethod(IDataReader reader)
        {
            Address address = new Address();
            int startingIndex = 0;
            address.Id = reader.GetSafeInt32(startingIndex++);
            address.LineOne = reader.GetSafeString(startingIndex++);
            address.SuiteNumber = reader.GetSafeInt32(startingIndex++);
            address.City = reader.GetSafeString(startingIndex++);
            address.State = reader.GetSafeString(startingIndex++);
            address.PostalCode = reader.GetSafeString(startingIndex++);
            address.IsActive = reader.GetBoolean(startingIndex++);
            address.Lat = reader.GetSafeDouble(startingIndex++);
            address.Long = reader.GetSafeDouble(startingIndex++);
            return address;
        }
    }


}
