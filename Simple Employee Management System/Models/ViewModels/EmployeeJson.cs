namespace Simple_Employee_Management_System.Models.ViewModels
{
    public class EmployeeJson
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DepartmentJson Department { get; set; }
        public decimal Salary { get; set; }
        public AddressJson Address { get; set; }
        public List<DependentJson> Dependents { get; set; }
        public List<ProjectJson> Projects { get; set; }
    }

    public class DepartmentJson
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }

    public class AddressJson
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }

    public class DependentJson
    {
        public int DependentId { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class ProjectJson
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Role { get; set; }
    }
}
