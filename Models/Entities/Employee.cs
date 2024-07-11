namespace EmployeeMgtApi.Models.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }  // New column
        public DateTime StatusChangeDate { get; set; }  // New column
    }

}
