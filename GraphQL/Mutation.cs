using Microsoft.AspNetCore.Mvc;
using System.Net;
using Web_App.Models;
using Web_App.Repositories.Interfaces;

namespace Web_App.GraphQL
{
    public class Mutation
    {
        public Task<Employee> CreateEmployee([Service] IEmployeeRepository employeeRepository, Employee employee)
        {
            return employeeRepository.CreateEmployee(employee);
        }
        public Task<Employee> UpdateEmployee([Service] IEmployeeRepository employeeRepository, Employee employee)
        {
            return employeeRepository.UpdateEmployee(employee);
        }
        public Task<string> DeleteEmployee([Service] IEmployeeRepository employeeRepository ,int id)
        {
            return employeeRepository.DeleteEmployee(id);
        }

        // User Register
        public User GetRegistered([Service] IUserRepository userRepository, UserInput userin)
        {
            var user = userRepository.UniqueUser(userin.UserName);
            if(user == false)
            {
                throw new InvalidOperationException("User already exists!");
            }

            return userRepository.Register(userin.UserName, userin.Password);
        }
    }
}
