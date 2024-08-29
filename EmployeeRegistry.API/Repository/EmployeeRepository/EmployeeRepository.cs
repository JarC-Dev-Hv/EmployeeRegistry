using Microsoft.EntityFrameworkCore;
using EmployeeRegistry.API.Models;
using EmployeeRegistry.API.DTOs;

namespace EmployeeRegistry.API.Repository.EmployeeRepository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _context;

        public EmployeeRepository(EmployeeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAll() =>
            await _context.Employees
                .ToListAsync();

        public async Task<Employee> GetById(int id) =>
            await _context.Employees
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task Add(Employee employee) =>
            await _context.Employees.AddAsync(employee);

        public void Update(Employee employee)
        {
            _context.Employees.Attach(employee);
            _context.Employees.Entry(employee).State = EntityState.Modified;
        }

        public void Delete(Employee employee) =>
            _context.Employees.Remove(employee);

        public async Task Save() =>
            await _context.SaveChangesAsync();


        public async Task<IEnumerable<Employee>> Search(EmployeeSearchDto searchParams)
        {
            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(searchParams.FirstName))
            {
                query = query.Where(e => e.FirstName.Contains(searchParams.FirstName));
            }

            if (!string.IsNullOrEmpty(searchParams.LastName))
            {
                query = query.Where(e => e.LastName.Contains(searchParams.LastName));
            }

            if (searchParams.BirthDate.HasValue)
            {
                query = query.Where(e => e.BirthDate == searchParams.BirthDate.Value);
            }

            if (searchParams.MinSalary.HasValue)
            {
                query = query.Where(e => e.Salary >= searchParams.MinSalary.Value);
            }

            if (searchParams.MaxSalary.HasValue)
            {
                query = query.Where(e => e.Salary <= searchParams.MaxSalary.Value);
            }

            return await query
                .Skip((searchParams.PageNumber - 1) * searchParams.PageSize)
                .Take(searchParams.PageSize)
                .ToListAsync();
        }
    }
}
