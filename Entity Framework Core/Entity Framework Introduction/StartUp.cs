using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                string result = RemoveTown(context);
                Console.WriteLine(result);
            }
        }

        //Problem 3
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var allEmployee = context.Employees
                .Select(e => new
                {
                    Id = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    MiddleName = e.MiddleName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary
                })
                .OrderBy(e => e.Id);

            foreach (var employee in allEmployee)
            {
                sb.AppendLine(
                    $"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }

            return sb.ToString().Trim();
        }

        //Problem 4
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var allEmployees = context.Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    Salary = e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ToList();

            foreach (var employee in allEmployees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            return sb.ToString().Trim();
        }

        //Problem 5
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var exrtractEmployees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    DeparmentName = e.Department.Name,
                    Salary = e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList();

            foreach (var employee in exrtractEmployees)
            {
                sb.AppendLine(
                    $"{employee.FirstName} {employee.LastName} from {employee.DeparmentName} - ${employee.Salary:F2}");
            }

            return sb.ToString().Trim();
        }

        //Problem 6
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Addresses.Add(newAddress);

            Employee nakovEmployee = context.Employees
                .First(e => e.LastName == "Nakov");

            nakovEmployee.Address = newAddress;

            context.SaveChanges();

            var allEmployees = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => e.Address.AddressText)
                .Take(10)
                .ToArray();

            foreach (var employee in allEmployees)
            {
                sb.AppendLine(employee);
            }

            return sb.ToString().Trim();
        }

        //Problem 7
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var allEmoloyees = context.Employees
                .Where(e =>
                    e.EmployeesProjects
                    .Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    FullName = e.FirstName + " " + e.LastName,
                    ManagerName = e.Manager.FirstName + " " + e.Manager.LastName,
                    Projects = e.EmployeesProjects
                        .Select(ep => new
                        {
                            ProjectName = ep.Project.Name,
                            StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt"),
                            EndDate = ep.Project.EndDate
                        })
                })
                .Take(10);

            foreach (var employee in allEmoloyees)
            {
                sb.AppendLine($"{employee.FullName} - Manager: {employee.ManagerName}");

                foreach (var project in employee.Projects)
                {
                    sb.Append($"--{project.ProjectName} - {project.StartDate} - ");

                    if (project.EndDate == null)
                    {
                        sb.AppendLine("not finished");
                    }
                    else
                    {
                        sb.AppendLine($"{project.EndDate:M/d/yyyy h:mm:ss tt}");
                    }
                }
            }

            return sb.ToString().Trim();
        }

        //Problem 8
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addressByTown = context.Addresses
                .OrderByDescending(e => e.Employees.Count)
                .ThenBy(e => e.Town.Name)
                .ThenBy(e => e.AddressText)
                .Select(e => new 
                { 
                    e.AddressText,
                    TownName = e.Town.Name,
                    EmployeeCount = e.Employees.Count
                })
                .Take(10)
                .ToList();

            foreach (var addresses in addressByTown)
            {
                sb.AppendLine($"{addresses.AddressText}, {addresses.TownName} - {addresses.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 9
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee = context.Employees
                .Include(ep => ep.EmployeesProjects)
                .FirstOrDefault(e => e.EmployeeId == 147);

            sb.AppendLine(
                $"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var projectName in employee.EmployeesProjects
                .Join(context.Projects,
                    ep => ep.ProjectId,
                    p => p.ProjectId,
                    (ep, p) => new 
                    { 
                        p.Name
                    })
                .OrderBy(p => p.Name))
            {
                sb.AppendLine($"{projectName.Name}");
            }

            return sb.ToString().Trim();
        }

        //Problem 10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments
                .Include(d => d.Manager)
                .Include(d => d.Employees)
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name);

            foreach (var dep in departments)
            {
                sb.AppendLine($"{dep.Name} - {dep.Manager.FirstName} {dep.Manager.LastName}");

                foreach (var emp in dep.Employees
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName))
                {
                    sb.AppendLine($"{emp.FirstName} {emp.LastName} {emp.JobTitle}");
                }
            }

            return sb.ToString().Trim();
        }

        //Problem 11
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var latest10projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .ToList();

            foreach (var project in latest10projects)
            {
                sb.AppendLine($"{project.Name}");
                sb.AppendLine($"{project.Description}");
                sb.AppendLine($"{project.StartDate:M/d/yyyy h:mm:ss tt}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeeToIncrease = context.Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" ||
                            e.Department.Name == "Marketing" || e.Department.Name == "Information Services");

            foreach (var emp in employeeToIncrease)
            {
                emp.Salary += emp.Salary * 0.12m;
            }

            context.SaveChanges();

            var increasedEmp = context.Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" ||
                            e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName);

            foreach (var emp in increasedEmp)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} (${emp.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesWithSa = context.Employees
                .Where(e => e.FirstName.ToLower().StartsWith("sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName);

            foreach (var emp in employeesWithSa)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle} - (${emp.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 14
        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projectToDelete = context.EmployeesProjects
                .Where(e => e.ProjectId == 2);

            context.EmployeesProjects.RemoveRange(projectToDelete); //FK

            var deletedProject = context.Projects
                .FirstOrDefault(p => p.ProjectId == 2);

            context.Projects.Remove(deletedProject); //PK
            context.SaveChanges();

            var projects = context.Projects
                .Take(10);

            foreach (var pr in projects)
            {
                sb.AppendLine($"{pr.Name}");
            }

            return sb.ToString().TrimEnd();

        }

        //Problem 15
        public static string RemoveTown(SoftUniContext context)
        {
            var nullEmpAddresses = context.Employees
                .Include(e => e.Address)
                .Where(e => e.Address.Town.Name == "Seattle");

            foreach (var addr in nullEmpAddresses)
            {
                addr.AddressId = null;
            }

            context.SaveChanges();

            var addressesToRemove = context.Addresses
                .Where(a => a.Town.Name == "Seattle");

            context.Addresses.RemoveRange(addressesToRemove);

            int countDeleted = context.SaveChanges();

            var townToRemove = context.Towns
                .FirstOrDefault(t => t.Name == "Seattle");

            context.Towns.Remove(townToRemove);

            context.SaveChanges();

            return $"{countDeleted} addresses in Seattle were deleted";
        }
    }
}
