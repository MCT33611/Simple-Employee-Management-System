using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Simple_Employee_Management_System.Models
{
    public class Dependent
    {
        [Key]
        public int DependentId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string Relationship { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public int EmployeeId { get; set; }

        [ValidateNever]
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}
