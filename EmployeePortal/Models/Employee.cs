using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeePortal.Models
{
    public class Employee
    {
        // This class represents an employee with all their relevant details like name, email, department, designation, hire date, birth date, employment type, gender, and salary.
        // It uses validation to ensure data integrity.
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Hire Date is required")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(6, ErrorMessage = "Gender should be Male, Female, or Other")]
        public string Gender { get; set; } = null!;

        [Required(ErrorMessage = "Salary is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }


        [Required(ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        [Required(ErrorMessage = "Designation is required")]
        public int DesignationId { get; set; }
        public Designation Designation { get; set; } = null!;

        [Required(ErrorMessage = "Employee Type is required")]
        public int EmployeeTypeId { get; set; }
        public EmployeeType EmployeeType { get; set; } = null!;
    }
}