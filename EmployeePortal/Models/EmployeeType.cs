namespace EmployeePortal.Models
{
    public class EmployeeType
    {
        // This class represents various types of employment, including Permanent, Temporary, Contract, and Intern.
        // It helps categorize employees based on their employment status and tracks whether each type is active.
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
        public List<Employee>? Employees { get; set; }
    }
}
