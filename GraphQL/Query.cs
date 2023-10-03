using HotChocolate.Authorization;
using Microsoft.AspNetCore.Mvc;
/*using Microsoft.AspNetCore.Authorization;*/
using Web_App.Models;
using Web_App.Repositories.Interfaces;

namespace Web_App.GraphQL
{
   
    public class Query
    {
        [Authorize(Policy = "UserIdPolicy")]
        public Task<IEnumerable<Employee>> GetAllEmployees([Service] IEmployeeRepository employeeRepository)
        {
            return employeeRepository.GetAllEmployees();
        }
        public Task<Employee> GetEmployeeById([Service] IEmployeeRepository employeeRepository, int id)
        {
            return employeeRepository.GetEmployeeById(id);
        }

        // User Authentication
        public User GetAuthenticated([Service] IUserRepository userRepository, UserInput userInput)
        {
            return userRepository.Authenticate(userInput.UserName, userInput.Password);
        }
    }
}
