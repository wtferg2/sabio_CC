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
    public class FriendsService : IFriendsService
    {
        IDataProvider _data = null;

        public FriendsService(IDataProvider data)
        {
            _data = data;
        }

        public void Update(FriendsUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Friends_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", model.Id);
                col.AddWithValue("@Title", model.Title);
                col.AddWithValue("Bio", model.Bio);
                col.AddWithValue("@Summary", model.Summary);
                col.AddWithValue("@Headline", model.Headline);
                col.AddWithValue("@Slug", model.Slug);
                col.AddWithValue("@StatusId", model.StatusId);
                col.AddWithValue("@PrimaryImage", model.PrimaryImage);
                col.AddWithValue("@UserId", model.UserId);


            }, returnParameters: null);
        }
        public int Add(FriendsAddRequest model, int userId)
        {

            int id = 0;
            string procName = "[dbo].[Friends_Insert]";
            DataTable myParamValue = MapSkillsToTable(model.Skills);
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Title", model.Title);
                col.AddWithValue("@Bio", model.Bio);
                col.AddWithValue("@Summary", model.Summary);
                col.AddWithValue("@Headline", model.Headline);
                col.AddWithValue("@Slug", model.Slug);
                col.AddWithValue("@StatusId", model.StatusId);
                col.AddWithValue("@PrimaryImage", model.PrimaryImage);
                col.AddWithValue("@UserId", model.UserId);
                col.AddWithValue("@SkillsTable", myParamValue);

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

        public Friend Get(int Id)
        {

            string procName = "[dbo].[Friends_SelectById_v2]";

            Friend friend = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {

                paramCollection.AddWithValue("@Id", Id);


            }, delegate (IDataReader reader, short set) //single record mapper
            {
                // oneShape > secondShape
                // reader from DB tabular data stream >>> Address


                friend = FriendMapperJoins(reader);

            }
             );

            return friend;
        }

        public void Delete(int Id)
        {

            string procName = "[dbo].[Friends_Delete]";

            Friend friend = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {

                paramCollection.AddWithValue("@Id", Id);


            }, delegate (IDataReader reader, short set) //single record mapper
            {
                // oneShape > secondShape
                // reader from DB tabular data stream >>> Address


                friend = FriendMapper(reader);

            }
             );

        }

        public List<Friend> GetAll()
        {
            List<Friend> list = null;

            string procName = "[dbo].[Friends_SelectAll_v2]";


            _data.ExecuteCmd(procName, inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set) //single record mapper
            {
                // oneShape > secondShape
                // reader from DB tabular data stream >>> Address

                Friend friend = FriendMapperJoins(reader);

                if (list == null)
                {
                    list = new List<Friend>();
                }
                list.Add(friend);
            });

            return list;
        }
        public Paged<Friend> Pagination(int pageIndex, int pageSize)
        {
            Paged<Friend> pagedList = null;
            List<Friend> list = null;
            string procName = "[dbo].[Friends_SelectPaginated_v2]";
            int totalCount = 0;

            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            }, (reader, recordSetIndex) =>
             { Friend friend = FriendMapperJoins(reader);
                 totalCount = reader.GetSafeInt32(6);
                 if (list == null)
                 {
                     list = new List<Friend>();
                 }
                 list.Add(friend);
             });
            if (list != null)
            {
                pagedList = new Paged<Friend>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;

        }
       

        private DataTable MapSkillsToTable(List<string> skillsToMap)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            
            foreach (string singleSkill in skillsToMap)
            {
                DataRow dr = dt.NewRow();
                
                dr.SetField(0, singleSkill);
                dt.Rows.Add(dr);
            }
            return dt;
        }
        private static Friend FriendMapper(IDataReader reader)
        {

            Friend friend = new Friend();
           
            int startingIndex = 0;
            friend.Id = reader.GetSafeInt32(startingIndex++);
            friend.DateModified = reader.GetSafeDateTime(startingIndex++);
            friend.DateAdded = reader.GetSafeDateTime(startingIndex++);
            friend.Title = reader.GetSafeString(startingIndex++);
            friend.Bio = reader.GetSafeString(startingIndex++);
            friend.Summary = reader.GetSafeString(startingIndex++);
            friend.Headline = reader.GetSafeString(startingIndex++);
            friend.Slug = reader.GetSafeString(startingIndex++);
            friend.StatusId = reader.GetSafeString(startingIndex++);
            friend.PrimaryImage = reader.GetSafeString(startingIndex++);
            friend.UserId = reader.GetSafeString(startingIndex++);
           
            return friend;
        }

        private static Friend FriendMapperJoins(IDataReader reader)
        {

            Friend friend = new Friend();
            
            int startingIndex = 0;
            friend.Id = reader.GetSafeInt32(startingIndex++);
            friend.DateModified = reader.GetSafeDateTime(startingIndex++);
            friend.DateAdded = reader.GetSafeDateTime(startingIndex++);
            friend.Title = reader.GetSafeString(startingIndex++);
            friend.Bio = reader.GetSafeString(startingIndex++);
            friend.Summary = reader.GetSafeString(startingIndex++);
            friend.Headline = reader.GetSafeString(startingIndex++);
            friend.Slug = reader.GetSafeString(startingIndex++);
            friend.StatusId = reader.GetSafeString(startingIndex++);
            friend.PrimaryImage = reader.GetSafeString(startingIndex++);
            friend.UserId = reader.GetSafeString(startingIndex++);
            friend.Skills = reader.DeserializeObject<List<Skills>>(startingIndex++);
            return friend;
        }
    }
}
