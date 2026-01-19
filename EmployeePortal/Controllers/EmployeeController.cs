using EmployeePortal.Models;
using EmployeePortal.Services;
using EmployeePortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmployeePortal.Controllers
{
    public class EmployeeController : Controller
    {
        // This controller manages HTTP requests for employee-related actions.
        // It handles showing employee lists, creating, updating, and deleting employees, and provides JSON data for dynamic dropdowns.
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }



        /*----------------------------------------------------------------------------*/
        [HttpGet]
        public async Task<IActionResult> ListAjax(
            string? searchTerm,
            int? selectedDepartmentId,
            int? selectedEmployeeTypeId,
            int pageNumber = 1,
            int pageSize = 5)
        {
            // نفس اللوجيك بالظبط
            var (employees, totalCount) =
                await _employeeService.GetEmployees(
                    searchTerm,
                    selectedDepartmentId,
                    selectedEmployeeTypeId,
                    pageNumber,
                    pageSize);

            var vm = new EmployeeListVM
            {
                Employees = employees,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Total = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };

            // IMPORTANT: بنرجّع Partial View مش View
            return PartialView("_EmployeeTable", vm);
        }


        /*----------------------------------------------------------------------------*/

        [HttpGet]
        public async Task<IActionResult> List(
            string? searchTerm,
            int? SelectedDepartmentId,
            int? SelectedEmployeeTypeId,
            int pageNumber = 1,
            int pageSize = 5)
        {
            var (employees, totalCount) = await _employeeService.GetEmployees(searchTerm, SelectedDepartmentId, SelectedEmployeeTypeId, pageNumber, pageSize);

            var viewModel = new EmployeeListVM
            {
                Employees = employees,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Total = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                SearchTerm = searchTerm,
                SelectedDepartmentId = SelectedDepartmentId,
                SelectedEmployeeTypeId = SelectedEmployeeTypeId,
                Departments = await _employeeService.GetDepartmentsAsync(),
                EmployeeTypes = await _employeeService.GetEmployeeTypesAsync()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new EmployeeCreateUpdateVM
            {
                Departments = await _employeeService.GetDepartmentsAsync(),
                EmployeeTypes = await _employeeService.GetEmployeeTypesAsync(),
                Designations = new List<Designation>()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateUpdateVM vm)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    FullName = vm.FullName,
                    Email = vm.Email,
                    DepartmentId = vm.DepartmentId,
                    DesignationId = vm.DesignationId,
                    HireDate = vm.HireDate,
                    DateOfBirth = vm.DateOfBirth,
                    EmployeeTypeId = vm.EmployeeTypeId,
                    Gender = vm.Gender,
                    Salary = vm.Salary
                };

                await _employeeService.CreateEmployeeAsync(employee);
                return RedirectToAction("Success", new { id = employee.Id });
            }

            vm.Departments = await _employeeService.GetDepartmentsAsync();
            vm.EmployeeTypes = await _employeeService.GetEmployeeTypesAsync();
            vm.Designations = vm.DepartmentId != 0 ? await _employeeService.GetDesignationsByDepartmentAsync(vm.DepartmentId) : new List<Designation>();

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();

            var vm = new EmployeeCreateUpdateVM
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                DesignationId = employee.DesignationId,
                HireDate = employee.HireDate,
                DateOfBirth = employee.DateOfBirth,
                EmployeeTypeId = employee.EmployeeTypeId,
                Gender = employee.Gender,
                Salary = employee.Salary,
                Departments = await _employeeService.GetDepartmentsAsync(),
                EmployeeTypes = await _employeeService.GetEmployeeTypesAsync(),
                Designations = await _employeeService.GetDesignationsByDepartmentAsync(employee.DepartmentId)
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(EmployeeCreateUpdateVM vm)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    Id = vm.Id!.Value,
                    FullName = vm.FullName,
                    Email = vm.Email,
                    DepartmentId = vm.DepartmentId,
                    DesignationId = vm.DesignationId,
                    HireDate = vm.HireDate,
                    DateOfBirth = vm.DateOfBirth,
                    EmployeeTypeId = vm.EmployeeTypeId,
                    Gender = vm.Gender,
                    Salary = vm.Salary
                };

                await _employeeService.UpdateEmployeeAsync(employee);
                TempData["Message"] = $"Employee with ID {employee.Id} and Name {employee.FullName} has been updated.";
                return RedirectToAction("List");
            }

            vm.Departments = await _employeeService.GetDepartmentsAsync();
            vm.EmployeeTypes = await _employeeService.GetEmployeeTypesAsync();
            vm.Designations = vm.DepartmentId != 0 ? await _employeeService.GetDesignationsByDepartmentAsync(vm.DepartmentId) : new List<Designation>();

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();

            await _employeeService.DeleteEmployeeAsync(id);
            TempData["Message"] = $"Employee with ID {id} and Name {employee.FullName} has been deleted.";
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<JsonResult> GetDesignations(int departmentId)
        {
            var designations = await _employeeService.GetDesignationsByDepartmentAsync(departmentId);
            var result = designations.Select(d => new { id = d.Id, name = d.Name }).ToList();
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Success(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}