using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Simple_Employee_Management_System.Models
{
    public class Project
    {

        [Key]
        public int ProjectId { get; set; }

        [Required, MaxLength(100)]
        public string ProjectName { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required, MaxLength(50)]
        public string Role { get; set; }

        public int EmployeeId { get; set; }


        [ValidateNever]
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}
