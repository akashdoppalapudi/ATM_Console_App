using ATM.Models.Enums;

namespace ATM.API.Models
{
    public class EmployeeCreateDTO
    {
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public string Username { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public string Password { get; set; }
    }
}
