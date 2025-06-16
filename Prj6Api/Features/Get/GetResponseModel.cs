namespace Prj6Api.Features.Get
{
    public class GetResponseModel : ResponseModel
    {
        public List<StudentList> Data { get; set; }
    }

    public class StudentList
    {
        public string? RollNo { get; set; }
        public string? Name { get; set; }

        public int Age { get; set; }

    }
}
