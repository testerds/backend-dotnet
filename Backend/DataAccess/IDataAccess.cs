using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DataAccess
{
    public interface IDataAccess
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserByUsername(string userName);
        Task<bool> CreateUser(User user);
        Task<bool> DeleteUserById(Guid userId);
    }
}
