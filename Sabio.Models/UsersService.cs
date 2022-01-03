using Sabio.Data;
using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models
{
    public class UsersService : IUsersService
    {
        IDataProvider _data = null;

        public UsersService(IDataProvider data)
        {
            _data = data;
        }

        public void Update(UsersUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Users_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", model.Id);
                col.AddWithValue("@UserId", model.UserId);
                col.AddWithValue("@FirstName", model.FirstName);
                col.AddWithValue("@LastName", model.LastName);
                col.AddWithValue("@Email", model.Email);
                col.AddWithValue("@Password", model.Password);
                col.AddWithValue("@AvatarUrl", model.AvatarUrl);
                col.AddWithValue("@TenantId", model.TenantId);


            }, returnParameters: null);
        }
        public int Add(UsersAddRequest model, int userId)
        {
            int id = 0;
            string procName = "[dbo].[Users_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@UserId", model.UserId);
                col.AddWithValue("@FirstName", model.FirstName);
                col.AddWithValue("@LastName", model.LastName);
                col.AddWithValue("@Email", model.Email);
                col.AddWithValue("@Password", model.Password);
                col.AddWithValue("@AvatarUrl", model.AvatarUrl);
                col.AddWithValue("@TenantId", model.TenantId);


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

        public User Get(int Id)
        {

            string procName = "[dbo].[Users_SelectById]";

            User user = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {

                paramCollection.AddWithValue("@Id", Id);


            }, delegate (IDataReader reader, short set) //single record mapper
            {
                // oneShape > secondShape
                // reader from DB tabular data stream >>> Address


                user = NewMethod(reader);

            }
             );

            return user;
        }

        public void Delete(int Id)
        {

            string procName = "[dbo].[Users_Delete]";

            User user = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {

                paramCollection.AddWithValue("@Id", Id);


            }, delegate (IDataReader reader, short set) //single record mapper
            {
                // oneShape > secondShape
                // reader from DB tabular data stream >>> Address


                user = NewMethod(reader);

            }
             );

        }

        public List<User> GetAll()
        {
            List<User> list = null;

            string procName = "[dbo].[Users_SelectAll]";


            _data.ExecuteCmd(procName, inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set) //single record mapper
            {
                // oneShape > secondShape
                // reader from DB tabular data stream >>> Address

                User user = NewMethod(reader);

                if (list == null)
                {
                    list = new List<User>();
                }
                list.Add(user);
            });

            return list;
        }

        private static User NewMethod(IDataReader reader)
        {
            User user = new User();
            int startingIndex = 0;
            user.Id = reader.GetSafeInt32(startingIndex++);
            user.DateModified = reader.GetSafeDateTime(startingIndex++);
            user.DateAdded = reader.GetSafeDateTime(startingIndex++);
            user.UserId = reader.GetSafeString(startingIndex++);
            user.FirstName = reader.GetSafeString(startingIndex++);
            user.LastName = reader.GetSafeString(startingIndex++);
            user.Email = reader.GetSafeString(startingIndex++);
            user.Password = reader.GetSafeString(startingIndex++);
            user.AvatarUrl = reader.GetSafeString(startingIndex++);
            user.TenantId = reader.GetSafeString(startingIndex++);

            return user;
        }

    }
}
