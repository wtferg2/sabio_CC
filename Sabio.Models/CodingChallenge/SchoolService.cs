using Sabio.Data;
using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.CodingChallenge
{
    public class SchoolService : ISchoolService
    {


        IDataProvider _data = null;

        public SchoolService(IDataProvider data)
        {
            _data = data;
        }

        public void Update(CourseUpdateRequest model)
        {
            string procName = "[dbo].[Course_CC_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Name", model.Name);
                col.AddWithValue("@Description", model.Description);
                col.AddWithValue("@SeasonTermId", model.SeasonTermId);
                col.AddWithValue("@TeacherId", model.TeacherId);

            }, returnParameters: null);
        }
        public int Add(CourseAddRequest model)
        {

            int id = 0;
            string procName = "[dbo].[Courses_CC_Insert]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {

               
                col.AddWithValue("@Name", model.Name);
                col.AddWithValue("@Description", model.Description);
                col.AddWithValue("@SeasonTermId", model.SeasonTermId);
                col.AddWithValue("@TeacherId", model.TeacherId);
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

        public Course Get(int Id)
        {

            string procName = "[dbo].[Select_ByCourseId]";

            Course course = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {

                paramCollection.AddWithValue("@Id", Id);


            }, delegate (IDataReader reader, short set) //single record mapper
            {
               course = CourseMapper(reader);

            }
             );

            return course;
        }

        public void Delete(int Id)
        {

            string procName = "[dbo].[Student_DeleteById]";

            Course course = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {

                paramCollection.AddWithValue("@Id", Id);


            }, delegate (IDataReader reader, short set) //single record mapper
            {
                // oneShape > secondShape
                // reader from DB tabular data stream >>> Address


                course = CourseMapper(reader);

            }
             );

        }

        public Paged<Course> Pagination(int pageIndex, int pageSize)
        {
            Paged<Course> pagedList = null;
            List<Course> list = null;
            string procName = "[dbo].[Courses_SelectPaginated_v2]";
            int totalCount = 0;

            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            }, (reader, recordSetIndex) =>
            {
                Course course = CourseMapper(reader);
                totalCount = reader.GetSafeInt32(6);
                if (list == null)
                {
                    list = new List<Course>();
                }
                list.Add(course);
            });
            if (list != null)
            {
                pagedList = new Paged<Course>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;

        }

        private static Course CourseMapper(IDataReader reader)
        {

            Course course = new Course();

            int startingIndex = 0;
            course.Id = reader.GetSafeInt32(startingIndex++);
            course.Name = reader.GetSafeString(startingIndex++);
            course.Description = reader.GetSafeString(startingIndex++);
            course.SeasonTerm = reader.GetSafeString(startingIndex++);
            course.Teacher = reader.GetSafeString(startingIndex++);
            course.Students = reader.DeserializeObject<List<Student>>(startingIndex++);

            return course;
        }



    }
}
    

