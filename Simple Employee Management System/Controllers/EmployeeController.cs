using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Simple_Employee_Management_System.Data;
using Simple_Employee_Management_System.Models;

namespace Simple_Employee_Management_System.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        /*        public async Task<IActionResult> Index()
                {
                    var employees = await _context.Employees
                        .Include(e => e.Department)
                        .ToListAsync();
                    return View(employees);
                }*/

        public async Task<IActionResult> Index(int? department, DateTime? startDate, DateTime? endDate, decimal? minSalary, decimal? maxSalary)
        {
            ViewBag.Departments = await _context.Departments.ToListAsync();

            var employees = _context.Employees
                .Include(e => e.Department)
                .AsQueryable();

            if (department.HasValue)
            {
                employees = employees.Where(e => e.DepartmentId == department.Value);
            }

            if (startDate.HasValue)
            {
                employees = employees.Where(e => e.DateOfJoining >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                employees = employees.Where(e => e.DateOfJoining <= endDate.Value);
            }

            if (minSalary.HasValue)
            {
                employees = employees.Where(e => e.Salary >= minSalary.Value);
            }

            if (maxSalary.HasValue)
            {
                employees = employees.Where(e => e.Salary <= maxSalary.Value);
            }

            return View(await employees.ToListAsync());
        }


        public IActionResult Create()
        {
            var departments = _context.Departments.ToList();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var departments = _context.Departments.ToList();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");
            return View(employee);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Address)
                .Include(e => e.Dependents)
                .Include(e => e.Projects)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            var departments = _context.Departments.ToList();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEmployee = await _context.Employees
                        .Include(e => e.Address)
                        .Include(e => e.Projects)
                        .Include(e => e.Dependents)
                        .FirstOrDefaultAsync(e => e.EmployeeId == id);

                    if (existingEmployee == null)
                    {
                        return NotFound();
                    }

                    _context.Entry(existingEmployee).CurrentValues.SetValues(employee);

                    if (employee.Address != null)
                    {
                        if (existingEmployee.Address == null)
                        {
                            existingEmployee.Address = new Address();
                        }
                        _context.Entry(existingEmployee.Address).CurrentValues.SetValues(employee.Address);
                    }

                    foreach (var project in employee.Projects)
                    {
                        var existingProject = existingEmployee.Projects.FirstOrDefault(p => p.ProjectId == project.ProjectId);
                        if (existingProject != null)
                        {
                            _context.Entry(existingProject).CurrentValues.SetValues(project);
                        }
                        else
                        {
                            existingEmployee.Projects.Add(project);
                        }
                    }

                    foreach (var existingProject in existingEmployee.Projects.ToList())
                    {
                        if (!employee.Projects.Any(p => p.ProjectId == existingProject.ProjectId))
                        {
                            _context.Projects.Remove(existingProject);
                        }
                    }

                    foreach (var dependent in employee.Dependents)
                    {
                        var existingDependent = existingEmployee.Dependents.FirstOrDefault(d => d.DependentId == dependent.DependentId);
                        if (existingDependent != null)
                        {
                            _context.Entry(existingDependent).CurrentValues.SetValues(dependent);
                        }
                        else
                        {
                            existingEmployee.Dependents.Add(dependent);
                        }
                    }

                    foreach (var existingDependent in existingEmployee.Dependents.ToList())
                    {
                        if (!employee.Dependents.Any(d => d.DependentId == existingDependent.DependentId))
                        {
                            _context.Dependents.Remove(existingDependent);
                        }
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            var departments = await _context.Departments.ToListAsync();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName", employee.DepartmentId);

            return RedirectToAction(nameof(Edit));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDependent(int dependentId, int employeeId)
        {
            var dependent = await _context.Dependents
                .FirstOrDefaultAsync(d => d.DependentId == dependentId && d.EmployeeId == employeeId);

            if (dependent == null)
            {
                return Json(new { success = false, message = "Dependent not found." });
            }

            _context.Dependents.Remove(dependent);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Dependent deleted successfully." });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProject(int projectId, int employeeId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == projectId && p.EmployeeId == employeeId);

            if (project == null)
            {
                return Json(new { success = false, message = "Project not found." });
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Project deleted successfully." });
        }


        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }

        private bool ProjectExists(int ProjectId,int EmployeeId)
        {
            return _context.Projects.Any(p => p.EmployeeId == EmployeeId && p.ProjectId == ProjectId);
        }

        private bool DepententExists(int DepententId, int EmployeeId)
        {
            return _context.Dependents.Any(d => d.EmployeeId == EmployeeId && d.DependentId == DepententId);
        }
    }
}

