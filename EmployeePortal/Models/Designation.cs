namespace EmployeePortal.Models
{
    public class Designation
    {
        // This class represents job titles or positions within departments, such as Software Developer or HR Manager.
        // Each designation belongs to a specific department and can be activated or deactivated.

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
