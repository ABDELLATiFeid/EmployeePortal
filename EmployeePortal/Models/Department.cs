namespace EmployeePortal.Models
{
    public class Department
    {
        // This class defines the various departments within the organization, including IT, HR, Sales, and Administration.
        // It organizes employees and designations by department and includes an active status to manage department availability.
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
        public ICollection<Designation>? Designations { get; set; } = new List<Designation>();
        public List<Employee>? Employees { get; set; }
    }
}