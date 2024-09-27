using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Simple_Employee_Management_System.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [Required, MaxLength(100)]
        public string Street { get; set; }

        [Required, MaxLength(50)]
        public string City { get; set; }

        [Required, MaxLength(50)]
        public string State { get; set; }

        [Required, MaxLength(10)]
        public string PostalCode { get; set; }

        public int EmployeeId { get; set; }

        [ValidateNever]
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}
