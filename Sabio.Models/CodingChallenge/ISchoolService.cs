namespace Sabio.Models.CodingChallenge
{
    public interface ISchoolService
    {
        int Add(CourseAddRequest model);
        void Delete(int Id);
        Course Get(int Id);
        Paged<Course> Pagination(int pageIndex, int pageSize);
        void Update(CourseUpdateRequest model);
    }
}