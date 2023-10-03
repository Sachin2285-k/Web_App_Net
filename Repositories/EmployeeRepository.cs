using Microsoft.EntityFrameworkCore;
using Web_App.Data;
using Web_App.Models;
using Web_App.Repositories.Interfaces;

namespace Web_App.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            var data = await _context.Employees.ToListAsync();
            return data;
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) throw new Exception($"Employee with ID:{id} not found.");
            return employee;
        }

        public async Task<Employee> CreateEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }
        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            try
            {
                var recordInDb = _context.Employees.Find(employee.Id);


                if (recordInDb == null)
                {
                    throw new Exception($"No such record in database!");
                
                }
                recordInDb.Name = employee.Name;
                recordInDb.Email = employee.Email;
                recordInDb.Address = employee.Address;
                _context.Employees.Update(recordInDb);
                await _context.SaveChangesAsync();
                return employee;
            }
            catch (Exception ex) {
                throw ex;
            }
        } 
        public async Task<string> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null) throw new Exception($"Employee with ID:{id} not found."); 

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return $"Data with Id:{id} Deleted successfully!";
        }
    }
}
