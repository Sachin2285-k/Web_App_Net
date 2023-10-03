using Web_App.Models;

namespace Web_App.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployees();
        Task<Employee> CreateEmployee(Employee employee);
        Task<Employee> GetEmployeeById(int id);
        Task<Employee> UpdateEmployee(Employee employee); 
        Task<string> DeleteEmployee(int id);
    }
}
