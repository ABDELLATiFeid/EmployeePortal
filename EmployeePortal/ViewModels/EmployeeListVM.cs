using EmployeePortal.Models;
namespace EmployeePortal.ViewModels
{
    public class EmployeeListVM
    {
        // This view model is used to pass employee data to the list view.
        // It includes the list of employees, paging information, search terms, selected filters,
        // lists of departments and employee types for dropdowns.
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public string? SearchTerm { get; set; }
        public int? SelectedDepartmentId { get; set; }
        public int? SelectedEmployeeTypeId { get; set; }
        
        public List<Employee>? Employees { get; set; }
        public List<Department>? Departments { get; set; }
        public List<EmployeeType>? EmployeeTypes { get; set; }
    }
}