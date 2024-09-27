using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Simple_Employee_Management_System.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime DateOfJoining { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [ValidateNever]
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal Salary { get; set; }


        [Required]
        public Address Address { get; set; }

        public ICollection<Dependent> Dependents { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
