using System.ComponentModel.DataAnnotations;

namespace EmployeeMgtApi.Models
{
    public class AddEmployeeDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Position is required")]
        public string Position { get; set; }
    }
}
